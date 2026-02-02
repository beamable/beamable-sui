
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskPurchaseResponse.h"




void UKioskPurchaseResponse::DeserializeRequestResponse(UObject* RequestData, FString ResponseContent)
{
	OuterOwner = RequestData;
	BeamDeserialize(ResponseContent);	
}

void UKioskPurchaseResponse::BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const
{
	
}

void UKioskPurchaseResponse::BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const
{
			
}

void UKioskPurchaseResponse::BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag)
{
	
}



