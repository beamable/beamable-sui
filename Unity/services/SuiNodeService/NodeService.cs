using System.Threading.Tasks;
using Jering.Javascript.NodeJS;

namespace SuiNodeServicve
{
    /// <summary>
    /// Service for calling SUI SDK functions
    /// </summary>
    public static class NodeService
    {
        private const string BridgeModulePath = "js/bridge.js";

        /// <summary>
        /// Creates SUI wallet keypair
        /// </summary>
        /// <returns></returns>
        public static async Task<string> CreateWallet()
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "createWallet") ?? "";
        }

        /// <summary>
        /// GetCurrentEpoch
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetCurrentEpoch(string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "getLatestEpoch",
                new object[] { environment }) ?? "";
        }

        /// <summary>
        /// GetGasInfo
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetGasInfo(string digest, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "getGasInfo",
                new object[] { digest, environment }) ?? "";
        }

        /// <summary>
        /// Creates SUI ephemeral wallet keypair
        /// </summary>
        /// <returns></returns>
        public static async Task<string> CreateEphemeral()
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "createEphemeral") ?? "";
        }

        /// <summary>
        /// Import SUI wallet from a private key
        /// </summary>
        /// <returns></returns>
        public static async Task<string> ImportWallet(string privateKey)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "importWallet",
                new object[] { privateKey }) ?? "";
        }

        /// <summary>
        /// Import SUI wallet from a private key
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> VerifySignature(string token, string challenge, string solution)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<bool>(
                BridgeModulePath,
                "verifySignature",
                new object[] { token, challenge, solution });
        }

        /// <summary>
        /// Mint regular coin
        /// </summary>
        /// <param name="mintRequestJson"></param>
        /// <param name="realmAccountPrivateKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> MintRegularCoin(string mintRequestJson, string realmAccountPrivateKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "mintRegularCoin",
                new object[] { mintRequestJson, realmAccountPrivateKey, environment }) ?? "";
        }

        /// <summary>
        /// Burn regular coin
        /// </summary>
        /// <param name="mintRequestJson"></param>
        /// <param name="realmAccountPrivateKey"></param>
        /// <param name="isEnoki"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> BurnRegularCoin(string mintRequestJson, string realmAccountPrivateKey, bool isEnoki, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "burnRegularCoin",
                new object[] { mintRequestJson, realmAccountPrivateKey, isEnoki, environment }) ?? "";
        }

        /// <summary>
        /// EnokiSignTransaction
        /// </summary>
        /// <param name="zkLoginProof"></param>
        /// <param name="maxEpoch"></param>
        /// <param name="ephemeralKey"></param>
        /// <param name="transactionBytes"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> EnokiSignTransaction(string zkLoginProof, long maxEpoch, string transactionBytes,
            string ephemeralKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "enokiSign",
                new object[] { zkLoginProof, maxEpoch, transactionBytes, ephemeralKey, environment }) ?? "";
        }

        /// <summary>
        /// Mint game coin
        /// </summary>
        /// <param name="mintRequestJson"></param>
        /// <param name="realmAccountPrivateKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> MintGameCoin(string mintRequestJson, string realmAccountPrivateKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "mintGameCoin",
                new object[] { mintRequestJson, realmAccountPrivateKey, environment }) ?? "";
        }

        /// <summary>
        /// Burn game coin
        /// </summary>
        /// <param name="mintRequestJson"></param>
        /// <param name="realmAccountPrivateKey"></param>
        /// <param name="isEnoki"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> BurnGameCoin(string mintRequestJson, string realmAccountPrivateKey, bool isEnoki, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "burnGameCoin",
                new object[] { mintRequestJson, realmAccountPrivateKey, isEnoki, environment }) ?? "";
        }

        /// <summary>
        /// CoinBalance
        /// </summary>
        /// <param name="wallet"></param>
        /// <param name="requestJson"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> CoinBalance(string wallet, string requestJson, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "getBalance",
                new object[] { wallet, requestJson, environment }) ?? "";
        }

        /// <summary>
        /// GetGameCoinBalance
        /// </summary>
        /// <param name="wallet"></param>
        /// <param name="requestJson"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> GetGameCoinBalance(string wallet, string requestJson, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "getGameCoinBalance",
                new object[] { wallet, requestJson, environment }) ?? "";
        }

        /// <summary>
        /// Mint NFTs
        /// </summary>
        /// <param name="mintRequestJson"></param>
        /// <param name="realmAccountPrivateKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> MintNfts(string mintRequestJson, string realmAccountPrivateKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "mintNfts",
                new object[] { mintRequestJson, realmAccountPrivateKey, environment }) ?? "";
        }

        /// <summary>
        /// GetOwnedObjects
        /// </summary>
        /// <param name="wallet"></param>
        /// <param name="packageId"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> GetOwnedObjects(string wallet, string packageId, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "getOwnedObjects",
                new object[] { wallet, packageId, environment }) ?? "";
        }

        /// <summary>
        /// GetAttachments
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> GetAttachments(string objectId, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "getAttachments",
                new object[] { objectId, environment }) ?? "";
        }

        /// <summary>
        /// UpdateNfts
        /// </summary>
        /// <param name="updateRequestJson"></param>
        /// <param name="realmAccountPrivateKey"></param>
        /// <param name="isEnoki"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> UpdateNfts(string updateRequestJson, string realmAccountPrivateKey, bool isEnoki, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "updateNft",
                new object[] { updateRequestJson, realmAccountPrivateKey, isEnoki, environment }) ?? "";
        }

        /// <summary>
        /// UpdateNfts
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmAccountPrivateKey"></param>
        /// <param name="isEnoki"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> AttachNft(string requestJson, string realmAccountPrivateKey, bool isEnoki, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "attachNft",
                new object[] { requestJson, realmAccountPrivateKey, isEnoki, environment }) ?? "";
        }

        /// <summary>
        /// DetachNft
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmAccountPrivateKey"></param>
        /// <param name="isEnoki"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> DetachNft(string requestJson, string realmAccountPrivateKey, bool isEnoki, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "detachNft",
                new object[] { requestJson, realmAccountPrivateKey, isEnoki, environment }) ?? "";
        }

        /// <summary>
        /// NftAttachmentUpdate
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmAccountPrivateKey"></param>
        /// <param name="isEnoki"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> NftAttachmentUpdate(string requestJson, string realmAccountPrivateKey, bool isEnoki, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "updateNftAttachment",
                new object[] { requestJson, realmAccountPrivateKey, isEnoki, environment }) ?? "";
        }

        /// <summary>
        /// DeleteNfts
        /// </summary>
        /// <param name="deleteRequestJson"></param>
        /// <param name="realmAccountPrivateKey"></param>
        /// <param name="environment"></param>
        /// <param name="isEnoki"></param>
        /// <returns></returns>
        public static async Task<string> DeleteNfts(string deleteRequestJson, string realmAccountPrivateKey, bool isEnoki, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "burnNft",
                new object[] { deleteRequestJson, realmAccountPrivateKey, isEnoki, environment }) ?? "";
        }

        /// <summary>
        /// SetNftContractOwner
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmAccountPrivateKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> SetNftContractOwner(string requestJson, string realmAccountPrivateKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "setNftContractOwner",
                new object[] { requestJson, realmAccountPrivateKey, environment }) ?? "";
        }

        /// <summary>
        /// ObjectExists
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> ObjectExists(string objectId, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "objectExists",
                new object[] { objectId, environment }) ?? "";
        }

        /// <summary>
        /// WithdrawCurrency
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmAccountPrivateKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> WithdrawCurrency(string requestJson, string realmAccountPrivateKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "withdrawCurrency",
                new object[] { requestJson, realmAccountPrivateKey, environment }) ?? "";
        }

        /// <summary>
        /// ListForSale
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> ListForSale(string requestJson, string realmKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "kioskListForSale",
                new object[] { requestJson, realmKey, environment }) ?? "";
        }

        /// <summary>
        /// DelistFromSale
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> DelistFromSale(string requestJson, string realmKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "kioskDelistFromSale",
                new object[] { requestJson, realmKey, environment }) ?? "";
        }

        /// <summary>
        /// KioskPurchase
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> KioskPurchase(string requestJson, string realmKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "kioskPurchase",
                new object[] { requestJson, realmKey, environment }) ?? "";
        }

        /// <summary>
        /// TransferSui
        /// </summary>
        /// <param name="to"></param>
        /// <param name="amount"></param>
        /// <param name="realmKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> TransferSui(string to, long amount, string realmKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "transferSui",
                new object[] { to, amount, realmKey, environment });
        }

        /// <summary>
        /// PersonalKioskCreate
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> PersonalKioskCreate(string requestJson, string realmKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "personalKioskCreate",
                new object[] { requestJson, realmKey, environment }) ?? "";
        }

        /// <summary>
        /// PersonalKioskList
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> PersonalKioskList(string requestJson, string realmKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "personalKioskList",
                new object[] { requestJson, realmKey, environment }) ?? "";
        }

        /// <summary>
        /// PersonalKioskDelist
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> PersonalKioskDelist(string requestJson, string realmKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "personalKioskDelist",
                new object[] { requestJson, realmKey, environment }) ?? "";
        }

        /// <summary>
        /// PersonalKioskTake
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> PersonalKioskTake(string requestJson, string realmKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "personalKioskTake",
                new object[] { requestJson, realmKey, environment }) ?? "";
        }

        /// <summary>
        /// PersonalKioskDeclinePurchase
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> PersonalKioskDeclinePurchase(string requestJson, string realmKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "personalKioskDeclinePurchase",
                new object[] { requestJson, realmKey, environment }) ?? "";
        }

        /// <summary>
        /// PersonalKioskCancelExclusive
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> PersonalKioskCancelExclusive(string requestJson, string realmKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "personalKioskCancelExclusive",
                new object[] { requestJson, realmKey, environment }) ?? "";
        }

        /// <summary>
        /// PersonalKioskPurchase
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> PersonalKioskPurchase(string requestJson, string realmKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "personalKioskPurchase",
                new object[] { requestJson, realmKey, environment }) ?? "";
        }

        /// <summary>
        /// PersonalKioskWithdraw
        /// </summary>
        /// <param name="requestJson"></param>
        /// <param name="realmKey"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static async Task<string> PersonalKioskWithdraw(string requestJson, string realmKey, string environment)
        {
            return await StaticNodeJSService.InvokeFromFileAsync<string>(
                BridgeModulePath,
                "personalKioskWithdraw",
                new object[] { requestJson, realmKey, environment }) ?? "";
        }
    }
}