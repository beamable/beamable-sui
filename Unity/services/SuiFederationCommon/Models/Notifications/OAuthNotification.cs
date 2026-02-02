using System.Runtime.Serialization;

namespace SuiFederationCommon.Models.Notifications
{
    /// <summary>
    /// OAuthNotification
    /// </summary>
    public class OAuthNotification : IPlayerNotification
    {
        /// <summary>
        /// Context for the OAuth notification.
        /// </summary>
        public string Context => PlayerNotificationContext.OauthContext;
        /// <summary>
        /// Unique identifier for the OAuth notification.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        ///  State of the OAuth notification.
        /// </summary>
        public OAuthState State { get; set; }
    }

    /// <summary>
    /// Represents the state of an OAuth notification.
    /// </summary>
    public enum OAuthState
    {
        /// <summary>
        /// Pending state of the OAuth notification.
        /// </summary>
        [EnumMember(Value = "pending")]
        Pending,

        /// <summary>
        /// Authorized state of the OAuth notification.
        /// </summary>
        [EnumMember(Value = "authorized")]
        Authorized,

        /// <summary>
        /// Denied state of the OAuth notification.
        /// </summary>
        [EnumMember(Value = "denied")]
        Denied,

        /// <summary>
        /// Expired state of the OAuth notification.
        /// </summary>
        [EnumMember(Value = "expired")]
        Expired,

        /// <summary>
        /// Revoked state of the OAuth notification.
        /// </summary>
        [EnumMember(Value = "revoked")]
        Revoked
    }
}