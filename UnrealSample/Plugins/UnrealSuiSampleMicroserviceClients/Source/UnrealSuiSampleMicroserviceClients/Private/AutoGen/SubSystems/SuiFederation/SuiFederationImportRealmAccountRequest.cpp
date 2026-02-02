
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationImportRealmAccountRequest.h"

void USuiFederationImportRealmAccountRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void USuiFederationImportRealmAccountRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_SuiFederation/ImportRealmAccount");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void USuiFederationImportRealmAccountRequest::BuildBody(FString& BodyString) const
{
	ensureAlways(Body);

	TUnrealJsonSerializer JsonSerializer = TJsonStringWriter<TCondensedJsonPrintPolicy<TCHAR>>::Create(&BodyString);
	Body->BeamSerialize(JsonSerializer);
	JsonSerializer->Close();
}

USuiFederationImportRealmAccountRequest* USuiFederationImportRealmAccountRequest::Make(FString _PrivateKey, UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	USuiFederationImportRealmAccountRequest* Req = NewObject<USuiFederationImportRealmAccountRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	Req->Body = NewObject<UImportRealmAccountRequestArgs>(Req);
	Req->Body->PrivateKey = _PrivateKey;
	

	return Req;
}
