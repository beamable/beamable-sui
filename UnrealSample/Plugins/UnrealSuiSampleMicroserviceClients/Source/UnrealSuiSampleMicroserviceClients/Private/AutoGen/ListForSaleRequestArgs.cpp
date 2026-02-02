
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/ListForSaleRequestArgs.h"

#include "Misc/DefaultValueHelper.h"



void UListForSaleRequestArgs::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("itemId"), ItemId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("price"), Price, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("optionalKioskContentId"), OptionalKioskContentId, Serializer);
}

void UListForSaleRequestArgs::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("itemId"), ItemId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("price"), Price, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("optionalKioskContentId"), OptionalKioskContentId, Serializer);		
}

void UListForSaleRequestArgs::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("itemId")), ItemId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("price")), Price);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("optionalKioskContentId")), OptionalKioskContentId);
}



