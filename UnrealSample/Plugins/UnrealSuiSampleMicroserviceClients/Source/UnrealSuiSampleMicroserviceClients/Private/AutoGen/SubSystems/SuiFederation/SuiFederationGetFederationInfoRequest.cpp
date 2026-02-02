
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationGetFederationInfoRequest.h"

void USuiFederationGetFederationInfoRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void USuiFederationGetFederationInfoRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_SuiFederation/GetFederationInfo");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void USuiFederationGetFederationInfoRequest::BuildBody(FString& BodyString) const
{
	
}

USuiFederationGetFederationInfoRequest* USuiFederationGetFederationInfoRequest::Make(UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	USuiFederationGetFederationInfoRequest* Req = NewObject<USuiFederationGetFederationInfoRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	

	return Req;
}
