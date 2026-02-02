
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationImportAccountResponse.h"
#include "Serialization/BeamJsonUtils.h"



void USuiFederationImportAccountResponse::DeserializeRequestResponse(UObject* RequestData, FString ResponseContent)
{
	OuterOwner = RequestData;
	UBeamJsonUtils::DeserializeRawPrimitive<FString>(ResponseContent, Value, OuterOwner);
}

void USuiFederationImportAccountResponse::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("Value"), Value, Serializer);
}

void USuiFederationImportAccountResponse::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("Value"), Value, Serializer);		
}

void USuiFederationImportAccountResponse::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("Value")), Value);
}



