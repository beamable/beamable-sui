

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/SuiFederation/K2BeamNode_ApiRequest_SuiFederationGetFederationInfo.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamSuiFederationApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationGetFederationInfoRequest.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/GetFederationInfoResponse.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_SuiFederationGetFederationInfo"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_SuiFederationGetFederationInfo::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_SuiFederationGetFederationInfo::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetFederationInfo);
}

FName UK2BeamNode_ApiRequest_SuiFederationGetFederationInfo::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(USuiFederationGetFederationInfoRequest, Make);
}

FString UK2BeamNode_ApiRequest_SuiFederationGetFederationInfo::GetServiceName() const
{
	return TEXT("SuiFederation");
}

FString UK2BeamNode_ApiRequest_SuiFederationGetFederationInfo::GetEndpointName() const
{
	return TEXT("GetFederationInfo");
}

UClass* UK2BeamNode_ApiRequest_SuiFederationGetFederationInfo::GetApiClass() const
{
	return UBeamSuiFederationApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationGetFederationInfo::GetRequestClass() const
{
	return USuiFederationGetFederationInfoRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationGetFederationInfo::GetResponseClass() const
{
	return UGetFederationInfoResponse::StaticClass();
}

FString UK2BeamNode_ApiRequest_SuiFederationGetFederationInfo::GetRequestSuccessDelegateName() const
{
	return TEXT("OnSuiFederationGetFederationInfoSuccess");
}

FString UK2BeamNode_ApiRequest_SuiFederationGetFederationInfo::GetRequestErrorDelegateName() const
{
	return TEXT("OnSuiFederationGetFederationInfoError");
}

FString UK2BeamNode_ApiRequest_SuiFederationGetFederationInfo::GetRequestCompleteDelegateName() const
{
	return TEXT("OnSuiFederationGetFederationInfoComplete");
}

#undef LOCTEXT_NAMESPACE
