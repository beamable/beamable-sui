

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/SuiFederation/K2BeamNode_ApiRequest_SuiFederationListForSale.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamSuiFederationApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationListForSaleRequest.h"
#include "Serialization/BeamPlainTextResponseBody.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_SuiFederationListForSale"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_SuiFederationListForSale::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_SuiFederationListForSale::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, ListForSale);
}

FName UK2BeamNode_ApiRequest_SuiFederationListForSale::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(USuiFederationListForSaleRequest, Make);
}

FString UK2BeamNode_ApiRequest_SuiFederationListForSale::GetServiceName() const
{
	return TEXT("SuiFederation");
}

FString UK2BeamNode_ApiRequest_SuiFederationListForSale::GetEndpointName() const
{
	return TEXT("ListForSale");
}

UClass* UK2BeamNode_ApiRequest_SuiFederationListForSale::GetApiClass() const
{
	return UBeamSuiFederationApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationListForSale::GetRequestClass() const
{
	return USuiFederationListForSaleRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationListForSale::GetResponseClass() const
{
	return UBeamPlainTextResponseBody::StaticClass();
}

FString UK2BeamNode_ApiRequest_SuiFederationListForSale::GetRequestSuccessDelegateName() const
{
	return TEXT("OnSuiFederationListForSaleSuccess");
}

FString UK2BeamNode_ApiRequest_SuiFederationListForSale::GetRequestErrorDelegateName() const
{
	return TEXT("OnSuiFederationListForSaleError");
}

FString UK2BeamNode_ApiRequest_SuiFederationListForSale::GetRequestCompleteDelegateName() const
{
	return TEXT("OnSuiFederationListForSaleComplete");
}

#undef LOCTEXT_NAMESPACE
