

#include "UnrealSuiSampleMicroserviceClientsBp/Public/AutoGen/SuiFederation/K2BeamNode_ApiRequest_SuiFederationDelistFromSale.h"

#include "BeamK2.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/BeamSuiFederationApi.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationDelistFromSaleRequest.h"
#include "Serialization/BeamPlainTextResponseBody.h"

#define LOCTEXT_NAMESPACE "K2BeamNode_ApiRequest_SuiFederationDelistFromSale"

using namespace BeamK2;

FName UK2BeamNode_ApiRequest_SuiFederationDelistFromSale::GetSelfFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, GetSelf);
}

FName UK2BeamNode_ApiRequest_SuiFederationDelistFromSale::GetRequestFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(UBeamSuiFederationApi, DelistFromSale);
}

FName UK2BeamNode_ApiRequest_SuiFederationDelistFromSale::GetMakeFunctionName() const
{
	return GET_FUNCTION_NAME_CHECKED(USuiFederationDelistFromSaleRequest, Make);
}

FString UK2BeamNode_ApiRequest_SuiFederationDelistFromSale::GetServiceName() const
{
	return TEXT("SuiFederation");
}

FString UK2BeamNode_ApiRequest_SuiFederationDelistFromSale::GetEndpointName() const
{
	return TEXT("DelistFromSale");
}

UClass* UK2BeamNode_ApiRequest_SuiFederationDelistFromSale::GetApiClass() const
{
	return UBeamSuiFederationApi::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationDelistFromSale::GetRequestClass() const
{
	return USuiFederationDelistFromSaleRequest::StaticClass();
}

UClass* UK2BeamNode_ApiRequest_SuiFederationDelistFromSale::GetResponseClass() const
{
	return UBeamPlainTextResponseBody::StaticClass();
}

FString UK2BeamNode_ApiRequest_SuiFederationDelistFromSale::GetRequestSuccessDelegateName() const
{
	return TEXT("OnSuiFederationDelistFromSaleSuccess");
}

FString UK2BeamNode_ApiRequest_SuiFederationDelistFromSale::GetRequestErrorDelegateName() const
{
	return TEXT("OnSuiFederationDelistFromSaleError");
}

FString UK2BeamNode_ApiRequest_SuiFederationDelistFromSale::GetRequestCompleteDelegateName() const
{
	return TEXT("OnSuiFederationDelistFromSaleComplete");
}

#undef LOCTEXT_NAMESPACE
