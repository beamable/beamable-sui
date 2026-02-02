#[allow(unused_use,duplicate_alias,unused_variable,unused_const,unused_function,lint(self_transfer))]
module {{toLowerCase Name}}_package::{{toLowerCase Name}} {
    use sui::kiosk::{Self, Kiosk, KioskOwnerCap};
    use sui::tx_context::{TxContext};
    use sui::coin::{Self, Coin};
    use sui::token::{Self, Token, TokenPolicy};

    use {{toLowerCase NftName}}_package::{{toLowerCase NftName}}::{ Self as {{toLowerCase NftName}}, {{toStructName NftName}} };
    use {{toLowerCase CoinName}}_package::{{toLowerCase CoinName}}::{ Self as {{toLowerCase CoinName}}, {{toUpperCase CoinName}} };

    // === Errors ===
    const ENotAdmin: u64 = 0;
    const EWrongPrice: u64 = 1;
    const ENotTheSeller: u64 = 2;
    const EMarketplaceIsPaused: u64 = 3;

    /// The capability that allows administering the marketplace.
    /// Created once in the `init` function.
    public struct {{toStructName Name}}MarketplaceAdminCap has key, store {
        id: UID
    }

    /// The main shared object for the marketplace.
    /// It holds the Kiosk and its capability, acting as a trusted custodian.
    public struct {{toStructName Name}}Marketplace has key {
        id: UID,
        kiosk: Kiosk,
        kiosk_cap: KioskOwnerCap,
        is_paused: bool
    }

    public struct {{toStructName Name}}Listing has key, store {
        id: UID,
        item_id: ID,
        seller: address,
        price: u64,
    }

    /// This function is called once when the package is published.
    /// It creates the central Marketplace object.
    fun init(ctx: &mut TxContext) {
        let (kiosk, kiosk_cap) = kiosk::new(ctx);

        // Create the marketplace object and store the kiosk and its cap inside.
        let marketplace = {{toStructName Name}}Marketplace {
            id: object::new(ctx),
            kiosk,
            kiosk_cap,
            is_paused: false
        };

        // Transfer the admin cap to the deployer and share the marketplace.
        transfer::public_transfer({{toStructName Name}}MarketplaceAdminCap { id: object::new(ctx) }, tx_context::sender(ctx));
        transfer::public_share_object(marketplace);
    }

    /// A seller lists an item by transferring it to the marketplace.
    public fun list(
        marketplace: &mut {{toStructName Name}}Marketplace,
        item: {{toStructName NftName}},
        price: u64,
        ctx: &mut TxContext
    ) {
        assert!(!marketplace.is_paused, EMarketplaceIsPaused);
        let item_id = object::id(&item);

        // The marketplace uses its OWN cap to place the item in its OWN kiosk.
        kiosk::place(&mut marketplace.kiosk, &marketplace.kiosk_cap, item);

        // Create and share the listing object.
        let listing = {{toStructName Name}}Listing {
            id: object::new(ctx),
            item_id,
            seller: tx_context::sender(ctx),
            price
        };
        transfer::public_share_object(listing);
    }

    /// The original seller delists their item.
    public fun delist(
        marketplace: &mut {{toStructName Name}}Marketplace,
        listing: &mut {{toStructName Name}}Listing,
        ctx: &mut TxContext
    ) {
        assert!(!marketplace.is_paused, EMarketplaceIsPaused);
        let {{toStructName Name}}Listing { id, item_id, seller, price: _ } = listing;
        assert!(tx_context::sender(ctx) == seller, ENotTheSeller);
        object::delete(id);

        // Marketplace uses its own cap to take the item and return it to the seller.
        let item: {{toStructName NftName}} = kiosk::take(
            &mut marketplace.kiosk,
            &marketplace.kiosk_cap,
            item_id
        );
        transfer::transfer(item, seller);
    }

    /// A buyer purchases an item from the marketplace.
    {{#if IsRegularCoin}}
      public fun purchase(
              marketplace: &mut {{toStructName Name}}Marketplace,
              listing: &mut {{toStructName Name}}Listing,
              payment: Coin<{{toUpperCase CoinName}}>,
              ctx: &mut TxContext
          ) {
                assert!(!marketplace.is_paused, EMarketplaceIsPaused);
                let {{toStructName Name}}Listing { id, item_id, seller, price } = listing;
                object::delete(id);

                let payment_amount = coin::value(&payment);
                assert!(payment_amount >= price, EWrongPrice);

                // The marketplace uses its OWN cap to take the item from its OWN kiosk.
                let item: {{toStructName NftName}} = kiosk::take(
                    &mut marketplace.kiosk,
                    &marketplace.kiosk_cap,
                    item_id
                );

                // Transfer NFT to buyer
                transfer::transfer(item, tx_context::sender(ctx));

                // Handle payment: split if overpaid, otherwise transfer full amount
                if (payment_amount > price) {
                    // Split the payment: exact amount to seller, change back to buyer
                    let change = coin::split(&mut payment, payment_amount - price, ctx);
                    
                    // Return change to buyer
                    transfer::public_transfer(change, tx_context::sender(ctx));
                    
                    // Send exact payment to seller (payment now has exact price value)
                    transfer::public_transfer(payment, seller);
                } else {
                    // Exact payment: transfer full amount to seller
                    transfer::public_transfer(payment, seller);
                }
          }
    {{else}}

      public fun purchase(
              marketplace: &mut {{toStructName Name}}Marketplace,
              listing: &mut {{toStructName Name}}Listing,
              policy: &mut TokenPolicy<{{toUpperCase CoinName}}>,
              payment: Token<{{toUpperCase CoinName}}>,
              ctx: &mut TxContext
          ) {
                assert!(!marketplace.is_paused, EMarketplaceIsPaused);
                let {{toStructName Name}}Listing { id, item_id, seller, price } = listing;
                object::delete(id);

                let payment_amount = token::value(&payment);
                assert!(payment_amount >= price, EWrongPrice);

                // The marketplace uses its OWN cap to take the item from its OWN kiosk.
                let item: {{toStructName NftName}} = kiosk::take(
                    &mut marketplace.kiosk,
                    &marketplace.kiosk_cap,
                    item_id
                );

                // Transfer NFT to buyer
                transfer::transfer(item, tx_context::sender(ctx));

                // Handle payment: split if overpaid, otherwise transfer full amount
                if (payment_amount > price) {
                    // Split the payment: exact amount to seller, change back to buyer
                    let (payment_exact, change) = token::split(payment, price, ctx);
                    
                    // Return change to buyer
                    let change_request = token::transfer(change, tx_context::sender(ctx), ctx);
                    token::confirm_request(policy, change_request, ctx);
                    
                    // Send exact payment to seller
                    let payment_request = token::transfer(payment_exact, seller, ctx);
                    token::confirm_request(policy, payment_request, ctx);
                } else {
                    // Exact payment: transfer full amount to seller
                    let payment_request = token::transfer(payment, seller, ctx);
                    token::confirm_request(policy, payment_request, ctx);
                }
          }
    {{/if}}


    /// Admin function to pause or unpause the marketplace.
        public fun set_pause(
            _: &{{toStructName Name}}MarketplaceAdminCap,
            marketplace: &mut {{toStructName Name}}Marketplace,
            paused: bool
        ) {
            marketplace.is_paused = paused;
        }
}