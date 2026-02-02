
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/DetachNftRequestArgs.h"

#include "Misc/DefaultValueHelper.h"



void UDetachNftRequestArgs::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("parentId"), ParentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("parentsInstanceId"), ParentsInstanceId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("nftId"), NftId, Serializer);
}

void UDetachNftRequestArgs::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("parentId"), ParentId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("parentsInstanceId"), ParentsInstanceId, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("nftId"), NftId, Serializer);		
}

void UDetachNftRequestArgs::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("parentId")), ParentId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("parentsInstanceId")), ParentsInstanceId);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("nftId")), NftId);
}



