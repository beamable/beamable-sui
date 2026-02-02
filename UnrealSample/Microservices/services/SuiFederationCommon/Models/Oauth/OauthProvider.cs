using System.Collections.Generic;

namespace SuiFederationCommon.Models.Oauth
{
    /// <summary>
    /// OauthProvider Names
    /// </summary>
    public static class OauthProvider
    {
        /// <summary>
        /// Google Oauth Provider
        /// </summary>
        public const string Google = "google";

        /// <summary>
        /// Twitch Oauth Provider
        /// </summary>
        public const string Twitch = "twitch";

        private static readonly HashSet<string> ValidProviders = new HashSet<string>
        {
            Google, Twitch
        };

        /// <summary>
        /// Parses the Oauth Provider from a string value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static bool TryParse(string value, out string provider)
        {
            provider = value;
            return !string.IsNullOrWhiteSpace(value) && ValidProviders.Contains(value);
        }

        /// <summary>
        /// Valid Oauth Providers
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid(string value) => ValidProviders.Contains(value);
    }
}