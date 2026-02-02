
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationOAuthCallbackRequest.h"

void USuiFederationOAuthCallbackRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void USuiFederationOAuthCallbackRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_SuiFederation/OAuthCallback");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void USuiFederationOAuthCallbackRequest::BuildBody(FString& BodyString) const
{
	
}

USuiFederationOAuthCallbackRequest* USuiFederationOAuthCallbackRequest::Make(UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	USuiFederationOAuthCallbackRequest* Req = NewObject<USuiFederationOAuthCallbackRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	

	return Req;
}
