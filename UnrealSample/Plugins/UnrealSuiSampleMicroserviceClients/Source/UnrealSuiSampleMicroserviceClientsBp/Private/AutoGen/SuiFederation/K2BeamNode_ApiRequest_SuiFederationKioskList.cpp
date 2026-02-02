

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/SuiFederation/K2BeamNode_ApiRequest_SuiFederationKioskList.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamSuiFederationApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationKioskListRequest.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListResponse.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_SuiFederationKioskList"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_SuiFederationKioskList::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_SuiFederationKioskList::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, KioskList);
}

FName UK2BeamNode_ApiRequest_SuiFederationKioskList::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(USuiFederationKioskListRequest, Make);
}

FString UK2BeamNode_ApiRequest_SuiFederationKioskList::GetServiceName() const
{
	return TEXT("SuiFederation");
}

FString UK2BeamNode_ApiRequest_SuiFederationKioskList::GetEndpointName() const
{
	return TEXT("KioskList");
}

UClass* UK2BeamNode_ApiRequest_SuiFederationKioskList::GetApiClass() const
{
	return UBeamSuiFederationApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationKioskList::GetRequestClass() const
{
	return USuiFederationKioskListRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationKioskList::GetResponseClass() const
{
	return UKioskListResponse::StaticClass();
}

FString UK2BeamNode_ApiRequest_SuiFederationKioskList::GetRequestSuccessDelegateName() const
{
	return TEXT("OnSuiFederationKioskListSuccess");
}

FString UK2BeamNode_ApiRequest_SuiFederationKioskList::GetRequestErrorDelegateName() const
{
	return TEXT("OnSuiFederationKioskListError");
}

FString UK2BeamNode_ApiRequest_SuiFederationKioskList::GetRequestCompleteDelegateName() const
{
	return TEXT("OnSuiFederationKioskListComplete");
}

#undef LOCTEXT_NAMESPACE
