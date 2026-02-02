
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationGetAccountRequest.h"

void USuiFederationGetAccountRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void USuiFederationGetAccountRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_SuiFederation/GetAccount");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void USuiFederationGetAccountRequest::BuildBody(FString& BodyString) const
{
	ensureAlways(Body);

	TUnrealJsonSerializer JsonSerializer = TJsonStringWriter<TCondensedJsonPrintPolicy<TCHAR>>::Create(&BodyString);
	Body->BeamSerialize(JsonSerializer);
	JsonSerializer->Close();
}

USuiFederationGetAccountRequest* USuiFederationGetAccountRequest::Make(FString _Id, UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	USuiFederationGetAccountRequest* Req = NewObject<USuiFederationGetAccountRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	Req->Body = NewObject<UGetAccountRequestArgs>(Req);
	Req->Body->Id = _Id;
	

	return Req;
}
