

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/SuiFederation/K2BeamNode_ApiRequest_SuiFederationWithdraw.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamSuiFederationApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationWithdrawRequest.h"
#include "Serialization/BeamPlainTextResponseBody.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_SuiFederationWithdraw"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_SuiFederationWithdraw::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_SuiFederationWithdraw::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, Withdraw);
}

FName UK2BeamNode_ApiRequest_SuiFederationWithdraw::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(USuiFederationWithdrawRequest, Make);
}

FString UK2BeamNode_ApiRequest_SuiFederationWithdraw::GetServiceName() const
{
	return TEXT("SuiFederation");
}

FString UK2BeamNode_ApiRequest_SuiFederationWithdraw::GetEndpointName() const
{
	return TEXT("Withdraw");
}

UClass* UK2BeamNode_ApiRequest_SuiFederationWithdraw::GetApiClass() const
{
	return UBeamSuiFederationApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationWithdraw::GetRequestClass() const
{
	return USuiFederationWithdrawRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationWithdraw::GetResponseClass() const
{
	return UBeamPlainTextResponseBody::StaticClass();
}

FString UK2BeamNode_ApiRequest_SuiFederationWithdraw::GetRequestSuccessDelegateName() const
{
	return TEXT("OnSuiFederationWithdrawSuccess");
}

FString UK2BeamNode_ApiRequest_SuiFederationWithdraw::GetRequestErrorDelegateName() const
{
	return TEXT("OnSuiFederationWithdrawError");
}

FString UK2BeamNode_ApiRequest_SuiFederationWithdraw::GetRequestCompleteDelegateName() const
{
	return TEXT("OnSuiFederationWithdrawComplete");
}

#undef LOCTEXT_NAMESPACE
