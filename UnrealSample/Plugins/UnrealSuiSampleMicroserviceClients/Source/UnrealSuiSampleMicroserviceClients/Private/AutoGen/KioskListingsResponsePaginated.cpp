
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListingsResponsePaginated.h"
#include "Serialization/BeamJsonUtils.h"
#include "Misc/DefaultValueHelper.h"


void UKioskListingsResponsePaginated::DeserializeRequestResponse(UObject* RequestData, FString ResponseContent)
{
	OuterOwner = RequestData;
	BeamDeserialize(ResponseContent);	
}

void UKioskListingsResponsePaginated::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("page"), Page, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("pageSize"), PageSize, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("totalCount"), TotalCount, Serializer);
	UBeamJsonUtils::SerializeArray<UKioskListing*>(TEXT("listings"), Listings, Serializer);
}

void UKioskListingsResponsePaginated::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("page"), Page, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("pageSize"), PageSize, Serializer);
	UBeamJsonUtils::SerializeRawPrimitive(TEXT("totalCount"), TotalCount, Serializer);
	UBeamJsonUtils::SerializeArray<UKioskListing*>(TEXT("listings"), Listings, Serializer);		
}

void UKioskListingsResponsePaginated::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("page")), Page);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("pageSize")), PageSize);
	UBeamJsonUtils::DeserializeRawPrimitive(Bag->GetStringField(TEXT("totalCount")), TotalCount);
	UBeamJsonUtils::DeserializeArray<UKioskListing*>(Bag->GetArrayField(TEXT("listings")), Listings, OuterOwner);
}



