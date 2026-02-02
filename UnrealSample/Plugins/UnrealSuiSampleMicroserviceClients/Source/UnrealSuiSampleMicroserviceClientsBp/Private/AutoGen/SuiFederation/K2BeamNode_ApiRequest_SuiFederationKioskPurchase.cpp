

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/SuiFederation/K2BeamNode_ApiRequest_SuiFederationKioskPurchase.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamSuiFederationApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationKioskPurchaseRequest.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskPurchaseResponse.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_SuiFederationKioskPurchase"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_SuiFederationKioskPurchase::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_SuiFederationKioskPurchase::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, KioskPurchase);
}

FName UK2BeamNode_ApiRequest_SuiFederationKioskPurchase::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(USuiFederationKioskPurchaseRequest, Make);
}

FString UK2BeamNode_ApiRequest_SuiFederationKioskPurchase::GetServiceName() const
{
	return TEXT("SuiFederation");
}

FString UK2BeamNode_ApiRequest_SuiFederationKioskPurchase::GetEndpointName() const
{
	return TEXT("KioskPurchase");
}

UClass* UK2BeamNode_ApiRequest_SuiFederationKioskPurchase::GetApiClass() const
{
	return UBeamSuiFederationApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationKioskPurchase::GetRequestClass() const
{
	return USuiFederationKioskPurchaseRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationKioskPurchase::GetResponseClass() const
{
	return UKioskPurchaseResponse::StaticClass();
}

FString UK2BeamNode_ApiRequest_SuiFederationKioskPurchase::GetRequestSuccessDelegateName() const
{
	return TEXT("OnSuiFederationKioskPurchaseSuccess");
}

FString UK2BeamNode_ApiRequest_SuiFederationKioskPurchase::GetRequestErrorDelegateName() const
{
	return TEXT("OnSuiFederationKioskPurchaseError");
}

FString UK2BeamNode_ApiRequest_SuiFederationKioskPurchase::GetRequestCompleteDelegateName() const
{
	return TEXT("OnSuiFederationKioskPurchaseComplete");
}

#undef LOCTEXT_NAMESPACE
