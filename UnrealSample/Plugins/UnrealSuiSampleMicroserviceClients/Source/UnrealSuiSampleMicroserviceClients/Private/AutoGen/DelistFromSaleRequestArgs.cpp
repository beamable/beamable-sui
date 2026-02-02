
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/DelistFromSaleRequestArgs.h"





void UDelistFromSaleRequestArgs::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("listingId"), ListingId, Serializer);
}

void UDelistFromSaleRequestArgs::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("listingId"), ListingId, Serializer);		
}

void UDelistFromSaleRequestArgs::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("listingId")), ListingId);
}



