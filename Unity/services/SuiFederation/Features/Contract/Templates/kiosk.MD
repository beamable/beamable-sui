# Kiosk smart contract

Smart contract for the kiosk lives [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/Unity/services/SuiFederation/Features/Contract/Templates/kiosk.move)  
The file contains regular Move code but it is used as a template, general best practice example on how an kiosk contract should look like with the most common functions.   
The microservice uses [Handlebars](https://www.nuget.org/packages/Handlebars.Net/) package to generate the final contract code by filling in the package and struct names from the Content system. Additional data from the content system that is used to construct the final contract are:
- name
- item type
- currency symbol

## Creating an kiosk smart contract
Creating an kiosk contract is initiated by Beamable's [content system](https://docs.beamable.com/docs/content-guide). Adding a new content item will automatically create a new contract from the template, compile and deploy. The sample project already contains one example of content type meant to act as an NFT [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/Unity/services/SuiFederationCommon/FederationContent/KioskItem.cs)

## Smart contract deployment  
The smart contract deployment happens at the time of: 
- microservice deployment, or
- content publish operation

## Smart contract deployment details  
The microservice deployment holds a [SUI CLI tool executable](https://github.com/beamable/Sui-MonoRepo/tree/main/Unity/services/SuiFederation/SuiTools) that is used to compile and deploy the smart contract. 
The full list of steps are:  
- the wallet used for deploying the contracts is created on the microservice [startup](https://github.com/beamable/Sui-MonoRepo/blob/main/Unity/services/SuiFederation/SuiFederation.cs#L43)
- on the devnet environment SUI coins are requested [automatically](https://github.com/beamable/Sui-MonoRepo/blob/main/Unity/services/SuiFederation/Features/Contract/SuiClientWrapper/SuiClient.cs#L56). On testnet environment, SUI coins should be requested manually [here](https://faucet.sui.io/)
- microservice [compiles and deploys](https://github.com/beamable/Sui-MonoRepo/blob/main/Unity/services/SuiFederation/Features/Contract/SuiClientWrapper/SuiClient.cs#L76) all of the defined content items

In the case of SUI CLI tool update, a new version can be installed using the provided [script.](https://github.com/beamable/Sui-MonoRepo/blob/main/Unity/services/SuiFederation/install_sui_tools.ps1)  

## Smart contract interaction
Bridging C# and SUI SDK TypeScript is done using the [Jering package](https://www.nuget.org/packages/Jering.Javascript.NodeJS). All smart contract and chain operations are done using [this service](https://github.com/beamable/Sui-MonoRepo/blob/main/Unity/services/SuiFederation/Features/SuiApi/SuiApiService.cs) which is used to call specific [SUI SDK functions](https://github.com/beamable/Sui-MonoRepo/blob/main/Unity/services/SuiFederation/Features/SuiApi/ts/bridge.ts).

## Smart contract functions
### List
The entry point for the list function is [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/Unity/services/SuiFederation/Features/Contract/Templates/kiosk.move#L58) It requires:
- marketplace object
- NFT object to list
- price

### Delist
The entry point for the delist function is [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/Unity/services/SuiFederation/Features/Contract/Templates/kiosk.move#L81) It requires:
- marketplace object
- listing object

### Purchase
The entry point for the purchase function is [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/Unity/services/SuiFederation/Features/Contract/Templates/kiosk.move#L102) It requires:
- marketplace object
- listing object
- payment object
There are 2 types of payment supported:
- using custom regular coins
- using custom closed loop token