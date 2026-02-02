

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/SuiFederation/K2BeamNode_ApiRequest_SuiFederationOAuthCallback.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamSuiFederationApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationOAuthCallbackRequest.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/OauthCallbackResponse.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_SuiFederationOAuthCallback"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_SuiFederationOAuthCallback::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_SuiFederationOAuthCallback::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, OAuthCallback);
}

FName UK2BeamNode_ApiRequest_SuiFederationOAuthCallback::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(USuiFederationOAuthCallbackRequest, Make);
}

FString UK2BeamNode_ApiRequest_SuiFederationOAuthCallback::GetServiceName() const
{
	return TEXT("SuiFederation");
}

FString UK2BeamNode_ApiRequest_SuiFederationOAuthCallback::GetEndpointName() const
{
	return TEXT("OAuthCallback");
}

UClass* UK2BeamNode_ApiRequest_SuiFederationOAuthCallback::GetApiClass() const
{
	return UBeamSuiFederationApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationOAuthCallback::GetRequestClass() const
{
	return USuiFederationOAuthCallbackRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationOAuthCallback::GetResponseClass() const
{
	return UOauthCallbackResponse::StaticClass();
}

FString UK2BeamNode_ApiRequest_SuiFederationOAuthCallback::GetRequestSuccessDelegateName() const
{
	return TEXT("OnSuiFederationOAuthCallbackSuccess");
}

FString UK2BeamNode_ApiRequest_SuiFederationOAuthCallback::GetRequestErrorDelegateName() const
{
	return TEXT("OnSuiFederationOAuthCallbackError");
}

FString UK2BeamNode_ApiRequest_SuiFederationOAuthCallback::GetRequestCompleteDelegateName() const
{
	return TEXT("OnSuiFederationOAuthCallbackComplete");
}

#undef LOCTEXT_NAMESPACE
