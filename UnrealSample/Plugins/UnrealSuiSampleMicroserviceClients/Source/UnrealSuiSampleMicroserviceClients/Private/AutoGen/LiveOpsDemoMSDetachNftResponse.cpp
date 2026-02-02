
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/LiveOpsDemoMSDetachNftResponse.h"
#include "Serialization/BeamJsonUtils.h"



void ULiveOpsDemoMSDetachNftResponse::DeserializeRequestResponse(UObject* RequestData, FString ResponseContent)
{
	OuterOwner = RequestData;
	UBeamJsonUtils::DeserializeRawPrimitive<bool>(ResponseContent, bValue, OuterOwner);
}

void ULiveOpsDemoMSDetachNftResponse::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("Value"), bValue, Serializer);
}

void ULiveOpsDemoMSDetachNftResponse::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("Value"), bValue, Serializer);		
}

void ULiveOpsDemoMSDetachNftResponse::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("Value")), bValue);
}



