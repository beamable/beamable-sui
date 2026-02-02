using System;
using Beamable.Common.Content;
using Beamable.Common.Inventory;
using SuiFederationCommon.Extensions;
using SuiFederationCommon.FederationContent;
using SuiFederationCommon.Models;
using UnityEngine;

namespace SuiFederationCommon.FederationContent
{
    /// <summary>
    /// CreatureItem NFT
    /// </summary>
    [ContentType(FederationContentTypes.CreatureItemType)]
    public class CreatureItem : ItemContent, INftBase
    {
        /// <summary>
        /// Default federation
        /// </summary>
        public CreatureItem()
        {
            federation = new OptionalFederation
            {
                HasValue = true,
                Value = new Federation
                {
                    Service = SuiFederationSettings.MicroserviceName,
                    Namespace = SuiFederationSettings.SuiIdentityName
                }
            };
        }

        [SerializeField] private new string name = "";
        [SerializeField] private string description = "";
        [SerializeField] private string image = "";

        [SerializeField]
        private SerializableDictionaryStringToString customProperties = new SerializableDictionaryStringToString();

        /// <summary>
        /// Token name
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Token description
        /// </summary>
        public string Description => description;

        /// <summary>
        /// Token image
        /// </summary>
        public string Image => image;

        /// <summary>
        /// Token custom properties
        /// </summary>
        public SerializableDictionaryStringToString CustomProperties => customProperties;
    }

    /// <summary>
    /// CreatureItemRef
    /// </summary>
    [Serializable]
    public class CreatureItemRef : ContentRef<CreatureItem>
    {
    }

    /// <summary>
    /// CreatureItemLink
    /// </summary>
    [Serializable]
    public class CreatureItemLink : ContentLink<CreatureItem>
    {
    }
}