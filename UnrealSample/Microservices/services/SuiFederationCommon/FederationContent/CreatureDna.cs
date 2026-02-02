using System;
using Beamable.Common.Content;
using SuiFederationCommon.Extensions;
using SuiFederationCommon.Models;

namespace SuiFederationCommon.FederationContent
{
    /// <summary>
    /// CreatureDna addon
    /// </summary>
    [ContentType(FederationContentTypes.CreatureDnaType)]
    public class CreatureDna : CreatureItem, INftAddon
    {

    }

    /// <summary>
    /// CreatureItemRef
    /// </summary>
    [Serializable]
    public class CreatureDnaRef : ContentRef<CreatureDna>
    {
    }

    /// <summary>
    /// CreatureItemLink
    /// </summary>
    [Serializable]
    public class CreatureDnaLink : ContentLink<CreatureDna>
    {
    }
}