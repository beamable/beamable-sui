using System;
using Beamable.Common.Content;
using SuiFederationCommon.Extensions;
using SuiFederationCommon.Models;

namespace SuiFederationCommon.FederationContent
{
    /// <summary>
    /// CreatureDna addon
    /// </summary>
    [ContentType(FederationContentTypes.WeaponLaserModType)]
    public class LaserMod : WeaponItem, INftAddon
    {

    }

    /// <summary>
    /// LaserModRef
    /// </summary>
    [Serializable]
    public class LaserModRef : ContentRef<LaserMod>
    {
    }

    /// <summary>
    /// LaserModLink
    /// </summary>
    [Serializable]
    public class LaserModLink : ContentLink<LaserMod>
    {
    }
}