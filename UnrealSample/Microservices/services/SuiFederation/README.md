# Beamable SUI Sample

Welcome to the Beamable SUI Sample project that demonstrates how
to integrate the [SUI](https://sui.io/) services into a [Beamable](https://beamable.com/) powered game. We'll
use two Beamable federation features:

- [Federated Authentication](https://beamable.github.io/UnrealSDK/user-reference/federation/federated-login/) - use SUI non-custodial wallet or automatically
  create one for each player
- [Federated Inventory](https://beamable.github.io/UnrealSDK/user-reference/federation/federated-inventory/) - mint NFTs for federated inventory items
  and FTs for federated inventory currency

## Requirements

Before getting started, please head to [Beamable](https://beamable.com/) and [SUI](https://blog.sui.io/sui-wallets/) to create
required accounts.
You should have the following tools installed on your development machine.

1. [Unreal 5.6](https://www.unrealengine.com/en-US/download)
2. [Docker](https://www.docker.com/products/docker-desktop/)
3. [Net8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
6. [NodeJS/NPM](https://nodejs.org/en/download)

## Repository structure
- /UnrealSample/Microservices/services - Beamable standalone microservice that implements federation features and communicates with the SUI blockchain
- /UnrealSample - Unreal project with Beamable SDK and basic UI to showcase the E2E flow.
- /Sui Wallet Integration - Slush wallet web sample project, generates JavaScript API for wallet interaction

## Getting Started
Navigate into the ```/UnrealSample/Microservices``` directory and run this commands to initialize the Beamable CLI and
connect the service to your Beamable organization and game:

```shell
dotnet tool restore
dotnet beam init --save-to-file
```

### Configuration
Configuration defaults are hard-coded inside the Configuration service [here](https://github.com/beamable/Sui-MonoRepo/blob/main/Unity/services/SuiFederation/Configuration/Configuration.cs).
You can override the values using the realm configuration.  
![Realm Configuration Example](../../../../Assets/realm-config.png)

**Default values:**  
| **Namespace** | **Key**          |**Default value** |**Description**                                         |
|---------------|------------------|------------------|--------------------------------------------------------|
| sui-federation| SuiEnvironment   | devnet           | SUI network name (mainnet, testnet, devnet, localnet)  |


## Running Locally
The SuiFederation project can be run locally by starting it from an IDE, or by executing the `dotnet run` command.
When a Microservice is built, a web-page will be opened automatically directed to the local swagger documentation for the service.

The project may also be run in Docker by executing the following command. However, it is recommended to
run the dotnet process locally for workflow speed.
```shell
dotnet beam services run --ids SuiFederation
```

#### BeamableTool
By default, both SuiFederation and SuiFederationCommon have the BeamCLI installed as a project tool.
However, it is likely that `beam` is installed as a global tool, and as such, the `BeamableTool` value
could be changed to `beam`.

#### OpenLocalSwaggerOnRun
If the `OpenLocalSwaggerOnRun` setting is set to `false`, then the local swagger will not open when the project is
built. However, it can be opened manually by the following shell command. Optionally, pass the `--remote` flag to open the
remote swagger.
```shell
dotnet beam project open-swagger SuiFederation
```

### Connecting to a Unity Project
The project can be linked to a number of Unreal Projects. The `./beamable/.linkedProjects.json` file
contains the relative paths to linked Unreal Projects. You can add an association by editing that file,
or by executing
```shell
dotnet beam project add-unreal-project .
```

## Deploying
The project can be deployed to the remote realm by running the follow commands.
```shell
dotnet beam deploy plan
dotnet beam deploy release --latest-plan
```

# Features
The example project implements following SUI features:
- creating a custodial SUI wallet for the player account
- attaching a Slush wallet to the player account
- automatic smart contract deployment
- support for following contract types:
    - Non-fungible token/NFT
    - Fungible token/Regular currency token
    - Fungible token/Closed loop token
    - Composite NFTs
    - Kiosk marketplace
    - Enoki integration
- transfer currency from a custodial to external wallet

## Custodial SUI wallet
Using Beamable's [federated authentication feature](https://beamable.github.io/UnrealSDK/user-reference/federation/federated-login/) the microservice can create a custodial SUI wallet for the player.
The implementation can be seen [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Endpoints/AuthenticateEndpoint.cs)

## Attaching a Slush wallet
Using Beamable's [federated authentication feature](https://beamable.github.io/UnrealSDK/user-reference/federation/federated-login/) the microservice can attach an external wallet the the player's account. Initiating the attach flow is specific for the platform (Unity or Unreal) but the backend implementation is the same. It works in a 2FA fashion, first by generating a message to sign, and then verifying the signature.
The implementation can be seen [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Endpoints/AuthenticateExternalEndpoint.cs)

## Attaching an OAuth based wallet using Enoki
Using Beamable's [federated authentication feature](https://beamable.github.io/UnrealSDK/user-reference/federation/federated-login/) the microservice can attach an OAuth based wallet the the player's account.
The implementation can be seen [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Endpoints/AuthenticateEnokiEndpoint.cs)

## Non-fungible token/NFT
NFT support in the microservice implements the following functionalities:
- dynamic smart contract creation
- NFT minting and burning
- Dynamic NFTs (updating NFT attributes)

Each NFT item can be minted, burned or updated trough inventory service update operations in a game authoritative fashion trough sponsored transactions. Beamable's federated inventory process is explained [here.](https://beamable.github.io/UnrealSDK/user-reference/federation/federated-inventory/)  
The example project contains an example of NFT game token item called "weapon", which is defined in [`here.`](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederationCommon/FederationContent/WeaponItem.cs)
Other custom NFT definition can be added by defining a new type which implements [`INftBase`](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederationCommon/Models/NftBase.cs) interface. Custom NFT type can be used within a Beamable content system to crete a set of possible NFTs that can be minted.    
Microservice will deploy smart contracts for each Beamable content item created based on the `INftBase` interface implementation.  
Content item definition supports following properties:
- name
- image
- description
- attributes: key-value pairs

Smart contract readme can be found [here.](Features/Contract/Templates/nft.MD)

## Fungible token/Regular currency token
Regular currency token support in the microservice implements the following functionalities:
- dynamic smart contract creation
- defining coin initial supply
- minting and burning

Each FT supports minting and burning coins trough sponsored transactions.
The example project contains an example of regular coin item called "coin", which is defined [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederationCommon/FederationContent/CoinCurrency.cs)  
Beamable content system can be used to add new regular coins for which contract will be automatically deployed.    
Content item definition supports following properties:
- name
- symbol
- decimals
- image
- description
- initial supply

Smart contract readme can be found [here.](Features/Contract/Templates/ft-rc.MD)

## Fungible token/Closed loop token
Closed loop token support in the microservice implements the following functionalities:
- dynamic smart contract creation
- defining coin initial supply and allowed actions
- minting and burning

Each FT supports minting and burning coins trough sponsored transactions.
The example project contains an example of regular coin item called "coin", which is defined [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederationCommon/FederationContent/InGameCurrency.cs)  
Beamable content system can be used to add new regular coins for which contract will be automatically deployed.    
Content item definition supports following properties:
- name
- symbol
- decimals
- image
- description
- initial supply
- optional token actions: spending, buying, transfers

Smart contract readme can be found [here.](Features/Contract/Templates/ft-cl.MD)

## Transfer currency
After a player’s custodial wallet is created and funded with fungible tokens, they can link an external (Slush) wallet and transfer tokens to it. The microservice exposes an [endpoint](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/SuiFederation.cs#L146) to initiate such a request. SUI SDK [function](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Features/SuiApi/ts/bridge.ts#L1094) handles coin fragmentation and execute appropriate transfers using sponsored transactions.

## Composable non-fungible token/NFT
Composable NFT support in the microservice implements the following functionalities:
- dynamic smart contract creation
- adding nested NFT items under parent NFT
- removing nested NFT items from parent NFT
- dynamic NFTs (updating NFT attributes)

Composable NFT item can be added, removed or updated trough inventory service update operations in a game authoritative fashion trough sponsored transactions. Beamable's federated inventory process is explained [here.](https://beamable.github.io/UnrealSDK/user-reference/federation/federated-inventory/)  
The example project contains an example of composable NFT game token item called "laser mod", which is defined in [`here.`](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederationCommon/FederationContent/LaserMod.cs)
Other custom NFT definition can be added by defining a new type which implements [`INftAddon`](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederationCommon/Models/INftAddon.cs) interface and sub-classing the parent NFT item type. Custom NFT type can be used within a Beamable content system to crete a set of possible NFTs that can be minted.  
Microservice will deploy smart contracts for each Beamable content item created based on the `INftAddon` interface implementation.  
Content item definition supports following properties:
- name
- image
- description
- attributes: key-value pairs

Smart contract readme can be found [here.](Features/Contract/Templates/nft_addon.MD)

## Kiosk marketplace
Kiosk marketplace support in the microservice implements the following functionalities:
- dynamic smart contract creation
- listing NFTs for sale
- de-listing NFTs from sale
- purchasing listed NFTs for a custom currency (regular or closed loop)

The example project contains an example of an kiosk marketplace which is defined in [`here.`](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederationCommon/FederationContent/KioskItem.cs)
Each kiosk marketplace is associated with a specific NFT type and a currency type (regular or closed loop). The kiosk contract supports listing, de-listing and purchasing NFTs using sponsored transactions.
Microservice will deploy smart contracts for each Beamable content item created that is defined under `kiosk` type.  
Content item definition supports following properties:
- kiosk name
- item type
- currency symbol

Smart contract readme can be found [here.](Features/Contract/Templates/kiosk.MD)

## Enoki integration
Enoki integration support in the microservice implements the following functionalities:
- Google and Twitch OAuth based wallets
- sponsored transactions
  To enable Enoki integration there are several steps as prerequisites:
- Enoki account needs to be created [here](https://portal.enoki.mystenlabs.com/)
- Enoki app needs to be created with OAuth providers (Google, Twitch) enabled
- Enoki API key needs to be added to the realm configuration under `EnokiApiKey` name
- OAuth provider client secrets need to be added to the realm configuration under `GoogleClientSecret` and/or `TwitchClientSecret` names  
  Using Beamable's [federated authentication feature](https://beamable.github.io/UnrealSDK/user-reference/federation/federated-login/) the microservice can attach an OAuth based wallet the the player's account.
  The implementation can be seen [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Endpoints/AuthenticateEnokiEndpoint.cs)  
  Beamable Content system is used to control which integration will be used for the specific NFT or FT asset. For example, choosing SuiEnokiIdentity for the "coin" currency asset will enable Enoki integration for that asset.  
  ![Content Configuration Example](../../../../Assets/content-config.png)    