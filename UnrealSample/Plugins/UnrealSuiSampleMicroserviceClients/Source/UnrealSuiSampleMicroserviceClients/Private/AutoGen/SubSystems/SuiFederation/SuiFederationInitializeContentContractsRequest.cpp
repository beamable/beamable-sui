
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationInitializeContentContractsRequest.h"

void USuiFederationInitializeContentContractsRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void USuiFederationInitializeContentContractsRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_SuiFederation/InitializeContentContracts");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void USuiFederationInitializeContentContractsRequest::BuildBody(FString& BodyString) const
{
	
}

USuiFederationInitializeContentContractsRequest* USuiFederationInitializeContentContractsRequest::Make(UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	USuiFederationInitializeContentContractsRequest* Req = NewObject<USuiFederationInitializeContentContractsRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	

	return Req;
}
