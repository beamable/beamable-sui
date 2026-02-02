

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/SuiFederation/K2BeamNode_ApiRequest_SuiFederationImportAccount.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamSuiFederationApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationImportAccountRequest.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationImportAccountResponse.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_SuiFederationImportAccount"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_SuiFederationImportAccount::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_SuiFederationImportAccount::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, ImportAccount);
}

FName UK2BeamNode_ApiRequest_SuiFederationImportAccount::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(USuiFederationImportAccountRequest, Make);
}

FString UK2BeamNode_ApiRequest_SuiFederationImportAccount::GetServiceName() const
{
	return TEXT("SuiFederation");
}

FString UK2BeamNode_ApiRequest_SuiFederationImportAccount::GetEndpointName() const
{
	return TEXT("ImportAccount");
}

UClass* UK2BeamNode_ApiRequest_SuiFederationImportAccount::GetApiClass() const
{
	return UBeamSuiFederationApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationImportAccount::GetRequestClass() const
{
	return USuiFederationImportAccountRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationImportAccount::GetResponseClass() const
{
	return USuiFederationImportAccountResponse::StaticClass();
}

FString UK2BeamNode_ApiRequest_SuiFederationImportAccount::GetRequestSuccessDelegateName() const
{
	return TEXT("OnSuiFederationImportAccountSuccess");
}

FString UK2BeamNode_ApiRequest_SuiFederationImportAccount::GetRequestErrorDelegateName() const
{
	return TEXT("OnSuiFederationImportAccountError");
}

FString UK2BeamNode_ApiRequest_SuiFederationImportAccount::GetRequestCompleteDelegateName() const
{
	return TEXT("OnSuiFederationImportAccountComplete");
}

#undef LOCTEXT_NAMESPACE
