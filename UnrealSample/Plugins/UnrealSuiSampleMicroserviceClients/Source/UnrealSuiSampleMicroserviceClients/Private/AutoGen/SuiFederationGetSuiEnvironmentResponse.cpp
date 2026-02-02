
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationGetSuiEnvironmentResponse.h"
#include "Serialization/BeamJsonUtils.h"



void USuiFederationGetSuiEnvironmentResponse::DeserializeRequestResponse(UObject* RequestData, FString ResponseContent)
{
	OuterOwner = RequestData;
	UBeamJsonUtils::DeserializeRawPrimitive<FString>(ResponseContent, Value, OuterOwner);
}

void USuiFederationGetSuiEnvironmentResponse::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("Value"), Value, Serializer);
}

void USuiFederationGetSuiEnvironmentResponse::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("Value"), Value, Serializer);		
}

void USuiFederationGetSuiEnvironmentResponse::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("Value")), Value);
}



