

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/LiveOpsDemoMS/K2BeamNode_ApiRequest_LiveOpsDemoMSAttachNft.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamLiveOpsDemoMSApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/LiveOpsDemoMS/LiveOpsDemoMSAttachNftRequest.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/LiveOpsDemoMSAttachNftResponse.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_LiveOpsDemoMSAttachNft"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_LiveOpsDemoMSAttachNft::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamLiveOpsDemoMSApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_LiveOpsDemoMSAttachNft::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamLiveOpsDemoMSApi, AttachNft);
}

FName UK2BeamNode_ApiRequest_LiveOpsDemoMSAttachNft::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(ULiveOpsDemoMSAttachNftRequest, Make);
}

FString UK2BeamNode_ApiRequest_LiveOpsDemoMSAttachNft::GetServiceName() const
{
	return TEXT("LiveOpsDemoMS");
}

FString UK2BeamNode_ApiRequest_LiveOpsDemoMSAttachNft::GetEndpointName() const
{
	return TEXT("AttachNft");
}

UClass* UK2BeamNode_ApiRequest_LiveOpsDemoMSAttachNft::GetApiClass() const
{
	return UBeamLiveOpsDemoMSApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_LiveOpsDemoMSAttachNft::GetRequestClass() const
{
	return ULiveOpsDemoMSAttachNftRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_LiveOpsDemoMSAttachNft::GetResponseClass() const
{
	return ULiveOpsDemoMSAttachNftResponse::StaticClass();
}

FString UK2BeamNode_ApiRequest_LiveOpsDemoMSAttachNft::GetRequestSuccessDelegateName() const
{
	return TEXT("OnLiveOpsDemoMSAttachNftSuccess");
}

FString UK2BeamNode_ApiRequest_LiveOpsDemoMSAttachNft::GetRequestErrorDelegateName() const
{
	return TEXT("OnLiveOpsDemoMSAttachNftError");
}

FString UK2BeamNode_ApiRequest_LiveOpsDemoMSAttachNft::GetRequestCompleteDelegateName() const
{
	return TEXT("OnLiveOpsDemoMSAttachNftComplete");
}

#undef LOCTEXT_NAMESPACE
