# Setting Up The Sui Integration Demo

Welcome to the Beamable SUI Sample project that demonstrates how
to integrate the [SUI](https://sui.io/) services into a [Beamable](https://beamable.com/) powered game using the Unreal Engine. We'll
use two main Beamable federation features:

- [Federated Authentication](https://github.com/beamable/FederatedAuthentication) - use SUI non-custodial wallet or automatically
  create one for each player
- [Federated Inventory](https://github.com/beamable/FederatedInventory) - mint NFTs for federated inventory items
  and FTs for federated inventory currency
- 
#### Support Disclaimer
This sample will not be supported on future Unreal updates, but you can use it as a sample for your own project. The Sui integration is not a Beamable feature but rather a custom implementation that uses the Beamable SDK to interact with the Sui blockchain.

# Getting Started
The sample already has the correct Beamable SDK and Beamable CLI installed, also the microservices have already been linked to the Unreal project.

## Copy Realm Content

1. Sign in to your account using the `dotnet beam init` command.
2. Then command `dotnet beam content replace-local --from DE_1897805636893893 --to YOUR_REALM_ID` to bring all the content from the sample to your current realm
3. Publish the content in the Content Tab (or run a `dotnet beam content publish`).

## Setting up the Sample Project
To set up an organization and realm to run this sample, follow the steps below.

1. Go to the Beamable Portal and create a new Beamable realm called `liveops-demo-sui`
2. Compile and open the `BeamableUnreal` editor project.
3. Sign into your Beamable account and go to the `liveops-demo-sui` realm.
    1. Optionally you can hit `Apply to Build` after the realm change is done.
4. After Signin in your account, you will need to run the command `dotnet beam content replace-local --from DE_1853976001305603 --to YOUR_REALM_ID` to bring all the content from the sample to your current realm
    1. Publish the content in the Content Tab.
5. Let's Setup the Content
    1. Open the `Content` window.
    2. Ensure there's two `SuiCoins` currencies with the name `beam` and `stars`
    3. Ensure there is one `SuiGameCoins` currency with the name `gold`
    4. Ensure there is two `SuiWeaponsItems` content with the name `darksaber` and `shisui`
    5. Make sure the Items have those parameters as informed in the table below:

6. Click `Publish` to publish those new contents to the realm.

## Running the Sample in Editor
Now you're set up to run the sample.

1. Open the Docker Desktop and Make sure it's running
2. From a terminal Open the directory `microservices/services/SuiFederation` and run `npm install`
3. Open a terminal and run `dotnet beam services run --ids SuiFederation`
4. Open the Unreal Editor
5. Go to the `Beamable -> Microservice` window. You should see the `SuiFederation` service running there.
6. Reset PIE Users to clear any existing data.
    1. Go to the `Home` window.
    2. Click on `Reset PIE Users`.
7. Open the `LiveOpsDemo` Level if it's not opened yet.
    1. You can find it inside the `BEAMPROJ_LiveOpsDemo Content`  folder.
    2. If you can't see plugin content in your content browser, you can change the settings of the UE `Content Browser` to display it.
8. Play the `LiveOpsDemo` in the Editor.

# About the Sample

- The sample project includes several important files that will help guide you through the sample flow.
    - Blueprint handles almost everything not related to SUI. We're using it primarily for **Beamable SDK** integration and UI/widget management.
    - Be sure to take a look at the **WBP_LiveOpsUI Blueprint** — it contains the core flow and logic for the sample.
    - On the C++ side, the **USuiWalletSigner** class in the **BEAMPROJ_LiveOpsDemo** is responsible for all SUI-related functionality.

- We are also using two external libraries for cryptographic operations:
    - [BLAKE2](https://github.com/BLAKE2/BLAKE2) for Blake2b hashing
    - [compact25519](https://github.com/DavyLandman/compact25519) for Ed25519 signature operations


## Sample Features
The example project implements the following SUI features:
- creating a custodial SUI wallet for the player account
- attaching a Slush wallet to the player account
- attaching an Enoki wallet to the player account
- automatic smart contract deployment
- support for the following contract types:
    - Non-fungible token/NFT
    - Fungible token/Regular currency token
    - Fungible token/Closed loop token
- transfer currency from a custodial to an external wallet
- a federated kiosk: list, delist, and purchase federated
- nest Nfts: attaching one federated Nft to another


## Custodial SUI wallet
Using Beamable's [federated authentication feature](https://github.com/beamable/FederatedAuthentication), the microservice can create a custodial SUI wallet for the player.
The implementation can be seen [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Endpoints/AuthenticateEndpoint.cs)

## Attaching a Slush wallet and Enoki wallet
Using Beamable's [federated authentication feature](https://github.com/beamable/FederatedAuthentication), the microservice can attach an external wallet to the player's account. Initiating the attached flow is specific for the platform (Unity or Unreal) but the backend implementation is the same. It works in a 2FA fashion, first by generating a message to sign, and then verifying the signature.
The Slush implementation can be seen [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Endpoints/AuthenticateExternalEndpoint.cs)
The Enoki implementation can be seen [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Endpoints/AuthenticateEnokiEndpoint.cs)

### Implementation Note:
The Slush Wallet integration is currently not fully done in Unreal Engine due to constraints within Unreal's embedded browser that prevent complete functionality. Specifically, the challenge lies in handling the signed message callback return to the engine.

However, full functionality can be achieved by utilizing an external browser in conjunction with one of the following approaches:
- Js/native bridge
- WebSocket communication through Beamable infrastructure


## Non-fungible token/NFT
NFT support in the microservice implements the following functionalities:
- dynamic smart contract creation
- NFT minting and burning
- Dynamic NFTs (updating NFT attributes)

Each NFT item can be minted, burned or updated through inventory service update operations in a game authoritative fashion through sponsored transactions. Beamable's federated inventory process is explained [here.](https://github.com/beamable/FederatedInventory)  
The example project contains an example of NFT game token item called `weapon`, which is defined in [`here.`](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederationCommon/FederationContent/WeaponItem.cs)
Other custom NFT definition can be added by defining a new type which implements [`INftBase`](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederationCommon/Models/NftBase.cs) interface.
You can create your own custom federated content items by following the `weapon` as an example.   
Microservice will deploy smart contracts for each Beamable content item created based on the `INftBase` interface implementation.  
Content item definition supports the following properties:
- name
- image
- description
- attributes: key-value pairs

Smart contract readme can be found [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Features/Contract/Templates/nft.MD)

## Fungible token/Regular currency token
Regular currency token support in the microservice implements the following functionalities:
- dynamic smart contract creation
- defining coin initial supply
- minting and burning

Each FT supports minting and burning coins through sponsored transactions.
The project contains an example of federated coin item called `CoinCurrency`, which is defined [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederationCommon/FederationContent/CoinCurrency.cs)  
You can creat your own custom federated content items by following the `CoinCurrency` as an example.   
Microservice will deploy smart contracts for each Beamable content item created.

## Fungible token/Closed loop token
Closed loop token support in the microservice implements the following functionalities:
- dynamic smart contract creation
- defining coin initial supply and allowed actions
- minting and burning

Each FT supports minting and burning coins through sponsored transactions.
The project contains an example of a closed loop item called `InGameCurrency`, which is defined [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederationCommon/FederationContent/InGameCurrency.cs)  
You can creat your own custom federated content items by following the `InGameCurrency` as an example.   
Microservice will deploy smart contracts for each Beamable content item created.      
Content item definition supports the following additional properties:
- initial supply
- optional token actions: spending, buying, transfers

## Kiosk
The kiosk feature allows players to list, delist and purchase federated items. The microservice exposes several kiosk [endpoints](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/SuiFederationKiosk.cs) to handle the kiosk operations.

## Nested NFTs
The nest NFT feature allows players to nest federated NFTs. The microservice exposes nestable federated items [endpoints](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederationCommon/FederationContent/CreatureDna.cs) which depends on `INftAddon` interface.
The microservice handles all the logic for nesting and detaching NFTs.
Attaching an NFT is done by simply updating the parent NFT with the child NFT id using a `string, string` key-value pair
where the key is `$attach` and the value is the `ChildNft content id`. This uses Beamable inventory service.
The child NFT is then automatically attached to the parent NFT and granted to the player's inventory.
Detaching is done in a similar fashion but with the key being `$detach` and the value being the `ChildNft content id`.

# Can I create my own custom federated content?
Yes, absolutely, and it is a two-step process:
1. Following the existing [content examples](https://github.com/beamable/Sui-MonoRepo/tree/main/UnrealSample/Microservices/services/SuiFederationCommon/FederationContent), you can create your own custom federated content items in the microservices.
2. In Unreal and under the C++ content folder (source/private/content and source/public/content), you will replicate the created item in step 1 in C++. You need to ensure that the content type string is the same in both places. You can follow the [existing examples](https://github.com/beamable/Sui-MonoRepo/tree/main/UnrealSample/Source/UnrealSuiSample/Public/Content) for reference.
Once done correctly, you will be able to use the new content from the `Beamable Content Manager`.

# Can I integrate SUI into my own Unreal Project?
The process is fairly easy and straight forward following these steps:
1. Update your project with the [Unreal Beamable SDK version 2.0.1](https://beamable.github.io/docs/Unreal-Latest/unreal/getting-started/setup/). This is the supported sdk for SUI integration. A quick side note for this step, once you reach `Set up the Beamable SDK - Fash Path / 2`, when adding the path to the `game-maker` command, you MUST add a `/` at the end of the path to make it work properly.
2. Copy the microservices folder from the [SUI-MonoRepo](https://github.com/beamable/Sui-MonoRepo/tree/main/UnrealSample/Microservices) to your project.
3. Link the microservices to your project using the CLI command in the root of your Unreal project `dotnet beam project add-Unreal-project "."`.
4. Deploy the services either locally or remotely

This will allow you to use the SUI integration in your project.