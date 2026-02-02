

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/SuiFederation/K2BeamNode_ApiRequest_SuiFederationGetRealmAccount.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamSuiFederationApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationGetRealmAccountRequest.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationGetRealmAccountResponse.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_SuiFederationGetRealmAccount"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_SuiFederationGetRealmAccount::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_SuiFederationGetRealmAccount::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetRealmAccount);
}

FName UK2BeamNode_ApiRequest_SuiFederationGetRealmAccount::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(USuiFederationGetRealmAccountRequest, Make);
}

FString UK2BeamNode_ApiRequest_SuiFederationGetRealmAccount::GetServiceName() const
{
	return TEXT("SuiFederation");
}

FString UK2BeamNode_ApiRequest_SuiFederationGetRealmAccount::GetEndpointName() const
{
	return TEXT("GetRealmAccount");
}

UClass* UK2BeamNode_ApiRequest_SuiFederationGetRealmAccount::GetApiClass() const
{
	return UBeamSuiFederationApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationGetRealmAccount::GetRequestClass() const
{
	return USuiFederationGetRealmAccountRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationGetRealmAccount::GetResponseClass() const
{
	return USuiFederationGetRealmAccountResponse::StaticClass();
}

FString UK2BeamNode_ApiRequest_SuiFederationGetRealmAccount::GetRequestSuccessDelegateName() const
{
	return TEXT("OnSuiFederationGetRealmAccountSuccess");
}

FString UK2BeamNode_ApiRequest_SuiFederationGetRealmAccount::GetRequestErrorDelegateName() const
{
	return TEXT("OnSuiFederationGetRealmAccountError");
}

FString UK2BeamNode_ApiRequest_SuiFederationGetRealmAccount::GetRequestCompleteDelegateName() const
{
	return TEXT("OnSuiFederationGetRealmAccountComplete");
}

#undef LOCTEXT_NAMESPACE
