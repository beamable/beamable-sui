
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/ImportRealmAccountRequestArgs.h"





void UImportRealmAccountRequestArgs::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("privateKey"), PrivateKey, Serializer);
}

void UImportRealmAccountRequestArgs::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("privateKey"), PrivateKey, Serializer);		
}

void UImportRealmAccountRequestArgs::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("privateKey")), PrivateKey);
}



