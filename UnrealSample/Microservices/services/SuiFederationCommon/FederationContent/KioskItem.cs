using System;
using Beamable.Common.Content;
using SuiFederationCommon.Extensions;
using UnityEngine;

namespace SuiFederationCommon.FederationContent
{
    /// <summary>
    /// KioskItem
    /// </summary>
    [ContentType(FederationContentTypes.KioskType)]
    public class KioskItem : ContentObject
    {
        [SerializeField] private string kioskName = "";
        [SerializeField] private string itemType = "";
        [SerializeField] private string currencySymbol = "";

        /// <summary>
        /// Kiosk Display name
        /// </summary>
        public string KioskName => kioskName;
        /// <summary>
        /// Token name
        /// </summary>
        public string ItemType => itemType;

        /// <summary>
        /// CurrencySymbol
        /// </summary>
        public string CurrencySymbol => currencySymbol;
    }
    
    /// <summary>
    /// KioskItemRef
    /// </summary>
    [Serializable]
    public class KioskItemRef : ContentRef<KioskItem>
    {
    }

    /// <summary>
    /// KioskItemLink
    /// </summary>
    [Serializable]
    public class KioskItemLink : ContentLink<KioskItem>
    {
    }
}