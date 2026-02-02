using System;

namespace SuiFederationCommon.Models.Oauth
{
    /// <summary>
    /// OauthCallbackResponse
    /// </summary>
    [Serializable]
    public class OauthCallbackResponse
    {
        /// <summary>
        /// Response message
        /// </summary>
        public string message;

        /// <summary>
        /// OkResult
        /// </summary>
        /// <returns></returns>
        public static OauthCallbackResponse OkResult(string message)
        {
            return new OauthCallbackResponse
            {
                message = message
            };
        }

        /// <summary>
        /// FailedResult
        /// </summary>
        /// <returns></returns>
        public static OauthCallbackResponse FailedResult()
        {
            return new OauthCallbackResponse
            {
                message = "failed"
            };
        }
    }
}