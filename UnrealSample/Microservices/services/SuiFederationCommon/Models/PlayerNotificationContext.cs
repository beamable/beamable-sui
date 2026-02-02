namespace SuiFederationCommon.Models
{
    /// <summary>
    /// Represents the context for player notification subscriptions.
    /// </summary>
    public class PlayerNotificationContext
    {
        /// <summary>
        /// Inventory transaction context for player notifications.
        /// </summary>
        public const string InventoryTransaction = "inventory-transaction";

        /// <summary>
        /// Oauth context for player notifications.
        /// </summary>
        public const string OauthContext = "oauth";

        /// <summary>
        /// Kiosk action for player notifications.
        /// </summary>
        public const string KioskContext = "kiosk";
    }
}