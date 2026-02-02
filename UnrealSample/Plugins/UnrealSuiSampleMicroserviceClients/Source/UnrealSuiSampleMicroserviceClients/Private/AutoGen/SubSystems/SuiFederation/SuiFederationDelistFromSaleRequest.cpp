
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationDelistFromSaleRequest.h"

void USuiFederationDelistFromSaleRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void USuiFederationDelistFromSaleRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_SuiFederation/DelistFromSale");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void USuiFederationDelistFromSaleRequest::BuildBody(FString& BodyString) const
{
	ensureAlways(Body);

	TUnrealJsonSerializer JsonSerializer = TJsonStringWriter<TCondensedJsonPrintPolicy<TCHAR>>::Create(&BodyString);
	Body->BeamSerialize(JsonSerializer);
	JsonSerializer->Close();
}

USuiFederationDelistFromSaleRequest* USuiFederationDelistFromSaleRequest::Make(FString _ListingId, UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	USuiFederationDelistFromSaleRequest* Req = NewObject<USuiFederationDelistFromSaleRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	Req->Body = NewObject<UDelistFromSaleRequestArgs>(Req);
	Req->Body->ListingId = _ListingId;
	

	return Req;
}
