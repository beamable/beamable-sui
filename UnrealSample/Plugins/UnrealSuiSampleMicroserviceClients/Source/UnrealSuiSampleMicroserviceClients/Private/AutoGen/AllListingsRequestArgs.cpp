
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/AllListingsRequestArgs.h"

#include "Misc/DefaultValueHelper.h"



void UAllListingsRequestArgs::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("optionalKioskContentId"), OptionalKioskContentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("page"), Page, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("pageSize"), PageSize, Serializer);
}

void UAllListingsRequestArgs::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("optionalKioskContentId"), OptionalKioskContentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("page"), Page, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("pageSize"), PageSize, Serializer);		
}

void UAllListingsRequestArgs::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("optionalKioskContentId")), OptionalKioskContentId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("page")), Page);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("pageSize")), PageSize);
}



