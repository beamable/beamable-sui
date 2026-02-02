
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskPurchaseRequestArgs.h"





void UKioskPurchaseRequestArgs::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("listingId"), ListingId, Serializer);
}

void UKioskPurchaseRequestArgs::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("listingId"), ListingId, Serializer);		
}

void UKioskPurchaseRequestArgs::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("listingId")), ListingId);
}



