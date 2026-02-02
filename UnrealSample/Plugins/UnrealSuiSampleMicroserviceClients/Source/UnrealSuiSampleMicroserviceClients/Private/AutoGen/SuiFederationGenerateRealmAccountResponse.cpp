
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationGenerateRealmAccountResponse.h"
#include "Serialization/BeamJsonUtils.h"



void USuiFederationGenerateRealmAccountResponse::DeserializeRequestResponse(UObject* RequestData, FString ResponseContent)
{
	OuterOwner = RequestData;
	UBeamJsonUtils::DeserializeRawPrimitive<FString>(ResponseContent, Value, OuterOwner);
}

void USuiFederationGenerateRealmAccountResponse::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("Value"), Value, Serializer);
}

void USuiFederationGenerateRealmAccountResponse::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("Value"), Value, Serializer);		
}

void USuiFederationGenerateRealmAccountResponse::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("Value")), Value);
}



