namespace SuiFederationCommon.Models.Notifications
{
    /// <summary>
    /// KioskNotification
    /// </summary>
    public class KioskNotification : IPlayerNotification
    {
        /// <summary>
        /// Context
        /// </summary>
        public string Context { get; } = PlayerNotificationContext.KioskContext;
        /// <summary>
        /// Value of the transaction.
        /// </summary>
        public string Id { get; set; }
    }
}