

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/SuiFederation/K2BeamNode_ApiRequest_SuiFederationOwnedListings.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamSuiFederationApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationOwnedListingsRequest.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListingsResponse.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_SuiFederationOwnedListings"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_SuiFederationOwnedListings::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_SuiFederationOwnedListings::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, OwnedListings);
}

FName UK2BeamNode_ApiRequest_SuiFederationOwnedListings::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(USuiFederationOwnedListingsRequest, Make);
}

FString UK2BeamNode_ApiRequest_SuiFederationOwnedListings::GetServiceName() const
{
	return TEXT("SuiFederation");
}

FString UK2BeamNode_ApiRequest_SuiFederationOwnedListings::GetEndpointName() const
{
	return TEXT("OwnedListings");
}

UClass* UK2BeamNode_ApiRequest_SuiFederationOwnedListings::GetApiClass() const
{
	return UBeamSuiFederationApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationOwnedListings::GetRequestClass() const
{
	return USuiFederationOwnedListingsRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationOwnedListings::GetResponseClass() const
{
	return UKioskListingsResponse::StaticClass();
}

FString UK2BeamNode_ApiRequest_SuiFederationOwnedListings::GetRequestSuccessDelegateName() const
{
	return TEXT("OnSuiFederationOwnedListingsSuccess");
}

FString UK2BeamNode_ApiRequest_SuiFederationOwnedListings::GetRequestErrorDelegateName() const
{
	return TEXT("OnSuiFederationOwnedListingsError");
}

FString UK2BeamNode_ApiRequest_SuiFederationOwnedListings::GetRequestCompleteDelegateName() const
{
	return TEXT("OnSuiFederationOwnedListingsComplete");
}

#undef LOCTEXT_NAMESPACE
