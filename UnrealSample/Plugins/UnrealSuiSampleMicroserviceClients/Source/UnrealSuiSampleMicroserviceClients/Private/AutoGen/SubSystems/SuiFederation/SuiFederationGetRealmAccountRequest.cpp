
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationGetRealmAccountRequest.h"

void USuiFederationGetRealmAccountRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void USuiFederationGetRealmAccountRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_SuiFederation/GetRealmAccount");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void USuiFederationGetRealmAccountRequest::BuildBody(FString& BodyString) const
{
	
}

USuiFederationGetRealmAccountRequest* USuiFederationGetRealmAccountRequest::Make(UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	USuiFederationGetRealmAccountRequest* Req = NewObject<USuiFederationGetRealmAccountRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	

	return Req;
}
