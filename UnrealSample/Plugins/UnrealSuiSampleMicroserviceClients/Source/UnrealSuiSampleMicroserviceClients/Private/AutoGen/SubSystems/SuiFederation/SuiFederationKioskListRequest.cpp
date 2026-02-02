
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationKioskListRequest.h"

void USuiFederationKioskListRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void USuiFederationKioskListRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_SuiFederation/KioskList");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void USuiFederationKioskListRequest::BuildBody(FString& BodyString) const
{
	
}

USuiFederationKioskListRequest* USuiFederationKioskListRequest::Make(UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	USuiFederationKioskListRequest* Req = NewObject<USuiFederationKioskListRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	

	return Req;
}
