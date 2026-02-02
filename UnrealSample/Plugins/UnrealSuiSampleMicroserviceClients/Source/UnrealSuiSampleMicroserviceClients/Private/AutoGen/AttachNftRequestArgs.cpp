
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/AttachNftRequestArgs.h"

#include "Misc/DefaultValueHelper.h"



void UAttachNftRequestArgs::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("parentId"), ParentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("parentsInstanceId"), ParentsInstanceId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("nftId"), NftId, Serializer);
}

void UAttachNftRequestArgs::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("parentId"), ParentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("parentsInstanceId"), ParentsInstanceId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("nftId"), NftId, Serializer);		
}

void UAttachNftRequestArgs::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("parentId")), ParentId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("parentsInstanceId")), ParentsInstanceId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("nftId")), NftId);
}



