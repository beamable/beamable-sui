
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/UpgradeItemRequestArgs.h"

#include "Misc/DefaultValueHelper.h"



void UUpgradeItemRequestArgs::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("contentId"), ContentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("itemInstanceId"), ItemInstanceId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("newLevel"), NewLevel, Serializer);
}

void UUpgradeItemRequestArgs::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("contentId"), ContentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("itemInstanceId"), ItemInstanceId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("newLevel"), NewLevel, Serializer);		
}

void UUpgradeItemRequestArgs::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("contentId")), ContentId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("itemInstanceId")), ItemInstanceId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("newLevel")), NewLevel);
}



