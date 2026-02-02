import {DisplayFieldsResponse, MoveStruct, MoveValue, SuiParsedData} from "@mysten/sui/client";

export class CreateWalletResponse {
    Address: string | undefined;
    PrivateKey: string | undefined;
    PublicKey: string | undefined;
}

export class SuiTransactionResult {
    status: string | undefined;
    digest: string | undefined;
    gasUsed: string | undefined;
    objectIds: string[] | undefined;
    error: string | undefined;
    createdId: string | undefined;
    metadata?: { [key: string]: string };
}

export interface PersonalKioskCreateRequest {
    PackageId: string;
    Module: string;
    Function: string;
    PlayerWalletKey: string;
    CustomPackageIds?: CustomRulePackageIds;
}

export interface PersonalKioskListRequest {
    PackageId: string;
    Module: string;
    Function: string;
    PlayerWalletKey: string;
    KioskId: string;
    PersonalKioskCap: string;
    ItemType: string;
    ItemId: string;
    Price: string;
    ExclusiveBuyerWallet: string;
    CustomPackageIds?: CustomRulePackageIds;
}

export interface PersonalKioskDeclinePurchaseRequest {
    PackageId: string;
    Module: string;
    Function: string;
    PlayerWalletKey: string;
    ItemType: string;
    PurchaseCap: string;
    Seller: string;
}

export interface PersonalKioskCancelExclusiveRequest {
    PackageId: string;
    Module: string;
    Function: string;
    PlayerWalletKey: string;
    ItemType: string;
    PurchaseCap: string;
    KioskId: string;
    PersonalKioskCap: string;
    CustomPackageIds?: CustomRulePackageIds;
}

export interface PersonalKioskPurchaseRequest {
    PackageId: string;
    Module: string;
    Function: string;
    PlayerWalletKey: string;
    ItemType: string;
    PurchaseCap: string;
    ItemId: string;
    KioskId: string;
    SellerKioskId: string;
    PersonalKioskCap: string;
    PolicyId: string;
    Price: number;
    Royalty: number;
    CustomPackageIds?: CustomRulePackageIds;
}

export interface PersonalKioskWithdrawRequest {
    PackageId: string;
    Module: string;
    Function: string;
    PlayerWalletKey: string;
    KioskId: string;
    PersonalKioskCap: string;
    Amount: number;
    CustomPackageIds?: CustomRulePackageIds;
}

export interface PersonalKioskDelistRequest {
    PackageId: string;
    Module: string;
    Function: string;
    PlayerWalletKey: string;
    KioskId: string;
    PersonalKioskCap: string;
    ItemType: string;
    ItemId: string;
    CustomPackageIds?: CustomRulePackageIds;
}

export interface PersonalKioskTakeRequest {
    PackageId: string;
    Module: string;
    Function: string;
    PlayerWalletKey: string;
    KioskId: string;
    PersonalKioskCap: string;
    ItemType: string;
    ItemId: string;
    CustomPackageIds?: CustomRulePackageIds;
}

export interface KioskListRequest {
    PackageId: string;
    Module: string;
    Function: string;
    MarketplaceId: string;
    ItemId: string;
    Price: string;
    PlayerWalletKey: string;
}

export interface KioskDelistRequest {
    PackageId: string;
    Module: string;
    Function: string;
    MarketplaceId: string;
    ListingId: string;
    PlayerWalletKey: string;
}

export interface KioskPurchaseRequest {
    PackageId: string;
    Module: string;
    Function: string;
    MarketplaceId: string;
    ListingId: string;
    PlayerWalletKey: string;
    CurrencyPackageId: string;
    CurrencyModule: string;
    Price: number;
    TokenPolicy: string;
}

export class SuiEnokiTransaction {
    Id: string | undefined;
}

export interface ZkLoginProofData {
    proofPoints: {
        a: string[];
        b: string[][];
        c: string[];
    };
    issBase64Details: {
        value: string;
        indexMod4: number;
    };
    headerBase64: string;
    addressSeed: string;
}

export interface RegularCoinMintMessage {
    PackageId: string;
    Module: string;
    Function: string;
    PlayerWalletAddress: string;
    AdminCap: string;
    TreasuryCap: string;
    Amount: number;
}

export interface RegularCoinBurnMessage {
    PackageId: string;
    Module: string;
    Function: string;
    PlayerWalletAddress: string;
    TreasuryCap: string;
    Amount: number;
    PlayerWalletKey: string;
}

export interface GameCoinMintMessage {
    PackageId: string;
    Module: string;
    Function: string;
    PlayerWalletAddress: string;
    AdminCap: string;
    TokenPolicy: string;
    TokenPolicyCap: string;
    Store: string;
    Amount: number;
}

export interface GameCoinBurnMessage {
    PackageId: string;
    Module: string;
    Function: string;
    PlayerWalletAddress: string;
    TokenPolicy: string;
    TokenPolicyCap: string;
    Store: string;
    Amount: number;
    PlayerWalletKey: string;
}

export interface CoinBalanceRequest {
    PackageId: string;
    ModuleName: string;
}

export interface RegularCoinBalanceRequest {
    PackageId: string;
    ModuleName: string;
}

export class CoinBalanceResponse {
    CoinType: string;
    Total: number;
    public constructor(coinType: string, total: number) {
        this.CoinType = coinType;
        this.Total = total;
    }
}

export interface NftMintMessage {
    ContentId: string;
    PackageId: string;
    Module: string;
    Function: string;
    PlayerWalletAddress: string;
    AdminCap: string;
    NftContentItem: NftContentItem;
}

export interface SetNftOwnerMessage {
    PackageId: string;
    Module: string;
    Function: string;
    AdminCap: string;
}

export interface NftContentItem {
    Name: string;
    Url: string;
    Description: string;
    ContentId: string;
    Attributes: Attribute[]
}

export interface Attribute {
    Name: string;
    Value: string;
}

export interface NftUpdateMessage {
    ProxyId: string;
    PackageId: string;
    Module: string;
    Function: string;
    PlayerWalletKey: string;
    PlayerWalletAddress: string;
    OwnerObjectId: string;
    Attachment: NftContentItem | null,
    Attributes: Attribute[]
}

export interface NftDeleteMessage {
    ProxyId: string;
    PackageId: string;
    Module: string;
    Function: string;
    PlayerWalletKey: string;
    PlayerWalletAddress: string;
    OwnerObjectId: string;
}

export class CoinToken {
    Id: string;
    Balance: number;
    public constructor(Id: string, Balance: number) {
        this.Id = Id;
        this.Balance = Balance;
    }
}

export interface PaginatedResult<T> {
    data: T[];
    hasNextPage: boolean;
    nextCursor?: string | null;
}
export interface InputParams {
    cursor?: string | null | undefined;
}

export interface CurrencyTransfer {
    PackageId: string;
    Module: string;
    PlayerWalletAddress: string;
    PlayerWalletKey: string;
    TargetWalletAddress: string;
    Amount: number;
}

export class SuiGameCoin {
    ObjectId: string;
    Balance: number;
    public constructor(objectId: string, balance: number) {
        this.ObjectId = objectId;
        this.Balance = balance;
    }
}

export class CoinModel {
    coinObjectId: string;
    balance: number;
    public constructor(coinObjectId: string, balance: number) {
        this.coinObjectId = coinObjectId;
        this.balance = balance;
    }
}

export class SuiObject {
    ObjectId: string;
    Digest: string;
    Version: string;
    Content?: SuiParsedData | null;
    Display?: DisplayFieldsResponse | null;
    public constructor(objectId: string, digest: string, version: string, content?: SuiParsedData | null, display?: DisplayFieldsResponse | null) {
        this.ObjectId = objectId;
        this.Digest = digest;
        this.Version = version;
        this.Content = content;
        this.Display = display;
    }

    toView(): SuiViewObject {
        const viewObject = new SuiViewObject(this.ObjectId);

        switch (this.Content?.dataType) {
            case 'moveObject':
                const typeParts = this.Content?.type.split("::");
                if (typeParts.length >= 3) {
                    viewObject.Type = typeParts[1];
                } else {
                    viewObject.Type = this.Content?.type;
                }

                const contentFields = this.Content?.fields;
                if (contentFields != null) {
                    viewObject.Name = (contentFields as { [key: string]: MoveValue })["name"] as string;
                    viewObject.Description = (contentFields as { [key: string]: MoveValue })["description"] as string;
                    viewObject.Image = (contentFields as { [key: string]: MoveValue })["url"] as string;
                    viewObject.ContentId = (contentFields as { [key: string]: MoveValue })["contentId"] as string;

                    const attributes = (contentFields as { [key: string]: MoveStruct })["attributes"];
                    if (attributes != undefined) {
                        if (Array.isArray(attributes)) {
                            attributes.forEach(a => {
                                const field = a as { fields: { [key: string]: MoveValue }; type: string; };
                                if (field != undefined) {
                                    const name = field.fields["name"] as string;
                                    const value = field.fields["value"] as string;
                                    viewObject.addAttribute(name, value);
                                }
                            });
                        }
                    }
                }
        }
        return viewObject;
    }

    toViewAttachment(): SuiViewObject {
        const viewObject = new SuiViewObject(this.ObjectId);

        switch (this.Content?.dataType) {
            case 'moveObject':
                const typeParts = this.Content?.type.split("::");
                if (typeParts.length >= 3) {
                    viewObject.Type = typeParts[3];
                } else {
                    viewObject.Type = this.Content?.type;
                }

                const contentValueFields = (this.Content?.fields as { [key: string]: MoveStruct })["value"];
                const contentFields = (contentValueFields as { [key: string]: MoveStruct })["fields"];
                if (contentFields != null) {
                    viewObject.Name = (contentFields as { [key: string]: MoveValue })["name"] as string;
                    viewObject.Description = (contentFields as { [key: string]: MoveValue })["description"] as string;
                    viewObject.Image = (contentFields as { [key: string]: MoveValue })["url"] as string;
                    viewObject.ContentId = (contentFields as { [key: string]: MoveValue })["contentId"] as string;

                    const attributes = (contentFields as { [key: string]: MoveStruct })["attributes"];
                    if (attributes != undefined) {
                        if (Array.isArray(attributes)) {
                            attributes.forEach(a => {
                                const field = a as { fields: { [key: string]: MoveValue }; type: string; };
                                if (field != undefined) {
                                    const name = field.fields["name"] as string;
                                    const value = field.fields["value"] as string;
                                    viewObject.addAttribute(name, value);
                                }
                            });
                        }
                    }
                }
        }
        return viewObject;
    }
}

export class SuiViewObject {
    ObjectId: string;
    Type: string | undefined;
    Name: string | undefined;
    Description: string | undefined;
    Image: string | undefined;
    ContentId: string | undefined;
    Attributes: Attribute[] = [];
    public constructor(objectId: string) {
        this.ObjectId = objectId;
    }

    addAttribute(name: string, value: string): void {
        const newAttribute: Attribute = { Name: name, Value: value };
        this.Attributes.push(newAttribute);
    }
}

export type CustomRulePackageIds = {
    royaltyRulePackageId?: string;
    kioskLockRulePackageId?: string;
    personalKioskRulePackageId?: string;
    floorPriceRulePackageId?: string;
};

export enum Network {
    MAINNET = 'mainnet',
    TESTNET = 'testnet',
    CUSTOM = 'custom',
}

export const ROYALTY_RULE_ADDRESS: Record<Network, string> = {
    [Network.TESTNET]: 'bd8fc1947cf119350184107a3087e2dc27efefa0dd82e25a1f699069fe81a585',
    [Network.MAINNET]: '0x434b5bd8f6a7b05fede0ff46c6e511d71ea326ed38056e3bcd681d2d7c2a7879',
    [Network.CUSTOM]: '',
};

export const KIOSK_LOCK_RULE_ADDRESS: Record<Network, string> = {
    [Network.TESTNET]: 'bd8fc1947cf119350184107a3087e2dc27efefa0dd82e25a1f699069fe81a585',
    [Network.MAINNET]: '0x434b5bd8f6a7b05fede0ff46c6e511d71ea326ed38056e3bcd681d2d7c2a7879',
    [Network.CUSTOM]: '',
};

export const FLOOR_PRICE_RULE_ADDRESS: Record<Network, string> = {
    [Network.TESTNET]: '0x06f6bdd3f2e2e759d8a4b9c252f379f7a05e72dfe4c0b9311cdac27b8eb791b1',
    [Network.MAINNET]: '0x34cc6762780f4f6f153c924c0680cfe2a1fb4601e7d33cc28a92297b62de1e0e',
    [Network.CUSTOM]: '',
};

export const PERSONAL_KIOSK_RULE_ADDRESS: Record<Network, string> = {
    [Network.TESTNET]: '0x06f6bdd3f2e2e759d8a4b9c252f379f7a05e72dfe4c0b9311cdac27b8eb791b1',
    [Network.MAINNET]: '0x0cb4bcc0560340eb1a1b929cabe56b33fc6449820ec8c1980d69bb98b649b802',
    [Network.CUSTOM]: '',
};