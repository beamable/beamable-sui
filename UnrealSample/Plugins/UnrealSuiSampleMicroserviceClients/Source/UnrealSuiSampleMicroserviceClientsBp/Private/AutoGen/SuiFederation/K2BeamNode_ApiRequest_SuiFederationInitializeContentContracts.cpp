

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/SuiFederation/K2BeamNode_ApiRequest_SuiFederationInitializeContentContracts.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamSuiFederationApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationInitializeContentContractsRequest.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationInitializeContentContractsResponse.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_SuiFederationInitializeContentContracts"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_SuiFederationInitializeContentContracts::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_SuiFederationInitializeContentContracts::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, InitializeContentContracts);
}

FName UK2BeamNode_ApiRequest_SuiFederationInitializeContentContracts::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(USuiFederationInitializeContentContractsRequest, Make);
}

FString UK2BeamNode_ApiRequest_SuiFederationInitializeContentContracts::GetServiceName() const
{
	return TEXT("SuiFederation");
}

FString UK2BeamNode_ApiRequest_SuiFederationInitializeContentContracts::GetEndpointName() const
{
	return TEXT("InitializeContentContracts");
}

UClass* UK2BeamNode_ApiRequest_SuiFederationInitializeContentContracts::GetApiClass() const
{
	return UBeamSuiFederationApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationInitializeContentContracts::GetRequestClass() const
{
	return USuiFederationInitializeContentContractsRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationInitializeContentContracts::GetResponseClass() const
{
	return USuiFederationInitializeContentContractsResponse::StaticClass();
}

FString UK2BeamNode_ApiRequest_SuiFederationInitializeContentContracts::GetRequestSuccessDelegateName() const
{
	return TEXT("OnSuiFederationInitializeContentContractsSuccess");
}

FString UK2BeamNode_ApiRequest_SuiFederationInitializeContentContracts::GetRequestErrorDelegateName() const
{
	return TEXT("OnSuiFederationInitializeContentContractsError");
}

FString UK2BeamNode_ApiRequest_SuiFederationInitializeContentContracts::GetRequestCompleteDelegateName() const
{
	return TEXT("OnSuiFederationInitializeContentContractsComplete");
}

#undef LOCTEXT_NAMESPACE
