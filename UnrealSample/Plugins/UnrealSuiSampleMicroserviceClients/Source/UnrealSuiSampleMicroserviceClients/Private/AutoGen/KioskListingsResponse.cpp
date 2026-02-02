
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListingsResponse.h"
#include "Serialization/BeamJsonUtils.h"



void UKioskListingsResponse::DeserializeRequestResponse(UObject* RequestData, FString ResponseContent)
{
	OuterOwner = RequestData;
	BeamDeserialize(ResponseContent);	
}

void UKioskListingsResponse::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeArray<UKioskListing*>(TEXT("listings"), Listings, Serializer);
}

void UKioskListingsResponse::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeArray<UKioskListing*>(TEXT("listings"), Listings, Serializer);		
}

void UKioskListingsResponse::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeArray<UKioskListing*>(Bag->GetArrayField(TEXT("listings")), Listings, OuterOwner);
}



