
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/GetFederationInfoResponse.h"
#include "Serialization/BeamJsonUtils.h"
#include "Misc/DefaultValueHelper.h"


void UGetFederationInfoResponse::DeserializeRequestResponse(UObject* RequestData, FString ResponseContent)
{
	OuterOwner = RequestData;
	BeamDeserialize(ResponseContent);	
}

void UGetFederationInfoResponse::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("gamerTag"), GamerTag, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("email"), Email, Serializer);
	UBeamJsonUtils::SerializeArray<UPlayerIdentity*>(TEXT("externalIdentities"), ExternalIdentities, Serializer);
}

void UGetFederationInfoResponse::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("gamerTag"), GamerTag, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("email"), Email, Serializer);
	UBeamJsonUtils::SerializeArray<UPlayerIdentity*>(TEXT("externalIdentities"), ExternalIdentities, Serializer);		
}

void UGetFederationInfoResponse::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("gamerTag")), GamerTag);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("email")), Email);
	UBeamJsonUtils::DeserializeArray<UPlayerIdentity*>(Bag->GetArrayField(TEXT("externalIdentities")), ExternalIdentities, OuterOwner);
}



