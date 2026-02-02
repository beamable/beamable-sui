# Beamable + Sui Unity Sample

This sample demonstrates how to integrate the Sui blockchain with a Beamable-powered Unity game. It showcases production-oriented patterns for account federation, inventory, and on-chain asset flows across Web, iOS, and Android targets.

The sample uses two primary Beamable federation capabilities:
- Federated Authentication — provision a custodial wallet per player or attach an external wallet.
- Federated Inventory — mint and manage NFTs/FTs that mirror game-authoritative state on-chain.

## Requirements

Before you begin, create accounts with Beamable and Sui. Install the following tooling:

1. Unity 6 (other recent Unity versions are likely compatible)
2. Beamable Unity SDK 3.0.0
3. Docker Desktop
4. .NET 8 SDK
5. Node.js and npm

## Repository structure

- Unity/services — Beamable microservice(s) implementing federation and sample content
- Unity — Unity project with Beamable SDK and example UI that demonstrates end-to-end Sui flows
- wallet-integration — Slush wallet web sample; builds a JavaScript API for Unity WebGL wallet interactions

## Getting Started

This repository already includes the correct Beamable SDK and CLI configuration. The services are linked to the Unity project.

### Copy Realm Content

1. Open a terminal in the Unity folder.
2. Sign in to Beamable:
   ```bash
   dotnet beam init
   ```
3. Replace local content with the sample realm:
   ```bash
   dotnet beam content replace-local --from DE_1860448245192704 --to YOUR_REALM_ID
   ```
4. Publish the content (either via the Unity Editor’s Content tab or CLI):
   ```bash
   dotnet beam content publish
   ```

## Running Locally

You can run the microservices in two modes depending on your iteration needs.

1) As local .NET processes (best for rapid iteration and debugging):
   ```bash
   beam project list
   dotnet beam project run --ids SuiFederation
   ```
    - Attachable with a debugger
    - Stop with CTRL+C or:
      ```bash
      beam project stop --ids SuiFederation
      ```

2) As Docker containers (prod-like parity):
   ```bash
   dotnet beam services run --ids SuiFederation
   ```
    - Mirrors the containerized runtime used in deployment
    - Requires Docker to be installed and available on PATH

## Running Remotely

You can deploy and run the SuiFederation service remotely from an IDE or the CLI:

```bash
bash dotnet beam deploy plan
dotnet beam deploy release --latest-plan
```

Docker must be installed and running.

### BeamableTool

By default, SuiFederation and SuiFederationCommon include the Beam CLI as a project tool. If you have beam installed globally, you can set BeamableTool to beam.

## Sample Features

The sample demonstrates:

- Custodial Sui wallet per player (via Federated Authentication)
- Attaching a Slush wallet to the player account
- Attaching an Enoki wallet to the player account
- Automatic smart contract deployment
- Token support:
    - Non-fungible tokens (NFTs)
    - Fungible tokens (regular currency)
    - Fungible tokens (closed-loop currency)
- Sponsored transfers from custodial to external wallets
- Federated Kiosk: list, delist, purchase
- Nested NFTs: attach/detach NFTs to/from other NFTs

## Custodial Sui Wallet

Using Beamable’s Federated Authentication, the microservice can provision a custodial Sui wallet for each player. The Unity sample’s implementation is described in the repository documentation.

## Attaching Slush and Enoki Wallets

External wallets can be attached to the player account using a two-step flow:
1) Generate a message for the wallet to sign.
2) Verify the signature server-side and complete the association.

Platform specifics differ (Unity vs. Unreal), but the backend flow is identical.

## NFTs (Non‑Fungible Tokens)

The microservice supports:
- Dynamic contract creation
- Minting and burning
- Dynamic NFTs (authoritative updates to attributes)

NFT lifecycle operations are driven by game-authoritative updates through Beamable’s Federated Inventory and executed as sponsored transactions. The sample includes a Weapon item example and an INftBase interface for defining additional NFT-backed content. Content definitions support:
- name
- image
- description
- attributes (key-value pairs)

## FTs (Fungible Tokens) — Regular Currency

The microservice supports:
- Dynamic contract creation
- Initial supply configuration
- Minting and burning

Regular currencies are minted and burned via sponsored transactions. The sample includes a CoinCurrency example you can replicate for your own designs.

## FTs (Fungible Tokens) — Closed Loop

The microservice supports:
- Dynamic contract creation
- Initial supply and allowed action configuration
- Minting and burning

Closed-loop currencies restrict actions (for example: spend, buy, transfer) according to your design. The sample includes an InGameCurrency example.

## Kiosk

The Kiosk feature enables listing, delisting, and purchasing of federated items through microservice endpoints. It’s designed for straightforward integration into in-game marketplaces.

## Nested NFTs

The sample demonstrates nesting (attaching) NFTs using Beamable inventory updates:
- Attach by updating the parent NFT with a key/value pair where the key is $attach and the value is the child NFT content ID.
- Detach using $detach with the child NFT content ID.

When attached, the child is automatically associated with the parent and granted to the player inventory under the rules enforced by the microservice.

## Integrating Sui Into Your Own Unity Project

1. Install the Beamable SDK version 3.0.0.
2. Copy the services folder from this repo into your Unity project root.
3. Link the services to your Unity project:
   ```bash
   dotnet beam project add-Unity-project "."
   ```
4. Deploy services locally or remotely as needed.

After these steps, your project will be set up to use Sui federated authentication, inventory, and kiosk features.