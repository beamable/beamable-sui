using System;
using Beamable.Common.Content;
using SuiFederationCommon.Extensions;
using SuiFederationCommon.Models;

namespace SuiFederationCommon.FederationContent
{
    /// <summary>
    /// CreatureBiome addon
    /// </summary>
    [ContentType(FederationContentTypes.CreatureBiomeType)]
    public class CreatureBiome : CreatureItem, INftAddon
    {

    }

    /// <summary>
    /// CreatureItemRef
    /// </summary>
    [Serializable]
    public class CreatureBiomeRef : ContentRef<CreatureBiome>
    {
    }

    /// <summary>
    /// CreatureItemLink
    /// </summary>
    [Serializable]
    public class CreatureBiomeLink : ContentLink<CreatureBiome>
    {
    }
}