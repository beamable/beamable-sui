# Personal Kiosk Functions Documentation

This document explains each personal kiosk function and what it does on the Sui blockchain.

## Overview

These functions utilize the **Sui Kiosk SDK** to interact with the Sui blockchain's kiosk system. The implementation uses custom deployed contracts for enhanced functionality:

### Custom Contracts

The microservice deploys and maintains local versions of key contracts:

- **personal_kiosk contract**: A custom implementation that replaces the standard Sui personal kiosk contract
- **royalty_rule contract**: A custom royalty handling implementation that replaces the standard Sui royalty rule contract

These custom contracts are deployed by the microservice and their addresses are used to configure the `KioskClient`. The `CustomPackageIds` parameter in requests allows specifying these custom contract addresses:

- `personalKioskRulePackageId`: Address of the deployed personal_kiosk contract
- `royaltyRulePackageId`: Address of the deployed royalty_rule contract

If custom package IDs are not provided, the functions fall back to the standard Sui contract addresses defined in `PERSONAL_KIOSK_RULE_ADDRESS` and `ROYALTY_RULE_ADDRESS` based on the network environment.

This architecture allows the microservice to use enhanced or modified kiosk functionality while maintaining compatibility with the standard Sui Kiosk SDK.

---

## personalKioskCreate

**What it does:**

Creates a new personal kiosk on Sui for a user. A personal kiosk is a marketplace storefront where users can store and sell their digital assets (NFTs).

**Process:**
1. Creates a new transaction on Sui
2. Initializes a kiosk client to interact with the Sui kiosk system
3. Creates a personal kiosk using `kioskTx.createPersonal()`
4. Signs and executes the transaction with the player's wallet
5. Extracts two important objects from the transaction results:
    - **Kiosk**: The actual marketplace storefront (identified by type `0x2::kiosk::Kiosk`)
    - **PersonalKioskCap**: A capability object that proves ownership and grants control over the kiosk (identified by type ending in `personal_kiosk::PersonalKioskCap`)

**Why it matters:** The PersonalKioskCap is like a key to your kiosk - you need it for all future operations.

---

## personalKioskList

**What it does:**

Lists an item for sale in your personal kiosk. This function supports two listing modes: public listings (anyone can buy) and exclusive listings (only a specific buyer can purchase).

**Process for public listings:**
1. Uses the kiosk client to place the item in the kiosk and list it for sale at the specified price
2. The `placeAndList` operation handles both storing the item and making it available for purchase

**Process for exclusive listings:**
1. Borrows the kiosk owner capability from the PersonalKioskCap using the custom `personal_kiosk::borrow_val` function
2. Calls `list_with_purchase_cap` to create a special listing with a specific buyer address
3. This generates a **PurchaseCap** (purchase capability) that only the designated buyer can use
4. Returns the capability back to the PersonalKioskCap using `personal_kiosk::return_val`
5. Finds and records the created listing and PurchaseCap in the transaction results

**Why it matters:** Exclusive listings enable peer-to-peer trading where you can offer items to specific people, preventing sniping by others.

---

## personalKioskDelist

**What it does:**

Removes an item from sale in your kiosk and sends it back to your wallet. This is a two-step process.

**Process:**
1. **Delist**: Removes the item from being listed for sale in the kiosk
2. **Transfer**: Moves the item from the kiosk storage to the owner's personal wallet address

**Why it matters:** This allows you to take back items you no longer want to sell, making them fully yours again outside the kiosk system.

---

## personalKioskTake

**What it does:**

Withdraws an item from your kiosk directly to your wallet. Unlike delist, this is used for items that aren't currently listed for sale.

**Process:**
1. Transfers the item from kiosk storage to the owner's wallet address
2. No delisting step needed since the item wasn't listed for sale

**Why it matters:** This lets you retrieve items you're storing in your kiosk but haven't put up for sale.

---

## personalKioskDeclinePurchase

**What it does:**

Allows a buyer to reject an exclusive purchase offer by returning the PurchaseCap back to the seller.

**Process:**
1. Transfers the PurchaseCap object back to the seller's address
2. Uses a sponsored transaction (the gas fees are paid by a sponsor, not the buyer)

**Why it matters:** If someone offers you an exclusive deal but you don't want it, you can politely decline and give them back the ability to offer it to someone else.

---

## personalKioskCancelExclusive

**What it does:**

Allows the seller to cancel an exclusive listing, retrieve the item, and invalidate the PurchaseCap.

**Process:**
1. Gets the item ID from the PurchaseCap using `purchase_cap_item`
2. Returns the PurchaseCap to the kiosk using `return_purchase_cap`, which invalidates it
3. Borrows the kiosk owner capability using the custom `personal_kiosk::borrow_val` function
4. Takes the item out of the kiosk using the standard `take` function
5. Returns the capability using `personal_kiosk::return_val`

**Why it matters:** If you made an exclusive offer to someone but changed your mind, this lets you cancel the deal and get your item back.

---

## personalKioskPurchase

**What it does:**

Buys an item from a kiosk. Handles both public listings (anyone can buy) and exclusive listings (requires a PurchaseCap).

**Process for public purchases:**
1. Uses `purchaseAndResolve` which automatically handles:
    - Purchasing the item from the seller's kiosk
    - Paying the royalties (if any)
    - Resolving all transfer policies
    - Moving the item to your kiosk

**Process for exclusive purchases:**
1. Splits the payment into two coins: one for the purchase, one for royalties
2. Uses `purchase_with_cap` with the PurchaseCap to buy the item
3. Manually pays royalties using the custom `royalty_rule::pay` function
4. Confirms the transfer request to complete the transaction
5. Transfers the purchased item to the buyer's wallet

**Why it matters:** This is how you actually buy NFTs from kiosks. The function handles all the complex payment routing including royalties to creators.

---

## personalKioskWithdraw

**What it does:**

Withdraws profits (SUI tokens) from your kiosk to your personal wallet.

**Process:**
1. Uses the kiosk client's `withdraw` function
2. Specifies the owner's address as the destination
3. Specifies the amount to withdraw

**Why it matters:** When you sell items in your kiosk, the payment stays in the kiosk. This function lets you take those profits out and use them.

---

## Key Concepts

**Personal Kiosk**: A personal storefront on Sui where you can list NFTs for sale.

**PersonalKioskCap**: Your ownership proof and control key for the kiosk.

**PurchaseCap**: A special token that grants exclusive purchase rights to a specific buyer.

**Royalties**: Automatic payments to original creators when items are resold.

**Transfer Policies**: Rules that must be satisfied when transferring items (like paying royalties).

**Kiosk SDK**: The official Sui Kiosk SDK used for blockchain interactions.

**Custom Contracts**: Locally deployed personal_kiosk and royalty_rule contracts that extend or modify standard Sui kiosk functionality.