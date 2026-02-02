
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListing.h"
#include "Serialization/BeamJsonUtils.h"
#include "Misc/DefaultValueHelper.h"



void UKioskListing::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("listingId"), ListingId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("kioskContentId"), KioskContentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("kioskName"), KioskName, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("itemContentId"), ItemContentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("itemProxyId"), ItemProxyId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("itemInventoryId"), ItemInventoryId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("priceContentId"), PriceContentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("price"), Price, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("gamerTag"), GamerTag, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("wallet"), Wallet, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("createdAt"), CreatedAt, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("status"), Status, Serializer);
	UBeamJsonUtils::SerializeArray<UItemProperties*>(TEXT("properties"), Properties, Serializer);
}

void UKioskListing::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("listingId"), ListingId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("kioskContentId"), KioskContentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("kioskName"), KioskName, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("itemContentId"), ItemContentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("itemProxyId"), ItemProxyId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("itemInventoryId"), ItemInventoryId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("priceContentId"), PriceContentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("price"), Price, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("gamerTag"), GamerTag, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("wallet"), Wallet, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("createdAt"), CreatedAt, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("status"), Status, Serializer);
	UBeamJsonUtils::SerializeArray<UItemProperties*>(TEXT("properties"), Properties, Serializer);		
}

void UKioskListing::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("listingId")), ListingId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("kioskContentId")), KioskContentId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("kioskName")), KioskName);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("itemContentId")), ItemContentId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("itemProxyId")), ItemProxyId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("itemInventoryId")), ItemInventoryId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("priceContentId")), PriceContentId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("price")), Price);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("gamerTag")), GamerTag);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("wallet")), Wallet);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("createdAt")), CreatedAt);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("status")), Status);
	UBeamJsonUtils::DeserializeArray<UItemProperties*>(Bag->GetArrayField(TEXT("properties")), Properties, OuterOwner);
}



