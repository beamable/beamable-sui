#[allow(unused_use,duplicate_alias,unused_variable,unused_const,unused_function)]
module {{toLowerCase Name}}_package::{{toLowerCase Name}} {

    use sui::object::{Self as object, UID};
    use sui::url::{Self, Url};
    use std::string::{Self, String, utf8};
    use sui::tx_context::{sender};
    use sui::tx_context::{Self as TxContext};
    use sui::dynamic_field as df;
    use sui::package;
    use sui::display;
    use std::vector as vec;
    use sui::ed25519;
    use sui::clock::{Self, Clock};
    use sui::transfer_policy::{Self, TransferPolicy, TransferPolicyCap};
    use {{toLowerCase ParentName}}_package::{{toLowerCase ParentName}}::{ Self as {{toLowerCase ParentName}}, {{toStructName ParentName}} };

    /// Ensure NFT metadata (Attributes) vector properties length
    const EVecLengthMismatch: u64 = 1;
    const EInvalidSignature: u64 = 2;
    const ESignatureExpired: u64 = 3;
    const EInvalidNonce: u64 = 4;
    const EAttachmentAlreadyExists: u64 = 5;

    const DOMAIN_ADD_ATTACHMENT: vector<u8> = b"ADD_ATTACHMENT";
    const DOMAIN_REMOVE_ATTACHMENT: vector<u8> = b"REMOVE_ATTACHMENT";
    const DOMAIN_UPDATE_ATTACHMENT: vector<u8> = b"UPDATE_ATTACHMENT";

    public struct AdminCap has key { id: UID }

    /// NFT one-time witness (guaranteed to have at most one instance), name matches the module name
    public struct {{toUpperCase Name}} has drop {}

    /// NFT metadata
    public struct Attribute has store, copy, drop {
        name: String,
        value: String
    }

    /// NFT Struct
    public struct {{toStructName Name}} has key {
        id: UID,
        name: String,
        description: String,
        url: Url,
        contentId: String,
        attributes: vector<Attribute>,
        nonce: u64,
    }

    // Struct to store the contract owner
    public struct OwnerInfo has key {
        id: UID,
        address: address,
        public_key: vector<u8>,
    }

    /// Called on contract publish, defines NFT display properties
    fun init(otw: {{toUpperCase Name}}, ctx: &mut TxContext) {
        let keys = vector[
            utf8(b"name"),
            utf8(b"description"),
            utf8(b"url")
        ];
        let values = vector[
            utf8(b"{name}"),
            utf8(b"{description}"),
            utf8(b"{url}")
        ];

        let publisher = package::claim(otw, ctx);
        let mut display = display::new_with_fields<{{toStructName Name}}>(&publisher, keys, values, ctx);

        let (mut policy, policy_cap) = transfer_policy::new<{{toStructName Name}}>(&publisher, ctx);
        transfer::public_share_object(policy);
        transfer::public_transfer(policy_cap, tx_context::sender(ctx));


        display::update_version(&mut display);
        transfer::public_transfer(publisher, sender(ctx));
        transfer::public_transfer(display, sender(ctx));
        transfer::transfer(AdminCap {id: object::new(ctx)}, tx_context::sender(ctx));
    }

    public entry fun set_owner(
            _: &AdminCap,
            public_key: vector<u8>,
            ctx: &mut TxContext) {
            let owner_info = OwnerInfo {
                id: object::new(ctx),
                address: tx_context::sender(ctx),
                public_key: public_key,
                };
            transfer::share_object(owner_info);
        }

    /// Constructs NFT object
    fun new(
        name: vector<u8>,
        description: vector<u8>,
        url: vector<u8>,
        contentId: vector<u8>,
        names: vector<String>,
        values: vector<String>,
        ctx: &mut TxContext): {{toStructName Name}} {
        let len = vec::length(&names);
        assert!(len == vec::length(&values), EVecLengthMismatch);

        let mut item = {{toStructName Name}} {
            id: object::new(ctx),
            name: string::utf8(name),
            description: string::utf8(description),
            url: url::new_unsafe_from_bytes(url),
            contentId: string::utf8(contentId),
            attributes: vec::empty(),
            nonce: 0
        };

        let mut i = 0;
        while (i < len) {
            vec::push_back(&mut item.attributes, Attribute {
                name: *vec::borrow(&names, i),
                value: *vec::borrow(&values, i)
            });
            i = i + 1;
        };
        item
    }

    fun get_current_time(clock: &Clock): u64 {
        clock::timestamp_ms(clock)
    }

    public entry fun add(
        parent: &mut {{toStructName ParentName}},
        owner: &OwnerInfo,
        clock: &Clock,
        signature: vector<u8>,
        timestamp: u64,
        name: vector<u8>,
        description: vector<u8>,
        url: vector<u8>,
        contentId: vector<u8>,
        names: vector<String>,
        values: vector<String>,
        ctx: &mut TxContext) {
        use sui::bcs;

        assert!(
            !df::exists_({{toLowerCase ParentName}}::uid_mut(parent), contentId),
            EAttachmentAlreadyExists
        );

        // Check timestamp
        let current_time = get_current_time(clock);
        assert!(current_time - timestamp <= 120_000, ESignatureExpired);

        // Build signature payload
        let domain_bytes = DOMAIN_ADD_ATTACHMENT;
        let parent_id_bytes = bcs::to_bytes(&object::id(parent));
        let timestamp_bytes = bcs::to_bytes(&timestamp);

        let mut combined_bytes = vector::empty<u8>();
        vec::append(&mut combined_bytes, domain_bytes);
        vec::append(&mut combined_bytes, parent_id_bytes);
        vec::append(&mut combined_bytes, name);
        vec::append(&mut combined_bytes, url);
        vec::append(&mut combined_bytes, contentId);
        vec::append(&mut combined_bytes, timestamp_bytes);

        // Verify signature
        assert!(
            ed25519::ed25519_verify(&signature, &owner.public_key, &combined_bytes),
            EInvalidSignature
        );

        if (!df::exists_({{toLowerCase ParentName}}::uid_mut(parent), contentId)) {
            let item = new(name,description,url,contentId,names,values,ctx);
            df::add(
                {{toLowerCase ParentName}}::uid_mut(parent),
                contentId,
                item
            );
        }
    }

    public entry fun remove(
        parent: &mut {{toStructName ParentName}},
        owner: &OwnerInfo,
        clock: &Clock,
        signature: vector<u8>,
        timestamp: u64,
        contentId: vector<u8>,
        nonce: u64,  
        ctx: &mut TxContext) {
        use sui::bcs;

        // Check timestamp
        let current_time = get_current_time(clock);
        assert!(current_time - timestamp <= 120_000, ESignatureExpired);

        if (df::exists_({{toLowerCase ParentName}}::uid_mut(parent), contentId)) {
            let item = df::borrow<vector<u8>, {{toStructName Name}}>(
                {{toLowerCase ParentName}}::uid(parent),
                contentId
            );

            assert!(nonce == item.nonce, EInvalidNonce);

            // Build signature payload
            let domain_bytes = DOMAIN_REMOVE_ATTACHMENT;
            let parent_id_bytes = bcs::to_bytes(&object::id(parent));
            let timestamp_bytes = bcs::to_bytes(&timestamp);
            let nonce_bytes = bcs::to_bytes(&nonce);

            let mut combined_bytes = vector::empty<u8>();
            vec::append(&mut combined_bytes, domain_bytes);
            vec::append(&mut combined_bytes, parent_id_bytes);
            vec::append(&mut combined_bytes, contentId);
            vec::append(&mut combined_bytes, timestamp_bytes);
            vec::append(&mut combined_bytes, nonce_bytes);

            // Verify signature
            assert!(
                ed25519::ed25519_verify(&signature, &owner.public_key, &combined_bytes),
                EInvalidSignature
            );

            let item: {{toStructName Name}} = df::remove({{toLowerCase ParentName}}::uid_mut(parent), contentId);
            let {{toStructName Name}} {
                id,
                name: _,
                description: _,
                url: _,
                contentId: _,
                attributes: _,
                nonce: _,
            } = item;
            object::delete(id);
        }
    }

    public entry fun update_attachment(
        parent: &mut {{toStructName ParentName}},
        owner: &OwnerInfo,
        clock: &Clock,
        signature: vector<u8>,
        timestamp: u64,
        nonce: u64,
        contentId: vector<u8>,
        name: String,
        value: String,
        ctx: &mut TxContext) {

        use sui::bcs;

        // Check timestamp
        let current_time = get_current_time(clock);
        assert!(current_time - timestamp <= 120_000, ESignatureExpired);


        if (df::exists_({{toLowerCase ParentName}}::uid_mut(parent), contentId)) {
            let item = df::borrow_mut<vector<u8>, {{toStructName Name}}>({{toLowerCase ParentName}}::uid_mut(parent), contentId);

            // Verify nonce matches THIS attachment's nonce
            assert!(nonce == item.nonce, EInvalidNonce);

            // Build signature payload
            let domain_bytes = DOMAIN_UPDATE_ATTACHMENT;
            let parent_id_bytes = bcs::to_bytes(&object::id(parent));
            let name_bytes = bcs::to_bytes(&name);
            let value_bytes = bcs::to_bytes(&value);
            let timestamp_bytes = bcs::to_bytes(&timestamp);
            let nonce_bytes = bcs::to_bytes(&nonce);

            let mut combined_bytes = vector::empty<u8>();
            vec::append(&mut combined_bytes, domain_bytes);
            vec::append(&mut combined_bytes, parent_id_bytes);
            vec::append(&mut combined_bytes, contentId);
            vec::append(&mut combined_bytes, name_bytes);
            vec::append(&mut combined_bytes, value_bytes);
            vec::append(&mut combined_bytes, timestamp_bytes);
            vec::append(&mut combined_bytes, nonce_bytes);

            // Verify signature
            assert!(
                ed25519::ed25519_verify(&signature, &owner.public_key, &combined_bytes),
                EInvalidSignature
            );            

            // Loop through the existing attributes
            let attributes = &mut item.attributes;
            let mut found = false;

            // Iterate through the attributes to find a match
            let len = vec::length(attributes);
            let mut i = 0;
            while (i < len) {
                let attribute = vec::borrow_mut(attributes, i);
                if (attribute.name == name) {
                    // If the attribute with the given name is found, update its value
                    attribute.value = value;
                    found = true;
                    break
                };
                i = i + 1;
            };

            // If the attribute was not found, add a new one
            if (!found) {
                let new_attribute = Attribute {
                name,
                value
                };
                vec::push_back(attributes, new_attribute);
            }
            item.nonce = item.nonce + 1;
        }
    }

    public fun get(
        parent: &{{toStructName ParentName}},
        contentId: vector<u8>,
        ctx: &mut TxContext): &{{toStructName Name}} {
            df::borrow<vector<u8>, {{toStructName Name}}>({{toLowerCase ParentName}}::uid(parent), contentId)
    }

}