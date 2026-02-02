namespace SuiFederationCommon.Models.Notifications
{
    /// <summary>
    /// InventoryTransactionNotification
    /// </summary>
    public class InventoryTransactionNotification : IPlayerNotification
    {
        /// <summary>
        /// Context
        /// </summary>
        public string Context { get; } = PlayerNotificationContext.InventoryTransaction;
        /// <summary>
        /// Value of the transaction.
        /// </summary>
        public string Id { get; set; }
    }
}