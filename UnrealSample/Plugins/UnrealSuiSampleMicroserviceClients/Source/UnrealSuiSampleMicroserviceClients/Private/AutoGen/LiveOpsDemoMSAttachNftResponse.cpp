
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/LiveOpsDemoMSAttachNftResponse.h"
#include "Serialization/BeamJsonUtils.h"



void ULiveOpsDemoMSAttachNftResponse::DeserializeRequestResponse(UObject* RequestData, FString ResponseContent)
{
	OuterOwner = RequestData;
	UBeamJsonUtils::DeserializeRawPrimitive<bool>(ResponseContent, bValue, OuterOwner);
}

void ULiveOpsDemoMSAttachNftResponse::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("Value"), bValue, Serializer);
}

void ULiveOpsDemoMSAttachNftResponse::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("Value"), bValue, Serializer);		
}

void ULiveOpsDemoMSAttachNftResponse::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("Value")), bValue);
}



