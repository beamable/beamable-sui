

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/SuiFederation/K2BeamNode_ApiRequest_SuiFederationImportRealmAccount.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamSuiFederationApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationImportRealmAccountRequest.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationImportRealmAccountResponse.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_SuiFederationImportRealmAccount"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_SuiFederationImportRealmAccount::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_SuiFederationImportRealmAccount::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, ImportRealmAccount);
}

FName UK2BeamNode_ApiRequest_SuiFederationImportRealmAccount::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(USuiFederationImportRealmAccountRequest, Make);
}

FString UK2BeamNode_ApiRequest_SuiFederationImportRealmAccount::GetServiceName() const
{
	return TEXT("SuiFederation");
}

FString UK2BeamNode_ApiRequest_SuiFederationImportRealmAccount::GetEndpointName() const
{
	return TEXT("ImportRealmAccount");
}

UClass* UK2BeamNode_ApiRequest_SuiFederationImportRealmAccount::GetApiClass() const
{
	return UBeamSuiFederationApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationImportRealmAccount::GetRequestClass() const
{
	return USuiFederationImportRealmAccountRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationImportRealmAccount::GetResponseClass() const
{
	return USuiFederationImportRealmAccountResponse::StaticClass();
}

FString UK2BeamNode_ApiRequest_SuiFederationImportRealmAccount::GetRequestSuccessDelegateName() const
{
	return TEXT("OnSuiFederationImportRealmAccountSuccess");
}

FString UK2BeamNode_ApiRequest_SuiFederationImportRealmAccount::GetRequestErrorDelegateName() const
{
	return TEXT("OnSuiFederationImportRealmAccountError");
}

FString UK2BeamNode_ApiRequest_SuiFederationImportRealmAccount::GetRequestCompleteDelegateName() const
{
	return TEXT("OnSuiFederationImportRealmAccountComplete");
}

#undef LOCTEXT_NAMESPACE
