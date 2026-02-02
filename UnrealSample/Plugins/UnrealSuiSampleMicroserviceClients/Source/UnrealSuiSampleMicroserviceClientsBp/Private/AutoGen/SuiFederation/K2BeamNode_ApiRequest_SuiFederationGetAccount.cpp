

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/SuiFederation/K2BeamNode_ApiRequest_SuiFederationGetAccount.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamSuiFederationApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationGetAccountRequest.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationGetAccountResponse.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_SuiFederationGetAccount"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_SuiFederationGetAccount::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_SuiFederationGetAccount::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetAccount);
}

FName UK2BeamNode_ApiRequest_SuiFederationGetAccount::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(USuiFederationGetAccountRequest, Make);
}

FString UK2BeamNode_ApiRequest_SuiFederationGetAccount::GetServiceName() const
{
	return TEXT("SuiFederation");
}

FString UK2BeamNode_ApiRequest_SuiFederationGetAccount::GetEndpointName() const
{
	return TEXT("GetAccount");
}

UClass* UK2BeamNode_ApiRequest_SuiFederationGetAccount::GetApiClass() const
{
	return UBeamSuiFederationApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationGetAccount::GetRequestClass() const
{
	return USuiFederationGetAccountRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationGetAccount::GetResponseClass() const
{
	return USuiFederationGetAccountResponse::StaticClass();
}

FString UK2BeamNode_ApiRequest_SuiFederationGetAccount::GetRequestSuccessDelegateName() const
{
	return TEXT("OnSuiFederationGetAccountSuccess");
}

FString UK2BeamNode_ApiRequest_SuiFederationGetAccount::GetRequestErrorDelegateName() const
{
	return TEXT("OnSuiFederationGetAccountError");
}

FString UK2BeamNode_ApiRequest_SuiFederationGetAccount::GetRequestCompleteDelegateName() const
{
	return TEXT("OnSuiFederationGetAccountComplete");
}

#undef LOCTEXT_NAMESPACE
