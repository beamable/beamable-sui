
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskResponse.h"





void UKioskResponse::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("kioskContentId"), KioskContentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("itemContentType"), ItemContentType, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("priceContentId"), PriceContentId, Serializer);
}

void UKioskResponse::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("kioskContentId"), KioskContentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("itemContentType"), ItemContentType, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("priceContentId"), PriceContentId, Serializer);		
}

void UKioskResponse::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("kioskContentId")), KioskContentId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("itemContentType")), ItemContentType);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("priceContentId")), PriceContentId);
}



