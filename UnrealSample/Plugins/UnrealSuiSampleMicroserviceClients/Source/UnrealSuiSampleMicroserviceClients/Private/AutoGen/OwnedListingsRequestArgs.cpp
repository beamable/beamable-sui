
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/OwnedListingsRequestArgs.h"





void UOwnedListingsRequestArgs::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("optionalKioskContentId"), OptionalKioskContentId, Serializer);
}

void UOwnedListingsRequestArgs::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("optionalKioskContentId"), OptionalKioskContentId, Serializer);		
}

void UOwnedListingsRequestArgs::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("optionalKioskContentId")), OptionalKioskContentId);
}



