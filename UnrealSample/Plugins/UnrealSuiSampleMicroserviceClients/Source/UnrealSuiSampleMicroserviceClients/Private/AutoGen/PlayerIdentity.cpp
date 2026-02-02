
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/PlayerIdentity.h"





void UPlayerIdentity::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("microservice"), Microservice, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("nameSpace"), NameSpace, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("address"), Address, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("type"), Type, Serializer);
}

void UPlayerIdentity::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("microservice"), Microservice, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("nameSpace"), NameSpace, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("address"), Address, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("type"), Type, Serializer);		
}

void UPlayerIdentity::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("microservice")), Microservice);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("nameSpace")), NameSpace);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("address")), Address);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("type")), Type);
}



