
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationGenerateRealmAccountRequest.h"

void USuiFederationGenerateRealmAccountRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void USuiFederationGenerateRealmAccountRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_SuiFederation/GenerateRealmAccount");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void USuiFederationGenerateRealmAccountRequest::BuildBody(FString& BodyString) const
{
	
}

USuiFederationGenerateRealmAccountRequest* USuiFederationGenerateRealmAccountRequest::Make(UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	USuiFederationGenerateRealmAccountRequest* Req = NewObject<USuiFederationGenerateRealmAccountRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	

	return Req;
}
