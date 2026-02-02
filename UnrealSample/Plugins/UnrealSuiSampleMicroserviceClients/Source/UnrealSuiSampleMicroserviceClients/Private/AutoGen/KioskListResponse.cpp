
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListResponse.h"
#include "Serialization/BeamJsonUtils.h"



void UKioskListResponse::DeserializeRequestResponse(UObject* RequestData, FString ResponseContent)
{
	OuterOwner = RequestData;
	BeamDeserialize(ResponseContent);	
}

void UKioskListResponse::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeArray<UKioskResponse*>(TEXT("kiosks"), Kiosks, Serializer);
}

void UKioskListResponse::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeArray<UKioskResponse*>(TEXT("kiosks"), Kiosks, Serializer);		
}

void UKioskListResponse::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeArray<UKioskResponse*>(Bag->GetArrayField(TEXT("kiosks")), Kiosks, OuterOwner);
}



