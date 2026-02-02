

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/SuiFederation/K2BeamNode_ApiRequest_SuiFederationAllListings.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamSuiFederationApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationAllListingsRequest.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListingsResponsePaginated.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_SuiFederationAllListings"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_SuiFederationAllListings::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_SuiFederationAllListings::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, AllListings);
}

FName UK2BeamNode_ApiRequest_SuiFederationAllListings::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(USuiFederationAllListingsRequest, Make);
}

FString UK2BeamNode_ApiRequest_SuiFederationAllListings::GetServiceName() const
{
	return TEXT("SuiFederation");
}

FString UK2BeamNode_ApiRequest_SuiFederationAllListings::GetEndpointName() const
{
	return TEXT("AllListings");
}

UClass* UK2BeamNode_ApiRequest_SuiFederationAllListings::GetApiClass() const
{
	return UBeamSuiFederationApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationAllListings::GetRequestClass() const
{
	return USuiFederationAllListingsRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationAllListings::GetResponseClass() const
{
	return UKioskListingsResponsePaginated::StaticClass();
}

FString UK2BeamNode_ApiRequest_SuiFederationAllListings::GetRequestSuccessDelegateName() const
{
	return TEXT("OnSuiFederationAllListingsSuccess");
}

FString UK2BeamNode_ApiRequest_SuiFederationAllListings::GetRequestErrorDelegateName() const
{
	return TEXT("OnSuiFederationAllListingsError");
}

FString UK2BeamNode_ApiRequest_SuiFederationAllListings::GetRequestCompleteDelegateName() const
{
	return TEXT("OnSuiFederationAllListingsComplete");
}

#undef LOCTEXT_NAMESPACE
