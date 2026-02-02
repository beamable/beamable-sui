
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/WithdrawRequestArgs.h"

#include "Misc/DefaultValueHelper.h"



void UWithdrawRequestArgs::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("contentId"), ContentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("amount"), Amount, Serializer);
}

void UWithdrawRequestArgs::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("contentId"), ContentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("amount"), Amount, Serializer);		
}

void UWithdrawRequestArgs::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("contentId")), ContentId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("amount")), Amount);
}



