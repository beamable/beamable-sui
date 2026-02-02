# Composable non-fungible token/NFT smart contract

Smart contract for the composable NTFs lives [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Features/Contract/Templates/nft_addon.move)  
The file contains regular Move code but it is used as a template, general best practice example on how an composable NFT contract should look like with the most common functions.   
The microservice uses [Handlebars](https://www.nuget.org/packages/Handlebars.Net/) package to generate the final contract code by filling in the package and struct names from the Content system. Additional data from the content system that is used to construct the final contract are:
- name
- description
- url
- attributes 

## Creating an composable NFT smart contract
Creating an composable NFT contract is initiated by Beamable's [content system](https://docs.beamable.com/docs/content-guide). Adding a new content item will automatically create a new contract from the template, compile and deploy. The sample project already contains one example of content type meant to act as an NFT [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederationCommon/FederationContent/LaserMod.cs)

## Smart contract deployment  
The smart contract deployment happens at the time of: 
- microservice deployment, or
- content publish operation

## Smart contract deployment details
The microservice deployment holds a [SUI CLI tool executable](https://github.com/beamable/Sui-MonoRepo/tree/main/UnrealSample/Microservices/services/SuiFederation/SuiTools) that is used to compile and deploy the smart contract.
The full list of steps are:
- the wallet used for deploying the contracts is created on the microservice [startup](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/SuiFederation.cs#L43)
- on the devnet environment SUI coins are requested [automatically](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Features/Contract/SuiClientWrapper/SuiClient.cs#L54). On testnet environment, SUI coins should be requested manually [here](https://faucet.sui.io/)
- microservice [compiles and deploys](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Features/Contract/SuiClientWrapper/SuiClient.cs#L76) all of the defined content items

In the case of SUI CLI tool update, a new version can be installed using the provided [script.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/install_sui_tools.ps1)

## Smart contract interaction
Bridging C# and SUI SDK TypeScript is done using the [Jering package](https://www.nuget.org/packages/Jering.Javascript.NodeJS). All smart contract and chain operations are done using [this service](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Features/SuiApi/SuiApiService.cs) which is used to call specific [SUI SDK functions](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Features/SuiApi/ts/bridge.ts).

## Smart contract functions
### Add
The entry point for the add function is [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Features/Contract/Templates/nft_addon.move#L119) It requires:
- parent NFT object
- common properties for the new NFT addon (name, url, description, attributes)
- content ID of the new NFT addon as the unique identifier of the parent dynamic field
- SUI's dynamic field feature is used to store the new NFT addon under the parent NFT

### Remove
The entry point for the remove function is [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Features/Contract/Templates/nft_addon.move#L139) It requires:
- parent NFT object
- content ID of the new NFT addon as the unique identifier of the parent dynamic field

### Update nested NFT attribute
The entry point for the update function is [here.](https://github.com/beamable/Sui-MonoRepo/blob/main/UnrealSample/Microservices/services/SuiFederation/Features/Contract/Templates/nft_addon.move#L157) It requires:
- NFT object
- the attribute name and value to change