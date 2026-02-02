import {
    getFullnodeUrl,
    GetOwnedObjectsParams,
    MoveValue,
    PaginatedObjectsResponse,
    SuiClient,
    SuiObjectChange,
    SuiObjectRef,
    SuiTransactionBlockResponse
} from '@mysten/sui/client';
import {verifyPersonalMessageSignature} from '@mysten/sui/verify';
import {fromBase64, fromHex, SUI_CLOCK_OBJECT_ID, toBase64} from '@mysten/sui/utils';
import {Ed25519Keypair} from '@mysten/sui/keypairs/ed25519';
import {bech32} from 'bech32';
import {Transaction, TransactionArgument} from '@mysten/sui/transactions';
import {
    CustomRulePackageIds,
    CoinBalanceRequest,
    CoinBalanceResponse,
    CoinModel,
    CoinToken,
    CreateWalletResponse,
    CurrencyTransfer,
    GameCoinBurnMessage,
    GameCoinMintMessage,
    KioskDelistRequest,
    KioskListRequest,
    KioskPurchaseRequest,
    NftDeleteMessage,
    NftMintMessage,
    NftUpdateMessage, PERSONAL_KIOSK_RULE_ADDRESS,
    PersonalKioskCancelExclusiveRequest,
    PersonalKioskCreateRequest,
    PersonalKioskDeclinePurchaseRequest,
    PersonalKioskDelistRequest,
    PersonalKioskListRequest,
    PersonalKioskPurchaseRequest,
    PersonalKioskTakeRequest,
    PersonalKioskWithdrawRequest,
    RegularCoinBalanceRequest,
    RegularCoinBurnMessage,
    RegularCoinMintMessage,
    SetNftOwnerMessage,
    SuiEnokiTransaction,
    SuiObject,
    SuiTransactionResult,
    ZkLoginProofData, ROYALTY_RULE_ADDRESS
} from './models';
import {calculateTotalCost, retrievePaginatedData, stringToNumber} from "./utils";
import {bcs} from "@mysten/sui/bcs";
import {getZkLoginSignature} from '@mysten/sui/zklogin';
import {KioskClient, KioskTransaction, Network} from "@mysten/kiosk";

type Callback<T> = (error: any, result: T | null) => void;
type Environment = 'mainnet' | 'testnet' | 'devnet' | 'localnet';
const EnvironmentNetwork: Record<Environment, Network> = {
    mainnet: Network.MAINNET,
    testnet: Network.TESTNET,
    devnet: Network.CUSTOM,
    localnet: Network.CUSTOM,
};
const SUI_PRIVATE_KEY_PREFIX = 'suiprivkey';
let suiClientInstance: SuiClient | null = null;
let kioskClientInstance: KioskClient | null = null;

function getSuiClientInstance(environment: Environment): SuiClient {
    if (!suiClientInstance) {
        suiClientInstance = new SuiClient({url: getFullnodeUrl(environment)});
    }
    return suiClientInstance;
}

function getKioskClientInstance(environment: Environment, customPackageIds?: CustomRulePackageIds): KioskClient {
    if (!kioskClientInstance) {
        if (!suiClientInstance) {
            suiClientInstance = new SuiClient({url: getFullnodeUrl(environment)});
        }
        kioskClientInstance = new KioskClient({
            client: suiClientInstance,
            network: EnvironmentNetwork[environment],
            packageIds: customPackageIds
        });
    }
    return kioskClientInstance;
}

function decodeSuiPrivateKey(value: string) {
    if (value.startsWith("0x")) {
        return fromHex(value);
    }
    if (value.startsWith(SUI_PRIVATE_KEY_PREFIX)) {
        const { prefix, words } = bech32.decode(value);
        const extendedSecretKey = new Uint8Array(bech32.fromWords(words));
        return extendedSecretKey.slice(1);
    }
    if (!value.startsWith(SUI_PRIVATE_KEY_PREFIX) && value.length == 60) {
        const pk_with_prefix = SUI_PRIVATE_KEY_PREFIX + value;
        const { prefix, words } = bech32.decode(pk_with_prefix);
        const extendedSecretKey = new Uint8Array(bech32.fromWords(words));
        return extendedSecretKey.slice(1);
    }
    if (!value.startsWith(SUI_PRIVATE_KEY_PREFIX) && value.length == 44) {
        return fromBase64(value);
    }
    throw new Error('invalid private key value');
}

async function sponsorTransaction(tx: Transaction, playerKey: string, developerKey: string, environment: Environment): Promise<SuiTransactionBlockResponse> {
    const suiClient = getSuiClientInstance(environment);
    const developerKeypair = Ed25519Keypair.fromSecretKey(decodeSuiPrivateKey(developerKey));
    const playerKeypair = Ed25519Keypair.fromSecretKey(decodeSuiPrivateKey(playerKey));
    let payment: SuiObjectRef[] = [];
    const coins = await suiClient.getCoins({ owner: developerKeypair.toSuiAddress(), limit: 1 });
    if (coins.data.length > 0) {
        payment = coins.data.map((coin) => ({
            objectId: coin.coinObjectId,
            version: coin.version,
            digest: coin.digest,
        }));
    } else {
        throw new Error(`Can't find gas coins from sponsor.`);
    }
    const kindBytes = await tx.build({ onlyTransactionKind: true, client: suiClient });
    const sponsoredTxb = Transaction.fromKind(kindBytes);
    sponsoredTxb.setSender(playerKeypair.toSuiAddress());
    sponsoredTxb.setGasOwner(developerKeypair.toSuiAddress());
    sponsoredTxb.setGasPayment(payment);
    const sponsoredBytes = await sponsoredTxb.build({ client: suiClient });
    const developerSignature = await developerKeypair!.signTransaction(sponsoredBytes);
    const playerSignature = await playerKeypair!.signTransaction(sponsoredBytes);

    return await suiClient.executeTransactionBlock({
        transactionBlock: sponsoredBytes,
        signature: [developerSignature.signature, playerSignature.signature],
        options: {
            showEffects: true,
            showEvents: true,
            showObjectChanges: true,
        },
    });
}

function setTransactionResponse(response: SuiTransactionBlockResponse, result: SuiTransactionResult) {
    if (response.effects != null) {
        result.status = response.effects.status.status;
        result.gasUsed = calculateTotalCost(response.effects.gasUsed);
        result.digest = response.effects.transactionDigest;
        result.objectIds = response.effects.created?.map(o => o.reference.objectId);
        result.error = response.effects.status.error;
    }
}

async function getLatestEpoch(callback: Callback<string>, environment: Environment) {
    let error = null;
    let result = "";
    try {
        const suiClient = getSuiClientInstance(environment);
        const epochInfo = await suiClient.getLatestSuiSystemState();
        result = epochInfo.epoch;
    } catch (ex) {
        error = ex;
    }
    callback(error, result);
}

async function getGasInfo(callback: Callback<string>, digest: string, environment: Environment) {
    let error = null;
    let result = "";
    try {
        const suiClient = getSuiClientInstance(environment);
        const tx = await suiClient.getTransactionBlock({
            digest: digest,
            options: {
                showEffects: true,
            },
        });
        if (tx.effects) {
            result = calculateTotalCost(tx.effects.gasUsed);
        }
    } catch (ex) {
        error = ex;
    }
    callback(error, result);
}
async function createWallet(callback: Callback<string>) {
    let error = null;
    const keys= new CreateWalletResponse();
    try {
        const keypair = new Ed25519Keypair();
        keys.PrivateKey = keypair.getSecretKey();
        keys.Address = keypair.toSuiAddress();
        keys.PublicKey = Buffer.from(keypair.getPublicKey().toRawBytes()).toString('base64');
    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(keys));
}

async function createEphemeral(callback: Callback<string>) {
    let error = null;
    const keys= new CreateWalletResponse();
    try {
        const keypair = new Ed25519Keypair();
        keys.PrivateKey = keypair.getSecretKey();
        keys.Address = keypair.toSuiAddress();
        keys.PublicKey = keypair.getPublicKey().toSuiPublicKey();
    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(keys));
}

async function importWallet(callback: Callback<string>, privateKey: string) {
    let error = null;
    const keys= new CreateWalletResponse();
    try {
        const decoded = decodeSuiPrivateKey(privateKey);
        const keypair = Ed25519Keypair.fromSecretKey(decoded);
        keys.PrivateKey = keypair.getSecretKey();
        keys.Address = keypair.toSuiAddress();
        keys.PublicKey = Buffer.from(keypair.getPublicKey().toRawBytes()).toString('base64');
    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(keys));
}
async function verifySignature(callback: Callback<boolean>, token: string, challenge: string, solution: string) {
    let isValid = false;
    let error = null;
    try {
        const messageEncoded = new TextEncoder().encode(challenge);
        const verificationPublicKey = await verifyPersonalMessageSignature(messageEncoded, solution);
        if (verificationPublicKey.toSuiAddress() === token) {
            isValid = true;
        }
    } catch (ex) {
        if (ex instanceof Error && ex.message === `Signature is not valid for the provided message`) {
            isValid = false;
        } else if (ex instanceof Error && ex.message.startsWith('Unsupported signature scheme')) {
            isValid = false;
        } else {
            isValid = false;
            error = ex;
        }
    }
    callback(error, isValid);
}
async function mintRegularCoin(callback: Callback<string>, item: string, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const mintRequests: RegularCoinMintMessage[] = JSON.parse(item);
        const keypair = Ed25519Keypair.fromSecretKey(realmKey);
        const txb = new Transaction();

        mintRequests.forEach((coinItem) => {
            const coinTarget: `${string}::${string}::${string}` = `${coinItem.PackageId}::${coinItem.Module}::${coinItem.Function}`;
            txb.moveCall({
                target: coinTarget,
                arguments: [
                    txb.object(coinItem.AdminCap),
                    txb.object(coinItem.TreasuryCap),
                    txb.pure.u64(coinItem.Amount),
                    txb.pure.address(coinItem.PlayerWalletAddress)
                ]});
        });

        const suiClient = getSuiClientInstance(environment);
        const response = await suiClient.signAndExecuteTransaction({
            signer: keypair,
            transaction: txb,
            options: {
                showEffects: true
            }
        });
        setTransactionResponse(response, result);
    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}
async function getBalance(callback: Callback<string>, address: string, item: string, environment: Environment) {
    let error = null;
    let coinBalanceResponse = new CoinBalanceResponse("",0);
    try {
        const suiClient = getSuiClientInstance(environment);
        const request: CoinBalanceRequest = JSON.parse(item);
        const coinType = `${request.PackageId}::${request.ModuleName.toLowerCase()}::${request.ModuleName.toUpperCase()}`;
        const coinBalance = await suiClient.getBalance({
            owner: address,
            coinType: coinType
        });
        coinBalanceResponse.CoinType = coinType;
        coinBalanceResponse.Total = parseInt(coinBalance.totalBalance ?? "0", 10) || 0;

    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(coinBalanceResponse));
}
async function mintNfts(callback: Callback<string>, item: string, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const mintRequests: NftMintMessage[] = JSON.parse(item);
        const keypair = Ed25519Keypair.fromSecretKey(realmKey);
        const txb = new Transaction();

        mintRequests.forEach((item) => {
            const target: `${string}::${string}::${string}` = `${item.PackageId}::${item.Module}::${item.Function}`;
            txb.moveCall({
                target: target,
                arguments: [
                    txb.object(item.AdminCap),
                    txb.pure.address(item.PlayerWalletAddress),
                    txb.pure.string(item.NftContentItem.Name),
                    txb.pure.string(item.NftContentItem.Description),
                    txb.pure.string(item.NftContentItem.Url),
                    txb.pure.string(item.NftContentItem.ContentId),
                    txb.pure.vector("string", item.NftContentItem.Attributes.map(attribute => attribute.Name)),
                    txb.pure.vector("string", item.NftContentItem.Attributes.map(attribute => attribute.Value))
                ]});
        });

        const suiClient = getSuiClientInstance(environment);
        const response = await suiClient.signAndExecuteTransaction({
            signer: keypair,
            transaction: txb,
            options: {
                showEffects: true
            }
        });
        setTransactionResponse(response, result);
    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}
async function setNftContractOwner(callback: Callback<string>, item: string, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const request: SetNftOwnerMessage = JSON.parse(item);
        const keypair = Ed25519Keypair.fromSecretKey(realmKey);
        const txb = new Transaction();

        const target: `${string}::${string}::${string}` = `${request.PackageId}::${request.Module}::${request.Function}`;
        txb.moveCall({
            target: target,
            arguments: [
                txb.object(request.AdminCap),
                txb.pure.vector("u8", keypair.getPublicKey().toRawBytes())
            ]});

        const suiClient = getSuiClientInstance(environment);
        const response = await suiClient.signAndExecuteTransaction({
            signer: keypair,
            transaction: txb,
            options: {
                showEffects: true
            }
        });
        setTransactionResponse(response, result);
    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}
async function getOwnedObjects(callback: Callback<string>, address: string, packageId: string, environment: Environment) {
    let error = null;
    const objects: SuiObject[] = [];
    try {
        const suiClient = getSuiClientInstance(environment);

        const inputParams: GetOwnedObjectsParams = {
            owner: address,
            cursor: null,
            options: {
                showType: true,
                showContent: true,
                showDisplay: true
            },
            filter: {
                Package: packageId
            }
        };
        const handleData = async (input: GetOwnedObjectsParams) => {
            return await suiClient.getOwnedObjects(input);
        };

        const results = await retrievePaginatedData<GetOwnedObjectsParams, PaginatedObjectsResponse>(handleData, inputParams);

        results.forEach(result => {
            result.data.forEach(element => {
                if (element.data != undefined)  {
                    const suiObject = new SuiObject(
                        element.data.objectId,
                        element.data.digest,
                        element.data.version,
                        element.data.content,
                        element.data.display
                    );
                    objects.push(suiObject);
                }
            });
        })

    } catch (ex) {
        error = ex;
    }

    callback(error, JSON.stringify(objects.map(object => object.toView())));
}


async function getAttachments(callback: Callback<string>, objectId: string, environment: Environment) {
    let error = null;
    let cursor: string | null = null;
    const objects: SuiObject[] = [];
    try {
        const suiClient = getSuiClientInstance(environment);
        let allDynamicFields = [];

        while (true) {
            const result = await suiClient.getDynamicFields({
                parentId: objectId,
                cursor: cursor
            });

            allDynamicFields.push(...result.data);

            if (!result.hasNextPage) {
                break;
            }
            cursor = result.nextCursor;
        }

        for (const field of allDynamicFields) {
            const dynamicFieldObject = await suiClient.getDynamicFieldObject({
                parentId: objectId,
                name: {
                    type: field.name.type,
                    value: field.name.value
                }
            });
            if (dynamicFieldObject.data != undefined)  {
                const suiObject = new SuiObject(
                    dynamicFieldObject.data.objectId,
                    dynamicFieldObject.data.digest,
                    dynamicFieldObject.data.version,
                    dynamicFieldObject.data.content,
                    dynamicFieldObject.data.display
                );
                objects.push(suiObject);
            }
        }

    } catch (ex) {
        error = ex;
    }

    callback(error, JSON.stringify(objects.map(object => object.toViewAttachment())));
}

async function getGameCoinBalance(callback: Callback<string>, address: string, item: string, environment: Environment) {
    let error = null;
    let coinBalanceResponse = new CoinBalanceResponse("",0);
    const objects: SuiObject[] = [];
    try {
        const suiClient = new SuiClient({url: getFullnodeUrl(environment)});
        const request: RegularCoinBalanceRequest = JSON.parse(item);
        const coinType = `0x2::token::Token<${request.PackageId}::${request.ModuleName.toLowerCase()}::${request.ModuleName.toUpperCase()}>`;
        coinBalanceResponse.CoinType = coinType;
        const inputParams: GetOwnedObjectsParams = {
            owner: address,
            cursor: null,
            options: {
                showType: true,
                showContent: true,
                showDisplay: false
            },
            filter: {
                StructType: coinType
            }
        };
        const handleData = async (input: GetOwnedObjectsParams) => {
            return await suiClient.getOwnedObjects(input);
        };

        const results = await retrievePaginatedData<GetOwnedObjectsParams, PaginatedObjectsResponse>(handleData, inputParams);

        results.forEach(result => {
            result.data.forEach(element => {
                if (element.data != undefined)  {
                    const suiObject = new SuiObject(
                        element.data.objectId,
                        element.data.digest,
                        element.data.version,
                        element.data.content,
                        null // NO display
                    );
                    objects.push(suiObject);
                }
            });
        })

        let totalAmount = 0;
        objects.forEach(obj => {
            switch (obj.Content?.dataType) {
                case 'moveObject':
                    const contentFields = obj.Content?.fields;
                    if (contentFields != null) {
                        const balanceStr = (contentFields as { [key: string]: MoveValue })["balance"] as string;
                        const balance = stringToNumber(balanceStr);
                        totalAmount += balance;
                    }
            }
        });
        coinBalanceResponse.Total = totalAmount;

    } catch (ex) {
        error = ex;
    }

    callback(error, JSON.stringify(coinBalanceResponse));
}
async function burnRegularCoin(callback: Callback<string>, request: string, realmKey: string, isEnoki: boolean, environment: Environment) {
    let error = null;
    const burnRequests: RegularCoinBurnMessage[] = JSON.parse(request);
    let result = null;
    try {
        const suiClient = getSuiClientInstance(environment);
        const txb = new Transaction();

        for (const coinItem of burnRequests) {
            const coins = await suiClient.getCoins({
                owner: coinItem.PlayerWalletAddress,
                coinType: `${coinItem.PackageId}::${coinItem.Module.toLowerCase()}::${coinItem.Module.toUpperCase()}`
            });

            // Sort coins by balance (descending)
            const sortedCoins = coins.data.sort((a, b) => Number(b.balance) - Number(a.balance));

            // Select coins whose total balance matches the amount to burn
            let totalBalance = 0;
            const selectedCoins = [];
            for (const coin of sortedCoins) {
                if (totalBalance >= coinItem.Amount) break;
                selectedCoins.push(coin);
                totalBalance += Number(coin.balance);
            }

            let remainingAmount = coinItem.Amount;
            const coinsToBurn = [];
            for (const coin of selectedCoins) {
                const coinBalance = Number(coin.balance);
                if (coinBalance <= remainingAmount) {
                    // Use the entire coin
                    coinsToBurn.push(coin.coinObjectId);
                    remainingAmount -= coinBalance;
                } else {
                    // Split the coin to get the exact amount needed
                    const splitCoin = txb.splitCoins(txb.object(coin.coinObjectId), [txb.pure.u64(remainingAmount)]);
                    coinsToBurn.push(splitCoin);
                    remainingAmount = 0;
                }
                if (remainingAmount === 0) break;
            }
            const coinTarget: `${string}::${string}::${string}` = `${coinItem.PackageId}::${coinItem.Module}::${coinItem.Function}`;
            for (const coinId of coinsToBurn) {
                txb.moveCall({
                    target: coinTarget,
                    arguments: [
                        txb.object(coinItem.TreasuryCap),
                        txb.object(coinId),
                    ],
                });
            }
        }

        if (isEnoki) {
            result = new SuiEnokiTransaction();
            const kindBytes = await txb.build({ onlyTransactionKind: true, client: suiClient });
            result.Id = toBase64(kindBytes);
        }
        else {
            result = new SuiTransactionResult();
            const response = await sponsorTransaction(txb, burnRequests[0].PlayerWalletKey, realmKey, environment);
            setTransactionResponse(response, result);
        }

    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function enokiSign(callback: Callback<string>, zkLoginProof: string, maxEpoch: number, transactionBytes: string, ephemeralKey: string, environment: Environment) {
    let error = null;
    let result = "";
    try {
        const keypair = Ed25519Keypair.fromSecretKey(ephemeralKey);
        const signatureWithBytes = await keypair.signTransaction(fromBase64(transactionBytes));

        const zkLoginSignature: ZkLoginProofData = JSON.parse(zkLoginProof);

        result = getZkLoginSignature({
            inputs: zkLoginSignature,
            maxEpoch: maxEpoch,
            userSignature: signatureWithBytes.signature,
        });
    } catch (ex) {
        error = ex;
    }
    callback(error, result);
}

async function mintGameCoin(callback: Callback<string>, item: string, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const mintRequests: GameCoinMintMessage[] = JSON.parse(item);
        const keypair = Ed25519Keypair.fromSecretKey(realmKey);
        const txb = new Transaction();

        mintRequests.forEach((coinItem) => {
            const coinTarget: `${string}::${string}::${string}` = `${coinItem.PackageId}::${coinItem.Module}::${coinItem.Function}`;
            txb.moveCall({
                target: coinTarget,
                arguments: [
                    txb.object(coinItem.AdminCap),
                    txb.object(coinItem.Store),
                    txb.pure.u64(coinItem.Amount),
                    txb.pure.address(coinItem.PlayerWalletAddress)
                ]});
        });

        const suiClient = getSuiClientInstance(environment);
        const response = await suiClient.signAndExecuteTransaction({
            signer: keypair,
            transaction: txb,
            options: {
                showEffects: true
            }
        });
        setTransactionResponse(response, result);

    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}
async function burnGameCoin(callback: Callback<string>, request: string, realmKey: string, isEnoki: boolean, environment: Environment) {
    let error = null;
    const burnRequests: GameCoinBurnMessage[] = JSON.parse(request);
    let result = null;
    const coinTokens: CoinToken[] = [];

    try {
        const suiClient = getSuiClientInstance(environment);
        const txb = new Transaction();

        for (const tokenItem of burnRequests) {
            const objects: SuiObject[] = [];

            const coinType = `0x2::token::Token<${tokenItem.PackageId}::${tokenItem.Module.toLowerCase()}::${tokenItem.Module.toUpperCase()}>`;
            const inputParams: GetOwnedObjectsParams = {
                owner: tokenItem.PlayerWalletAddress,
                cursor: null,
                options: {
                    showType: true,
                    showContent: true,
                    showDisplay: false
                },
                filter: {
                    StructType: coinType
                }
            };
            const handleData = async (input: GetOwnedObjectsParams) => {
                return await suiClient.getOwnedObjects(input);
            };

            const results = await retrievePaginatedData<GetOwnedObjectsParams, PaginatedObjectsResponse>(handleData, inputParams);

            results.forEach(result => {
                result.data.forEach(element => {
                    if (element.data != undefined)  {
                        const suiObject = new SuiObject(
                            element.data.objectId,
                            element.data.digest,
                            element.data.version,
                            element.data.content,
                            null // NO display
                        );
                        objects.push(suiObject);
                    }
                });
            });
            objects.forEach(obj => {
                switch (obj.Content?.dataType) {
                    case 'moveObject':
                        const contentFields = obj.Content?.fields;
                        if (contentFields != null) {
                            const balanceStr = (contentFields as { [key: string]: MoveValue })["balance"] as string;
                            const balance = stringToNumber(balanceStr);
                            const idValue = (contentFields as { [key: string]: MoveValue })["id"] as { id: string };
                            const idStr = idValue.id;
                            coinTokens.push(new CoinToken(idStr, balance));
                        }
                }
            });

            const sortedCoins = coinTokens.sort((a, b) => a.Balance - b.Balance);

            // Select coins whose total balance matches the amount to burn
            let totalBalance = 0;
            const selectedCoins = [];
            for (const coin of sortedCoins) {
                if (totalBalance >= tokenItem.Amount) break;
                selectedCoins.push(coin);
                totalBalance += Number(coin.Balance);
            }

            let remainingAmount = tokenItem.Amount;
            for (const coin of selectedCoins) {
                const coinBalance = Number(coin.Balance);
                if (coinBalance <= remainingAmount) {
                    // Use the entire coin
                    const coinTarget: `${string}::${string}::${string}` = `${tokenItem.PackageId}::${tokenItem.Module}::${tokenItem.Function}`;
                    txb.moveCall({
                        target: coinTarget,
                        arguments: [
                            txb.object(coin.Id),
                            txb.object(tokenItem.Store),
                        ],
                    });
                    remainingAmount -= coinBalance;
                } else {
                    // Split the coin to get the exact amount needed
                    const coinTarget: `${string}::${string}::${string}` = `${tokenItem.PackageId}::${tokenItem.Module}::splitBurn`;
                    txb.moveCall({
                        target: coinTarget,
                        arguments: [
                            txb.object(coin.Id),
                            txb.object(tokenItem.Store),
                            txb.pure.u64(remainingAmount)
                        ],
                    });
                    remainingAmount = 0;
                }
                if (remainingAmount === 0) break;
            }
        }

        if (isEnoki) {
            result = new SuiEnokiTransaction();
            const kindBytes = await txb.build({ onlyTransactionKind: true, client: suiClient });
            result.Id = toBase64(kindBytes);
        }
        else {
            result = new SuiTransactionResult();
            const response = await sponsorTransaction(txb, burnRequests[0].PlayerWalletKey, realmKey, environment);
            setTransactionResponse(response, result);
        }

    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function updateNft(callback: Callback<string>, request: string, realmKey: string, isEnoki: boolean, environment: Environment) {
    let error = null;
    const updateRequests: NftUpdateMessage[] = JSON.parse(request);
    let result = null;
    try {
        const suiClient = getSuiClientInstance(environment);
        const gameKeypair = Ed25519Keypair.fromSecretKey(realmKey);
        const txb = new Transaction();
        const currentTime = Date.now();

        for (const request of updateRequests) {
            for (const attribute of request.Attributes) {
                const nftIdBytes = bcs.Address.serialize(request.ProxyId).toBytes();
                const nameBytes = bcs.string().serialize(attribute.Name).toBytes();
                const valueBytes = bcs.string().serialize(attribute.Value).toBytes();
                const timestampBytes = bcs.u64().serialize(currentTime).toBytes();

                // Combine all serialized bytes into a single Uint8Array
                const messageBytes = new Uint8Array([
                    ...nftIdBytes,
                    ...nameBytes,
                    ...valueBytes,
                    ...timestampBytes,
                ]);

                const signature = await gameKeypair.sign(messageBytes);
                const target: `${string}::${string}::${string}` = `${request.PackageId}::${request.Module}::${request.Function}`;
                txb.moveCall({
                    target: target,
                    arguments: [
                        txb.object(request.ProxyId),
                        txb.object(request.OwnerObjectId),
                        txb.object(SUI_CLOCK_OBJECT_ID),
                        txb.pure.vector("u8", signature),
                        txb.pure.u64(currentTime),
                        txb.pure.string(attribute.Name),
                        txb.pure.string(attribute.Value),
                    ],
                });
            }
        }

        if (isEnoki) {
            result = new SuiEnokiTransaction();
            const kindBytes = await txb.build({ onlyTransactionKind: true, client: suiClient });
            result.Id = toBase64(kindBytes);
        }
        else {
            result = new SuiTransactionResult();
            const response = await sponsorTransaction(txb, updateRequests[0].PlayerWalletKey, realmKey, environment);
            setTransactionResponse(response, result);
        }

    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function attachNft(callback: Callback<string>, request: string, realmKey: string, isEnoki: boolean, environment: Environment) {
    let error = null;
    const updateRequests: NftUpdateMessage[] = JSON.parse(request);
    let result = null;
    try {
        const suiClient = getSuiClientInstance(environment);
        const txb = new Transaction();

        updateRequests.forEach((item) => {
            if (item.Attachment) {
                const target: `${string}::${string}::${string}` = `${item.PackageId}::${item.Module}::${item.Function}`;
                txb.moveCall({
                    target: target,
                    arguments: [
                        txb.object(item.ProxyId),
                        txb.pure.string(item.Attachment.Name),
                        txb.pure.string(item.Attachment.Description),
                        txb.pure.string(item.Attachment.Url),
                        txb.pure.string(item.Attachment.ContentId),
                        txb.pure.vector("string", item.Attachment.Attributes.map(attribute => attribute.Name)),
                        txb.pure.vector("string", item.Attachment.Attributes.map(attribute => attribute.Value))
                    ]});
            }
        });

        if (isEnoki) {
            result = new SuiEnokiTransaction();
            const kindBytes = await txb.build({ onlyTransactionKind: true, client: suiClient });
            result.Id = toBase64(kindBytes);
        }
        else {
            result = new SuiTransactionResult();
            const response = await sponsorTransaction(txb, updateRequests[0].PlayerWalletKey, realmKey, environment);
            setTransactionResponse(response, result);
        }

    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function updateNftAttachment(callback: Callback<string>, request: string, realmKey: string, isEnoki: boolean, environment: Environment) {
    let error = null;
    const updateRequests: NftUpdateMessage[] = JSON.parse(request);
    let result = null;
    try {
        const suiClient = getSuiClientInstance(environment);
        const txb = new Transaction();

        updateRequests.forEach((request) => {
            if (request.Attachment) {
                for (const attribute of request.Attachment.Attributes) {
                    const target: `${string}::${string}::${string}` = `${request.PackageId}::${request.Module}::${request.Function}`;
                    txb.moveCall({
                        target: target,
                        arguments: [
                            txb.object(request.ProxyId),
                            txb.pure.string(request.Attachment.ContentId),
                            txb.pure.string(attribute.Name),
                            txb.pure.string(attribute.Value),
                        ]});
                }
            }
        });

        if (isEnoki) {
            result = new SuiEnokiTransaction();
            const kindBytes = await txb.build({ onlyTransactionKind: true, client: suiClient });
            result.Id = toBase64(kindBytes);
        }
        else {
            result = new SuiTransactionResult();
            const response = await sponsorTransaction(txb, updateRequests[0].PlayerWalletKey, realmKey, environment);
            setTransactionResponse(response, result);
        }

    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}


async function detachNft(callback: Callback<string>, request: string, realmKey: string, isEnoki: boolean, environment: Environment) {
    let error = null;
    const updateRequests: NftUpdateMessage[] = JSON.parse(request);
    let result = null;
    try {
        const suiClient = getSuiClientInstance(environment);
        const txb = new Transaction();

        updateRequests.forEach((item) => {
            if (item.Attachment) {
                const target: `${string}::${string}::${string}` = `${item.PackageId}::${item.Module}::${item.Function}`;
                txb.moveCall({
                    target: target,
                    arguments: [
                        txb.object(item.ProxyId),
                        txb.pure.string(item.Attachment.ContentId)
                    ]});
            }
        });

        if (isEnoki) {
            result = new SuiEnokiTransaction();
            const kindBytes = await txb.build({ onlyTransactionKind: true, client: suiClient });
            result.Id = toBase64(kindBytes);
        }
        else {
            result = new SuiTransactionResult();
            const response = await sponsorTransaction(txb, updateRequests[0].PlayerWalletKey, realmKey, environment);
            setTransactionResponse(response, result);
        }

    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function burnNft(callback: Callback<string>, request: string, realmKey: string, isEnoki: boolean, environment: Environment) {
    let error = null;
    const deleteRequests: NftDeleteMessage[] = JSON.parse(request);
    let result= null;
    try {
        const suiClient = getSuiClientInstance(environment);
        const gameKeypair = Ed25519Keypair.fromSecretKey(realmKey);
        const txb = new Transaction();
        const currentTime = Date.now();

        for (const request of deleteRequests) {
            const nftIdBytes = bcs.Address.serialize(request.ProxyId).toBytes();
            const timestampBytes = bcs.u64().serialize(currentTime).toBytes();

            // Combine all serialized bytes into a single Uint8Array
            const messageBytes = new Uint8Array([
                ...nftIdBytes,
                ...timestampBytes,
            ]);

            const signature = await gameKeypair.sign(messageBytes);
            const target: `${string}::${string}::${string}` = `${request.PackageId}::${request.Module}::${request.Function}`;
            txb.moveCall({
                target: target,
                arguments: [
                    txb.object(request.ProxyId),
                    txb.object(request.OwnerObjectId),
                    txb.object(SUI_CLOCK_OBJECT_ID),
                    txb.pure.vector("u8", signature),
                    txb.pure.u64(currentTime),
                ],
            });
        }

        if (isEnoki) {
            result = new SuiEnokiTransaction();
            const kindBytes = await txb.build({ onlyTransactionKind: true, client: suiClient });
            result.Id = toBase64(kindBytes);
        }
        else {
            result = new SuiTransactionResult();
            const response = await sponsorTransaction(txb, deleteRequests[0].PlayerWalletKey, realmKey, environment);
            setTransactionResponse(response, result);
        }

    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}
async function objectExists(callback: Callback<string>, itemId: string, environment: Environment) {
    let error = null;
    let result = true;
    try {
        const suiClient = getSuiClientInstance(environment);
        const objectResult = await suiClient.getObject({
            id: itemId,
        });

        if (objectResult.error !== undefined && objectResult.error !== null) {
            result = false;
        }

    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function getTargetCoins(owner: string, packageId: string, coin: string, target: number, suiClient: SuiClient) {
    const coinsFromClient: CoinModel[] = [];
    let coins_resp;
    let cursor = null;
    let targetFound = false;
    let sum = 0;
    do {
        coins_resp = await suiClient.getCoins({
            coinType: `${packageId}::${coin.toLowerCase()}::${coin.toUpperCase()}`,
            owner : owner,
            cursor: cursor,
            limit : 10
        });
        for (const coin of coins_resp.data) {
            coinsFromClient.push(new CoinModel(coin.coinObjectId, Number(coin.balance)))
            sum += Number(coin.balance);
        }

        if (sum >= target) {
            targetFound = true;
            break;
        }
        cursor = coins_resp?.nextCursor;
    } while (coins_resp.hasNextPage);

    if (!targetFound) {
        return [];
    }
    // Sort the array in ascending order based on the balance
    coinsFromClient.sort((a, b) => a.balance - b.balance);

    // Reduce sorted array to include only coins needed for transaction
    let accumulatedBalance = 0;
    return coinsFromClient.reduce((acc: CoinModel[], coin: CoinModel) => {
        if (accumulatedBalance < target) {
            acc.push(coin);
            accumulatedBalance += coin.balance;
        }
        return acc;
    }, []);
}

async function getGameTargetCoins(owner: string, packageId: string, coinModule: string, target: number, suiClient: SuiClient) {
    const coinsFromClient: CoinModel[] = [];
    let coins_resp;
    let cursor = null;
    let targetFound = false;
    let sum = 0;
    const coinType = `0x2::token::Token<${packageId}::${coinModule.toLowerCase()}::${coinModule.toUpperCase()}>`;
    do {
        coins_resp = await suiClient.getOwnedObjects({
            owner: owner,
            cursor: null,
            options: {
                showType: true,
                showContent: true,
                showDisplay: false
            },
            filter: {
                StructType: coinType
            },
            limit: 10
        });
        if (coins_resp.data != undefined) {
            for (const coin of coins_resp.data) {
                if (coin.data != undefined) {
                    switch (coin.data.content?.dataType) {
                        case 'moveObject':
                            const contentFields = coin.data.content?.fields;
                            if (contentFields != null) {
                                const balanceStr = (contentFields as { [key: string]: MoveValue })["balance"] as string;
                                const balance = stringToNumber(balanceStr);
                                coinsFromClient.push(new CoinModel(coin.data.objectId, Number(balance)));
                                sum += Number(balance);
                            }
                    }
                }
            }
        }
        if (sum >= target) {
            targetFound = true;
            break;
        }
        cursor = coins_resp?.nextCursor;
    } while (coins_resp.hasNextPage);

    if (!targetFound) {
        return [];
    }
    // Sort the array in ascending order based on the balance
    coinsFromClient.sort((a, b) => a.balance - b.balance);

    // Reduce sorted array to include only coins needed for transaction
    let accumulatedBalance = 0;
    return coinsFromClient.reduce((acc: CoinModel[], coin: CoinModel) => {
        if (accumulatedBalance < target) {
            acc.push(coin);
            accumulatedBalance += coin.balance;
        }
        return acc;
    }, []);
}

async function withdrawCurrency(callback: Callback<string>, request: string, developerKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const suiClient = getSuiClientInstance(environment);
        const currencyTransfer: CurrencyTransfer = JSON.parse(request);
        const playerKeypair = Ed25519Keypair.fromSecretKey(decodeSuiPrivateKey(currencyTransfer.PlayerWalletKey));
        const sourceWallet = playerKeypair.toSuiAddress();

        if (currencyTransfer != null) {
            currencyTransfer.Amount = Math.abs(currencyTransfer.Amount);
            const targetCoins= await getTargetCoins(sourceWallet, currencyTransfer.PackageId, currencyTransfer.Module, currencyTransfer.Amount, suiClient);
            if (!Array.isArray(targetCoins) || (Array.isArray(targetCoins) && targetCoins.length === 0)) {
                throw new Error(`Can't find target amount (${currencyTransfer.Amount}) of coins from address ${sourceWallet}.`);
            }
            const tx = new Transaction();

            if (targetCoins.length === 1) {
                const coin = targetCoins[0];
                if (coin.balance === currencyTransfer.Amount) {
                    tx.transferObjects([tx.object(coin.coinObjectId)], tx.pure.address(currencyTransfer.TargetWalletAddress));
                } else if (coin.balance > currencyTransfer.Amount) {
                    const [targetCoin] = tx.splitCoins(tx.object(coin.coinObjectId), [tx.pure.u64(currencyTransfer.Amount)]);
                    tx.transferObjects([targetCoin], tx.pure.address(currencyTransfer.TargetWalletAddress));
                }
            } else if (targetCoins.length > 1) {
                const totalBalance = targetCoins.reduce((sum, coin) => sum + coin.balance, 0);
                if (totalBalance === currencyTransfer.Amount) {
                    const coinsToTransfer = targetCoins.map(coin => tx.object(coin.coinObjectId));
                    tx.transferObjects(coinsToTransfer, tx.pure.address(currencyTransfer.TargetWalletAddress));
                } else {
                    targetCoins.sort((a, b) => a.balance - b.balance);
                    const balanceExceptLargest = targetCoins.slice(0, -1).reduce((sum, coin) => sum + coin.balance, 0);
                    const [targetCoinFromLargest] = tx.splitCoins(tx.object(targetCoins[targetCoins.length - 1].coinObjectId), [tx.pure.u64(currencyTransfer.Amount - balanceExceptLargest)]);
                    tx.transferObjects([targetCoinFromLargest], tx.pure.address(currencyTransfer.TargetWalletAddress));
                    const remainingCoinsToTransfer = targetCoins.slice(0, -1).map(coin => tx.object(coin.coinObjectId));
                    tx.transferObjects(remainingCoinsToTransfer, tx.pure.address(currencyTransfer.TargetWalletAddress));
                }
            }
            const response = await sponsorTransaction(tx, currencyTransfer.PlayerWalletKey, developerKey, environment);
            setTransactionResponse(response, result);
        } else {
            throw new Error(`Can't deserialize CurrencyTransfer object.`);
        }

    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function kioskListForSale(callback: Callback<string>, request: string, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const listRequest: KioskListRequest = JSON.parse(request);
        const tx = new Transaction();

        const target: `${string}::${string}::${string}` = `${listRequest.PackageId}::${listRequest.Module}::${listRequest.Function}`;
        tx.moveCall({
            target: target,
            arguments: [
                tx.object(listRequest.MarketplaceId),
                tx.object(listRequest.ItemId),
                tx.pure.u64(listRequest.Price)
            ]});

        const response = await sponsorTransaction(tx, listRequest.PlayerWalletKey, realmKey, environment);
        if (response.effects != null) {
            result.status = response.effects.status.status;
            result.gasUsed = calculateTotalCost(response.effects.gasUsed);
            result.digest = response.effects.transactionDigest;
            result.objectIds = response.effects.created?.map(o => o.reference.objectId);
            result.error = response.effects.status.error;
            const listing = response.objectChanges?.find(
                (change): change is Extract<SuiObjectChange, { type: "created" }> =>
                    change.type === "created" && change.objectType.endsWith("Listing")
            );
            result.createdId = listing?.objectId ?? "";
        }
    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function kioskDelistFromSale(callback: Callback<string>, request: string, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const listRequest: KioskDelistRequest = JSON.parse(request);
        const tx = new Transaction();

        const target: `${string}::${string}::${string}` = `${listRequest.PackageId}::${listRequest.Module}::${listRequest.Function}`;
        tx.moveCall({
            target: target,
            arguments: [
                tx.object(listRequest.MarketplaceId),
                tx.object(listRequest.ListingId)
            ]});

        const response = await sponsorTransaction(tx, listRequest.PlayerWalletKey, realmKey, environment);
        setTransactionResponse(response, result);
    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function getRegularCoinPurchaseTarget(owner: string, currencyPackageId: string, currencyModule: string, target: number, suiClient: SuiClient, tx: Transaction) {
    const targetCoins= await getTargetCoins(owner, currencyPackageId, currencyModule, target, suiClient);
    if (!Array.isArray(targetCoins) || (Array.isArray(targetCoins) && targetCoins.length === 0)) {
        throw new Error(`Can't find target amount (${target}) of coins from address ${owner}.`);
    }
    let paymentCoinArgument: TransactionArgument;
    if (targetCoins.length === 1) {
        const coin = targetCoins[0];
        if (coin.balance === target) {
            paymentCoinArgument = tx.object(coin.coinObjectId);
        } else if (coin.balance > target) {
            const [targetCoin] = tx.splitCoins(tx.object(coin.coinObjectId), [tx.pure.u64(target)]);
            paymentCoinArgument = targetCoin;
        } else {
            throw new Error(`Insufficient funds. Single coin balance ${coin.balance} is less than required price ${target}.`);
        }
    } else if (targetCoins.length > 1) {
        const totalBalance = targetCoins.reduce((sum, coin) => sum + coin.balance, 0);

        if (totalBalance < target) {
            throw new Error(`Insufficient funds. Total balance ${totalBalance} is less than required price ${target}.`);
        }

        if (totalBalance === target) {
            // First, convert all coin IDs to TransactionArguments
            const coinArguments = targetCoins.map(coin => tx.object(coin.coinObjectId));
            // Designate the first coin as the destination
            const destinationCoin = coinArguments[0];
            // The rest are the sources
            const sourceCoins = coinArguments.slice(1);
            // Now, call mergeCoins with both arguments
            tx.mergeCoins(destinationCoin, sourceCoins);
            // The destinationCoin is now the resulting merged coin
            paymentCoinArgument = destinationCoin;

        } else {
            targetCoins.sort((a, b) => a.balance - b.balance); // Smallest to largest
            const largestCoin = targetCoins[targetCoins.length - 1];
            const smallerCoins = targetCoins.slice(0, -1);

            const balanceFromSmallerCoins = smallerCoins.reduce((sum, coin) => sum + coin.balance, 0);
            const amountToSplitFromLargest = target - balanceFromSmallerCoins;

            // Split the exact remaining amount needed from the largest coin
            const [splitPieceFromLargest] = tx.splitCoins(
                tx.object(largestCoin.coinObjectId),
                [tx.pure.u64(amountToSplitFromLargest)]
            );
            // Now, we need to merge this new `splitPieceFromLargest` with all the smaller coins.
            const smallerCoinArguments = smallerCoins.map(coin => tx.object(coin.coinObjectId));
            // Let's use the new split piece as the destination for the merge
            const destinationCoin = splitPieceFromLargest;
            tx.mergeCoins(destinationCoin, smallerCoinArguments);
            // The result is our destination coin, which now holds the exact total price.
            paymentCoinArgument = destinationCoin;
        }
    } else {
        throw new Error(`Can't find target amount (${target}) of coins from address ${owner}.`);
    }
    return paymentCoinArgument;
}

async function getGameCoinPurchaseTarget(owner: string, packageId: string, coinModule: string, target: number, suiClient: SuiClient, tx: Transaction) {
    const targetCoins= await getGameTargetCoins(owner, packageId, coinModule, target, suiClient);
    if (!Array.isArray(targetCoins) || (Array.isArray(targetCoins) && targetCoins.length === 0)) {
        throw new Error(`Can't find target amount (${target}) of coins from address ${owner}.`);
    }
    //const coinType = `0x2::token::Token<${packageId}::${coinModule.toLowerCase()}::${coinModule.toUpperCase()}>`;
    const coinType = `${packageId}::${coinModule}::${coinModule.toUpperCase()}`;
    let paymentCoinArgument: TransactionArgument;
    const exactMatchToken = targetCoins.find(t => t.balance === target);
    const tokenToSplit = targetCoins.find(t => t.balance > target);

    if (exactMatchToken) {
        paymentCoinArgument = tx.object(exactMatchToken.coinObjectId);
    } else if (tokenToSplit) {
        paymentCoinArgument = tx.moveCall({
            target: '0x2::token::split',
            typeArguments: [coinType],
            arguments: [
                tx.object(tokenToSplit.coinObjectId),
                tx.pure.u64(target)
            ]
        });
    } else {
        const totalBalance = targetCoins.reduce((sum, token) => sum + token.balance, 0);
        if (totalBalance < target) {
            throw new Error(`Insufficient funds. Total balance of ${totalBalance} is less than required price ${target}.`);
        }
        const destinationToken = targetCoins[0];
        const sourceTokens = targetCoins.slice(1);
        const destinationTokenArg = tx.object(destinationToken.coinObjectId);
        // Loop and call 'token::join' for each source token.
        for (const source of sourceTokens) {
            tx.moveCall({
                target: '0x2::token::join',
                typeArguments: [coinType],
                arguments: [
                    destinationTokenArg,
                    tx.object(source.coinObjectId)
                ]
            });
        }
        // Now, `destinationTokenArg` represents the super-token with the total balance.
        // We split the required price from this newly merged token.
        paymentCoinArgument = tx.moveCall({
            target: '0x2::token::split',
            typeArguments: [coinType],
            arguments: [
                destinationTokenArg,
                tx.pure.u64(target)
            ]
        });
    }
    return paymentCoinArgument;
}

async function kioskPurchase(callback: Callback<string>, request: string, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    let paymentCoinArgument: undefined | TransactionArgument | string = undefined;
    let coinsFromClient: CoinModel[] = [];
    try {
        const purchaseRequest: KioskPurchaseRequest = JSON.parse(request);
        const suiClient = getSuiClientInstance(environment);
        const playerKeypair = Ed25519Keypair.fromSecretKey(decodeSuiPrivateKey(purchaseRequest.PlayerWalletKey));
        const sourceWallet = playerKeypair.toSuiAddress();
        const tx = new Transaction();

        const target: `${string}::${string}::${string}` = `${purchaseRequest.PackageId}::${purchaseRequest.Module}::${purchaseRequest.Function}`;

        if (!purchaseRequest.TokenPolicy) {
            paymentCoinArgument = await getRegularCoinPurchaseTarget(sourceWallet, purchaseRequest.CurrencyPackageId, purchaseRequest.CurrencyModule, purchaseRequest.Price, suiClient, tx);
            tx.moveCall({
                target: target,
                arguments: [
                    tx.object(purchaseRequest.MarketplaceId),
                    tx.object(purchaseRequest.ListingId),
                    paymentCoinArgument
                ]});
        }
        else {
            paymentCoinArgument = await getGameCoinPurchaseTarget(sourceWallet, purchaseRequest.CurrencyPackageId, purchaseRequest.CurrencyModule, purchaseRequest.Price, suiClient, tx);
            tx.moveCall({
                target: target,
                arguments: [
                    tx.object(purchaseRequest.MarketplaceId),
                    tx.object(purchaseRequest.ListingId),
                    tx.object(purchaseRequest.TokenPolicy),
                    paymentCoinArgument
                ]});
        }

        const response = await sponsorTransaction(tx, purchaseRequest.PlayerWalletKey, realmKey, environment);
        setTransactionResponse(response, result);
    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function transferSui(callback: Callback<string>, toAddress: string, amount: number, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const suiClient = getSuiClientInstance(environment);
        const keypair = Ed25519Keypair.fromSecretKey(decodeSuiPrivateKey(realmKey));
        const tx = new Transaction();
        const [coin] = tx.splitCoins(tx.gas, [amount]);
        tx.transferObjects([coin], toAddress);
        const response = await suiClient.signAndExecuteTransaction({
            signer: keypair,
            transaction: tx,
            options: {
                showEffects: true
            }
        });
        setTransactionResponse(response, result);
    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function personalKioskCreate(callback: Callback<string>, request: string, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const createRequest: PersonalKioskCreateRequest = JSON.parse(request);
        const playerKeypair = Ed25519Keypair.fromSecretKey(decodeSuiPrivateKey(createRequest.PlayerWalletKey));
        const tx = new Transaction();
        const kioskClient= getKioskClientInstance(environment, createRequest.CustomPackageIds);
        const kioskTx = new KioskTransaction({ transaction: tx, kioskClient });

        kioskTx.createPersonal().finalize();

        const suiClient = getSuiClientInstance(environment);
        const response = await suiClient.signAndExecuteTransaction({
            signer: playerKeypair,
            transaction: tx,
            options: {
                showEffects: true,
                showObjectChanges: true
            }
        });

        if (response.effects != null) {
            result.status = response.effects.status.status;
            result.gasUsed = calculateTotalCost(response.effects.gasUsed);
            result.digest = response.effects.transactionDigest;
            result.objectIds = response.effects.created?.map(o => o.reference.objectId);
            result.error = response.effects.status.error;
            result.metadata ??= {};
            const kiosk = response.objectChanges?.find(
                (change): change is Extract<SuiObjectChange, { type: "created" }> =>
                    change.type === "created" && change.objectType.startsWith("0x2::kiosk::Kiosk")
            );
            result.metadata["kioskId"] = kiosk?.objectId ?? "";
            const personalKioskCap = response.objectChanges?.find(
                (change): change is Extract<SuiObjectChange, { type: "created" }> =>
                    change.type === "created" && change.objectType.endsWith("personal_kiosk::PersonalKioskCap")
            );
            result.metadata["personalKioskCap"] = personalKioskCap?.objectId ?? "";
        }

    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function personalKioskList(callback: Callback<string>, request: string, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const listRequest: PersonalKioskListRequest = JSON.parse(request);
        const playerKeypair = Ed25519Keypair.fromSecretKey(decodeSuiPrivateKey(listRequest.PlayerWalletKey));
        const tx = new Transaction();
        const kioskClient= getKioskClientInstance(environment, listRequest.CustomPackageIds);
        const { kioskOwnerCaps } = await kioskClient.getOwnedKiosks({ address: playerKeypair.toSuiAddress()});
        const playerCap = kioskOwnerCaps.find(cap => cap.objectId === listRequest.PersonalKioskCap);
        const kioskTx = new KioskTransaction({ transaction: tx, kioskClient, cap: playerCap });

        if (!listRequest.ExclusiveBuyerWallet) {
            kioskTx
                .placeAndList({
                    itemType: listRequest.ItemType,
                    item: listRequest.ItemId,
                    price: listRequest.Price,
                })
                .finalize();
        } else {
            let targetPackage = listRequest.CustomPackageIds?.personalKioskRulePackageId;
            if (!targetPackage) {
                targetPackage = PERSONAL_KIOSK_RULE_ADDRESS[EnvironmentNetwork[environment]];
            }
            const [kioskOwnerCap, borrow] = tx.moveCall({
                target: `${targetPackage}::personal_kiosk::borrow_val`,
                arguments: [
                    tx.object(playerCap!.objectId)
                ],
            });

            tx.moveCall({
                target: '0x2::kiosk::list_with_purchase_cap',
                typeArguments: [listRequest.ItemType],
                arguments: [
                    tx.object(listRequest.KioskId),
                    kioskOwnerCap,
                    tx.object(listRequest.ItemId),
                    tx.pure.u64(listRequest.Price),
                    tx.pure.address(listRequest.ExclusiveBuyerWallet)
                ],
            });

            tx.moveCall({
                target: `${targetPackage}::personal_kiosk::return_val`,
                arguments: [
                    tx.object(playerCap!.objectId),
                    kioskOwnerCap,
                    borrow
                ]
            });

        }

        const suiClient = getSuiClientInstance(environment);
        const response = await suiClient.signAndExecuteTransaction({
            signer: playerKeypair,
            transaction: tx,
            options: {
                showEffects: true,
                showObjectChanges: true
            }
        });
        if (response.effects != null) {
            result.status = response.effects.status.status;
            result.gasUsed = calculateTotalCost(response.effects.gasUsed);
            result.digest = response.effects.transactionDigest;
            result.objectIds = response.effects.created?.map(o => o.reference.objectId);
            result.error = response.effects.status.error;
            result.metadata ??= {};
            const listing = response.objectChanges?.find(
                (change): change is Extract<SuiObjectChange, { type: "created" }> =>
                    change.type === "created" && change.objectType.endsWith("Wrapper<0x2::kiosk::Item>, 0x2::object::ID>")
            );
            result.createdId = listing?.objectId ?? "";

            if (listRequest.ExclusiveBuyerWallet) {
                const kioskPurchaseCap = response.objectChanges?.find(
                    (change): change is Extract<SuiObjectChange, { type: "created" }> =>
                        change.type === "created" && change.objectType.startsWith("0x2::kiosk::PurchaseCap")
                );
                result.metadata["kioskPurchaseCap"] = kioskPurchaseCap?.objectId ?? "";
            }
        }
    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function personalKioskDelist(callback: Callback<string>, request: string, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const listRequest: PersonalKioskDelistRequest = JSON.parse(request);
        const playerKeypair = Ed25519Keypair.fromSecretKey(decodeSuiPrivateKey(listRequest.PlayerWalletKey));
        const tx = new Transaction();
        const kioskClient= getKioskClientInstance(environment, listRequest.CustomPackageIds);
        const { kioskOwnerCaps } = await kioskClient.getOwnedKiosks({ address: playerKeypair.toSuiAddress()});
        const playerCap = kioskOwnerCaps.find(cap => cap.objectId === listRequest.PersonalKioskCap);
        const kioskTx = new KioskTransaction({ transaction: tx, kioskClient, cap: playerCap });

        kioskTx
            .delist({
                itemType: listRequest.ItemType,
                itemId: listRequest.ItemId,
            });
        kioskTx
            .transfer({
                itemType: listRequest.ItemType,
                itemId: listRequest.ItemId,
                address: playerKeypair.toSuiAddress()
            });
        kioskTx.finalize();

        const suiClient = getSuiClientInstance(environment);
        const response = await suiClient.signAndExecuteTransaction({
            signer: playerKeypair,
            transaction: tx,
            options: {
                showEffects: true,
                showObjectChanges: true
            }
        });
        if (response.effects != null) {
            result.status = response.effects.status.status;
            result.gasUsed = calculateTotalCost(response.effects.gasUsed);
            result.digest = response.effects.transactionDigest;
            result.objectIds = response.effects.created?.map(o => o.reference.objectId);
            result.error = response.effects.status.error;
        }
    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function personalKioskTake(callback: Callback<string>, request: string, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const listRequest: PersonalKioskTakeRequest = JSON.parse(request);
        const playerKeypair = Ed25519Keypair.fromSecretKey(decodeSuiPrivateKey(listRequest.PlayerWalletKey));
        const tx = new Transaction();
        const kioskClient= getKioskClientInstance(environment, listRequest.CustomPackageIds);
        const { kioskOwnerCaps } = await kioskClient.getOwnedKiosks({ address: playerKeypair.toSuiAddress()});
        const playerCap = kioskOwnerCaps.find(cap => cap.objectId === listRequest.PersonalKioskCap);
        const kioskTx = new KioskTransaction({ transaction: tx, kioskClient, cap: playerCap });

         kioskTx
            .transfer({
                itemType: listRequest.ItemType,
                itemId: listRequest.ItemId,
                address: playerKeypair.toSuiAddress()
            })
            .finalize();

        const suiClient = getSuiClientInstance(environment);
        const response = await suiClient.signAndExecuteTransaction({
            signer: playerKeypair,
            transaction: tx,
            options: {
                showEffects: true,
                showObjectChanges: true
            }
        });
        if (response.effects != null) {
            result.status = response.effects.status.status;
            result.gasUsed = calculateTotalCost(response.effects.gasUsed);
            result.digest = response.effects.transactionDigest;
            result.objectIds = response.effects.created?.map(o => o.reference.objectId);
            result.error = response.effects.status.error;
        }
    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function personalKioskDeclinePurchase(callback: Callback<string>, request: string, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const listRequest: PersonalKioskDeclinePurchaseRequest = JSON.parse(request);
        const tx = new Transaction();

        tx.transferObjects([tx.object(listRequest.PurchaseCap)], listRequest.Seller);

        const response = await sponsorTransaction(tx, listRequest.PlayerWalletKey, realmKey, environment);
        if (response.effects != null) {
            result.status = response.effects.status.status;
            result.gasUsed = calculateTotalCost(response.effects.gasUsed);
            result.digest = response.effects.transactionDigest;
            result.objectIds = response.effects.created?.map(o => o.reference.objectId);
            result.error = response.effects.status.error;
        }
    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function personalKioskCancelExclusive(callback: Callback<string>, request: string, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const listRequest: PersonalKioskCancelExclusiveRequest = JSON.parse(request);
        const playerKeypair = Ed25519Keypair.fromSecretKey(decodeSuiPrivateKey(listRequest.PlayerWalletKey));
        const kioskClient= getKioskClientInstance(environment, listRequest.CustomPackageIds);
        const { kioskOwnerCaps } = await kioskClient.getOwnedKiosks({ address: playerKeypair.toSuiAddress()});
        const playerCap = kioskOwnerCaps.find(cap => cap.objectId === listRequest.PersonalKioskCap);
        const tx = new Transaction();

        const [item_id] = tx.moveCall({
            target: '0x2::kiosk::purchase_cap_item',
            typeArguments: [listRequest.ItemType],
            arguments: [
                tx.object(listRequest.PurchaseCap)
            ],
        });

        tx.moveCall({
            target: '0x2::kiosk::return_purchase_cap',
            typeArguments: [listRequest.ItemType],
            arguments: [
                tx.object(listRequest.KioskId),
                tx.object(listRequest.PurchaseCap)
            ],
        });

        let targetPackage = listRequest.CustomPackageIds?.personalKioskRulePackageId;
        if (!targetPackage) {
            targetPackage = PERSONAL_KIOSK_RULE_ADDRESS[EnvironmentNetwork[environment]];
        }

        const [kioskOwnerCap, borrow] = tx.moveCall({
            target: `${targetPackage}::personal_kiosk::borrow_val`,
            arguments: [
                tx.object(playerCap!.objectId)
            ],
        });

        tx.moveCall({
            target: '0x2::kiosk::take',
            typeArguments: [listRequest.ItemType],
            arguments: [
                tx.object(listRequest.KioskId),
                kioskOwnerCap,
                item_id
            ],
        });

        tx.moveCall({
            target: `${targetPackage}::personal_kiosk::return_val`,
            arguments: [
                tx.object(playerCap!.objectId),
                kioskOwnerCap,
                borrow
            ]
        });

        const suiClient = getSuiClientInstance(environment);
        const response = await suiClient.signAndExecuteTransaction({
            signer: playerKeypair,
            transaction: tx,
            options: {
                showEffects: true,
                showObjectChanges: true
            }
        });
        if (response.effects != null) {
            result.status = response.effects.status.status;
            result.gasUsed = calculateTotalCost(response.effects.gasUsed);
            result.digest = response.effects.transactionDigest;
            result.objectIds = response.effects.created?.map(o => o.reference.objectId);
            result.error = response.effects.status.error;
        }
    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function personalKioskPurchase(callback: Callback<string>, request: string, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const purchaseRequest: PersonalKioskPurchaseRequest = JSON.parse(request);
        const playerKeypair = Ed25519Keypair.fromSecretKey(decodeSuiPrivateKey(purchaseRequest.PlayerWalletKey));
        const kioskClient= getKioskClientInstance(environment, purchaseRequest.CustomPackageIds);
        const { kioskOwnerCaps } = await kioskClient.getOwnedKiosks({ address: playerKeypair.toSuiAddress()});
        const playerCap = kioskOwnerCaps.find(cap => cap.objectId === purchaseRequest.PersonalKioskCap);
        const tx = new Transaction();

        if (!purchaseRequest.PurchaseCap) {
            const kioskTx = new KioskTransaction({ transaction: tx, kioskClient, cap: playerCap });
            await kioskTx.purchaseAndResolve({
                itemType: purchaseRequest.ItemType,
                itemId: purchaseRequest.ItemId,
                price: purchaseRequest.Price.toString(),
                sellerKiosk: purchaseRequest.SellerKioskId,
            });
            kioskTx.finalize();
        } else {
            let targetKioskPackage = purchaseRequest.CustomPackageIds?.personalKioskRulePackageId;
            let targetRoyaltyPackage = purchaseRequest.CustomPackageIds?.royaltyRulePackageId;
            if (!targetKioskPackage) {
                targetKioskPackage = PERSONAL_KIOSK_RULE_ADDRESS[EnvironmentNetwork[environment]];
            }
            if (!targetRoyaltyPackage) {
                targetRoyaltyPackage = ROYALTY_RULE_ADDRESS[EnvironmentNetwork[environment]];
            }

            const [paymentCoin] = tx.splitCoins(tx.gas, [purchaseRequest.Price]);
            const [paymentCoinRoyalty] = tx.splitCoins(tx.gas, [purchaseRequest.Price]);

            // 1. Purchase with the PurchaseCap
            const [item, transferRequest] = tx.moveCall({
                target: '0x2::kiosk::purchase_with_cap',
                arguments: [
                    tx.object(purchaseRequest.KioskId),
                    tx.object(purchaseRequest.PurchaseCap),
                    paymentCoin,
                ],
                typeArguments: [purchaseRequest.ItemType],
            });

            // 2. Manually resolve the royalty rule
            tx.moveCall({
                target: `${targetRoyaltyPackage}::royalty_rule::pay`,
                arguments: [
                    tx.object(purchaseRequest.PolicyId),
                    transferRequest,
                    tx.object(paymentCoinRoyalty),
                ],
                typeArguments: [purchaseRequest.ItemType],
            });

            // 3. Confirm the TransferRequest
            tx.moveCall({
                target: '0x2::transfer_policy::confirm_request',
                arguments: [
                    tx.object(purchaseRequest.PolicyId),
                    transferRequest,
                ],
                typeArguments: [purchaseRequest.ItemType],
            });

            tx.transferObjects([item], playerKeypair.toSuiAddress());
        }

        const suiClient = getSuiClientInstance(environment);
        const response = await suiClient.signAndExecuteTransaction({
            signer: playerKeypair,
            transaction: tx,
            options: {
                showEffects: true
            }
        });
        setTransactionResponse(response, result);

    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

async function personalKioskWithdraw(callback: Callback<string>, request: string, realmKey: string, environment: Environment) {
    let error = null;
    const result = new SuiTransactionResult();
    try {
        const withdrawRequest: PersonalKioskWithdrawRequest = JSON.parse(request);
        const playerKeypair = Ed25519Keypair.fromSecretKey(decodeSuiPrivateKey(withdrawRequest.PlayerWalletKey));
        const kioskClient= getKioskClientInstance(environment, withdrawRequest.CustomPackageIds);
        const { kioskOwnerCaps } = await kioskClient.getOwnedKiosks({ address: playerKeypair.toSuiAddress()});
        const playerCap = kioskOwnerCaps.find(cap => cap.objectId === withdrawRequest.PersonalKioskCap);
        const tx = new Transaction();
        const kioskTx = new KioskTransaction({ transaction: tx, kioskClient, cap: playerCap });


        kioskTx
            .withdraw(playerKeypair.toSuiAddress(), withdrawRequest.Amount)
            .finalize();

        const suiClient = getSuiClientInstance(environment);
        const response = await suiClient.signAndExecuteTransaction({
            signer: playerKeypair,
            transaction: tx,
            options: {
                showEffects: true
            }
        });
        setTransactionResponse(response, result);

    } catch (ex) {
        error = ex;
    }
    callback(error, JSON.stringify(result));
}

module.exports = {
    createWallet,
    createEphemeral,
    getLatestEpoch,
    importWallet,
    getGasInfo,
    verifySignature,
    mintRegularCoin,
    getBalance,
    mintNfts,
    getOwnedObjects,
    burnRegularCoin,
    mintGameCoin,
    burnGameCoin,
    getGameCoinBalance,
    setNftContractOwner,
    updateNft,
    attachNft,
    detachNft,
    objectExists,
    burnNft,
    withdrawCurrency,
    enokiSign,
    getAttachments,
    updateNftAttachment,
    kioskListForSale,
    kioskDelistFromSale,
    kioskPurchase,
    transferSui,
    personalKioskCreate,
    personalKioskList,
    personalKioskDelist,
    personalKioskTake,
    personalKioskDeclinePurchase,
    personalKioskCancelExclusive,
    personalKioskPurchase,
    personalKioskWithdraw
};