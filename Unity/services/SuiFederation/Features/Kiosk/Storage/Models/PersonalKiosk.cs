using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Beamable.SuiFederation.Features.Kiosk.Storage.Models;

public record PersonalKiosk(
    [property: BsonId] long GamerTag,
    string Wallet,
    string Network,
    string Namespace,
    DateTime CreatedAt,
    string KioskContentId,
    string KioskId,
    string PersonalKioskCap
);