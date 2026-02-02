using System.Linq;
using Beamable.Common.Inventory;
using SuiFederationCommon.FederationContent;

namespace SuiFederationCommon.Extensions
{
    /// <summary>
    /// FederationContentExtensions
    /// </summary>
    public static class FederationContentExtensions
    {
        /// <summary>
        /// CoinCurrency module name
        /// </summary>
        /// <param name="coinCurrency"></param>
        /// <returns></returns>
        public static string ToModuleName(this CoinCurrency coinCurrency)
            => SanitizeModuleName(coinCurrency.name).ToLowerInvariant();

        /// <summary>
        /// InGameCurrency module name
        /// </summary>
        /// <param name="coinCurrency"></param>
        /// <returns></returns>
        public static string ToModuleName(this InGameCurrency coinCurrency)
            => SanitizeModuleName(coinCurrency.name).ToLowerInvariant();

        /// <summary>
        /// ItemContent module name
        /// </summary>
        /// <param name="itemContent"></param>
        /// <returns></returns>
        public static string ToModuleName(this ItemContent itemContent)
            => SanitizeModuleName(string.Join("_", itemContent.ContentType.Split('.').Skip(1))).ToLowerInvariant();

        /// <summary>
        /// Get content type part for NFTs
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public static string ToContentType(this string contentId)
            => contentId.Contains('.') ? contentId[..contentId.LastIndexOf('.')] : contentId;

        /// <summary>
        /// KioskItem module name
        /// </summary>
        /// <param name="kioskItem"></param>
        /// <returns></returns>
        public static string ToModuleName(this KioskItem kioskItem)
            => SanitizeModuleName(kioskItem.name).ToLowerInvariant();

        /// <summary>
        /// RegularCoinPrefix
        /// </summary>
        public static string RegularCoinPrefix => $"currency.{FederationContentTypes.RegularCoinType}";

        /// <summary>
        /// NFTPrefix
        /// </summary>
        public static string NftPrefix => "items.";

        /// <summary>
        /// RegularCoinModuleName
        /// </summary>
        public static string SanitizeModuleName(string module)
            => module.Replace("_", "").Replace("-", "").Replace(" ", "");
    }

    /// <summary>
    /// Federation Content Type Names
    /// </summary>
    public static class FederationContentTypes
    {
        /// <summary>
        /// Regular coin type name
        /// </summary>
        public const string RegularCoinType = "coin";

        /// <summary>
        /// In game coin type name
        /// </summary>
        public const string InGameCoinType = "game_coin";

        /// <summary>
        /// Weapon type name
        /// </summary>
        public const string WeaponItemType = "weapon";

        /// <summary>
        /// Creature type name
        /// </summary>
        public const string CreatureItemType = "creature";

        /// <summary>
        /// Creature biome type name
        /// </summary>
        public const string CreatureBiomeType = "biome";

        /// <summary>
        /// Creature Dna type name
        /// </summary>
        public const string CreatureDnaType = "dna";

        /// <summary>
        /// WeaponLaserModType name
        /// </summary>
        public const string WeaponLaserModType = "lasermoditem";

        /// <summary>
        /// Kiosk type name
        /// </summary>
        public const string KioskType = "kiosk";
    }
}