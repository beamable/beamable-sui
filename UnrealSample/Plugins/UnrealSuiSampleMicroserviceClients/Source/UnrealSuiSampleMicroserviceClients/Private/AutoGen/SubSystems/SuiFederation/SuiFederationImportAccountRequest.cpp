
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationImportAccountRequest.h"

void USuiFederationImportAccountRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void USuiFederationImportAccountRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_SuiFederation/ImportAccount");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void USuiFederationImportAccountRequest::BuildBody(FString& BodyString) const
{
	ensureAlways(Body);

	TUnrealJsonSerializer JsonSerializer = TJsonStringWriter<TCondensedJsonPrintPolicy<TCHAR>>::Create(&BodyString);
	Body->BeamSerialize(JsonSerializer);
	JsonSerializer->Close();
}

USuiFederationImportAccountRequest* USuiFederationImportAccountRequest::Make(FString _Id, FString _PrivateKey, UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	USuiFederationImportAccountRequest* Req = NewObject<USuiFederationImportAccountRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	Req->Body = NewObject<UImportAccountRequestArgs>(Req);
	Req->Body->Id = _Id;
	Req->Body->PrivateKey = _PrivateKey;
	

	return Req;
}
