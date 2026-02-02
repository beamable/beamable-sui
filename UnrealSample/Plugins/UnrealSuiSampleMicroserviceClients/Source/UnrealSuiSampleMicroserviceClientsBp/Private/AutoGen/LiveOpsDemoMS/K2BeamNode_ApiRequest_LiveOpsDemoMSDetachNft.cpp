

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/LiveOpsDemoMS/K2BeamNode_ApiRequest_LiveOpsDemoMSDetachNft.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamLiveOpsDemoMSApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/LiveOpsDemoMS/LiveOpsDemoMSDetachNftRequest.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/LiveOpsDemoMSDetachNftResponse.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_LiveOpsDemoMSDetachNft"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_LiveOpsDemoMSDetachNft::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamLiveOpsDemoMSApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_LiveOpsDemoMSDetachNft::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamLiveOpsDemoMSApi, DetachNft);
}

FName UK2BeamNode_ApiRequest_LiveOpsDemoMSDetachNft::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(ULiveOpsDemoMSDetachNftRequest, Make);
}

FString UK2BeamNode_ApiRequest_LiveOpsDemoMSDetachNft::GetServiceName() const
{
	return TEXT("LiveOpsDemoMS");
}

FString UK2BeamNode_ApiRequest_LiveOpsDemoMSDetachNft::GetEndpointName() const
{
	return TEXT("DetachNft");
}

UClass* UK2BeamNode_ApiRequest_LiveOpsDemoMSDetachNft::GetApiClass() const
{
	return UBeamLiveOpsDemoMSApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_LiveOpsDemoMSDetachNft::GetRequestClass() const
{
	return ULiveOpsDemoMSDetachNftRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_LiveOpsDemoMSDetachNft::GetResponseClass() const
{
	return ULiveOpsDemoMSDetachNftResponse::StaticClass();
}

FString UK2BeamNode_ApiRequest_LiveOpsDemoMSDetachNft::GetRequestSuccessDelegateName() const
{
	return TEXT("OnLiveOpsDemoMSDetachNftSuccess");
}

FString UK2BeamNode_ApiRequest_LiveOpsDemoMSDetachNft::GetRequestErrorDelegateName() const
{
	return TEXT("OnLiveOpsDemoMSDetachNftError");
}

FString UK2BeamNode_ApiRequest_LiveOpsDemoMSDetachNft::GetRequestCompleteDelegateName() const
{
	return TEXT("OnLiveOpsDemoMSDetachNftComplete");
}

#undef LOCTEXT_NAMESPACE
