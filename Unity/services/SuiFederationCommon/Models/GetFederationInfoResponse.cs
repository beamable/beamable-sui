using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace SuiFederationCommon.Models
{
    /// <summary>
    /// Federation information response model.
    /// </summary>
    [Serializable]
    public class GetFederationInfoResponse
    {
        /// <summary>
        /// Unique identifier for the player, typically a gamer tag.
        /// </summary>
        public long gamerTag;
        /// <summary>
        ///  Email address associated with the player account.
        /// </summary>
        public string email;
        /// <summary>
        ///  List of external identities associated with the player.
        /// </summary>
        public List<PlayerIdentity> externalIdentities;
    }

    /// <summary>
    /// Player identity model representing external identities associated with a player.
    /// </summary>
    [Serializable]
    public class PlayerIdentity
    {
        /// <summary>
        /// Microservice name associated with the identity.
        /// </summary>
        public string microservice;
        /// <summary>
        /// Namespace of the identity.
        /// </summary>
        public string nameSpace;
        /// <summary>
        /// Blockchain address associated with the identity.
        /// </summary>
        public string address;
        /// <summary>
        /// Identity type, such as wallet type.
        /// </summary>
        public string type;
    }
}