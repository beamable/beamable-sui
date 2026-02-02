using System;
using System.Collections.Generic;

namespace SuiFederationCommon.Models.Kiosk
{
    /// <summary>
    /// KioskListingsResponsePaginated
    /// </summary>
    [Serializable]
    public class KioskListingsResponsePaginated
    {
        /// <summary>
        ///  Current page
        /// </summary>
        public int page;
        /// <summary>
        ///  Page size
        /// </summary>
        public int pageSize;
        /// <summary>
        ///  Total count of listings
        /// </summary>
        public long totalCount;
        /// <summary>
        /// The list of kiosk listings owned by the user.
        /// </summary>
        public List<KioskListing> listings;
    }
    /// <summary>
    /// KioskListingsResponse
    /// </summary>
    [Serializable]
    public class KioskListingsResponse
    {
        /// <summary>
        /// The list of kiosk listings owned by the user.
        /// </summary>
        public List<KioskListing> listings;
    }

    /// <summary>
    /// KioskListing
    /// </summary>
    [Serializable]
    public class KioskListing
    {
        /// <summary>
        ///  Listing ID
        /// </summary>
        public string listingId;
        /// <summary>
        ///  Kiosk Content ID
        /// </summary>
        public string kioskContentId;
        /// <summary>
        ///  Kiosk Name
        /// </summary>
        public string kioskName;
        /// <summary>
        ///  Item Content ID
        /// </summary>
        public string itemContentId;
        /// <summary>
        /// Item Proxy ID
        /// </summary>
        public string itemProxyId;
        /// <summary>
        /// Item Inventory ID
        /// </summary>
        public long itemInventoryId;
        /// <summary>
        /// Price Content ID
        /// </summary>
        public string priceContentId;
        /// <summary>
        /// Price
        /// </summary>
        public long price;
        /// <summary>
        /// Gamer Tag
        /// </summary>
        public long gamerTag;
        /// <summary>
        /// Wallet
        /// </summary>
        public string wallet;
        /// <summary>
        /// Created At
        /// </summary>
        public DateTime createdAt;
        /// <summary>
        ///  Status
        /// </summary>
        public string status;
        /// <summary>
        ///  Status
        /// </summary>
        public ItemProperties[] properties;
    }

    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ItemProperties
    {
        /// <summary>
        /// name
        /// </summary>
        public string name;
        /// <summary>
        /// value
        /// </summary>
        public string value;
    }
}