using System;
using System.Collections.Generic;

namespace SuiFederationCommon.Models.Kiosk
{
    /// <summary>
    /// KioshListingStatus
    /// </summary>
    public enum KioskListingStatus
    {
        /// <summary>
        /// Listing hasn't been sold yet
        /// </summary>
        Active,
        /// <summary>
        /// Other users have purchased the listing
        /// </summary>
        Sold
    }
    
    /// <summary>
    /// KioskListResponse
    /// </summary>
    [Serializable]
    public class KioskListResponse
    {
        /// <summary>
        /// List of kiosks
        /// </summary>
        public List<KioskResponse> kiosks;
    }

    /// <summary>
    /// KioskListResponse
    /// </summary>
    [Serializable]
    public class KioskResponse
    {
        /// <summary>
        /// contentId of the kiosk listing
        /// </summary>
        public string kioskContentId;
        /// <summary>
        /// content type of the item being sold
        /// </summary>
        public string itemContentType;
        /// <summary>
        /// contentId of the price being used
        /// </summary>
        public string priceContentId;
    }
}