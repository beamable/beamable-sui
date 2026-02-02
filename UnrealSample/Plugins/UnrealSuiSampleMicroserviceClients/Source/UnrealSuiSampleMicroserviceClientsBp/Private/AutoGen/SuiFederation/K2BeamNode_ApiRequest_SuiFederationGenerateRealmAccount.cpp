

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/SuiFederation/K2BeamNode_ApiRequest_SuiFederationGenerateRealmAccount.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamSuiFederationApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationGenerateRealmAccountRequest.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationGenerateRealmAccountResponse.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_SuiFederationGenerateRealmAccount"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_SuiFederationGenerateRealmAccount::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_SuiFederationGenerateRealmAccount::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GenerateRealmAccount);
}

FName UK2BeamNode_ApiRequest_SuiFederationGenerateRealmAccount::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(USuiFederationGenerateRealmAccountRequest, Make);
}

FString UK2BeamNode_ApiRequest_SuiFederationGenerateRealmAccount::GetServiceName() const
{
	return TEXT("SuiFederation");
}

FString UK2BeamNode_ApiRequest_SuiFederationGenerateRealmAccount::GetEndpointName() const
{
	return TEXT("GenerateRealmAccount");
}

UClass* UK2BeamNode_ApiRequest_SuiFederationGenerateRealmAccount::GetApiClass() const
{
	return UBeamSuiFederationApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationGenerateRealmAccount::GetRequestClass() const
{
	return USuiFederationGenerateRealmAccountRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationGenerateRealmAccount::GetResponseClass() const
{
	return USuiFederationGenerateRealmAccountResponse::StaticClass();
}

FString UK2BeamNode_ApiRequest_SuiFederationGenerateRealmAccount::GetRequestSuccessDelegateName() const
{
	return TEXT("OnSuiFederationGenerateRealmAccountSuccess");
}

FString UK2BeamNode_ApiRequest_SuiFederationGenerateRealmAccount::GetRequestErrorDelegateName() const
{
	return TEXT("OnSuiFederationGenerateRealmAccountError");
}

FString UK2BeamNode_ApiRequest_SuiFederationGenerateRealmAccount::GetRequestCompleteDelegateName() const
{
	return TEXT("OnSuiFederationGenerateRealmAccountComplete");
}

#undef LOCTEXT_NAMESPACE
