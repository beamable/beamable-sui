
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationListForSaleRequest.h"

void USuiFederationListForSaleRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void USuiFederationListForSaleRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_SuiFederation/ListForSale");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void USuiFederationListForSaleRequest::BuildBody(FString& BodyString) const
{
	ensureAlways(Body);

	TUnrealJsonSerializer JsonSerializer = TJsonStringWriter<TCondensedJsonPrintPolicy<TCHAR>>::Create(&BodyString);
	Body->BeamSerialize(JsonSerializer);
	JsonSerializer->Close();
}

USuiFederationListForSaleRequest* USuiFederationListForSaleRequest::Make(int64 _ItemId, int64 _Price, FString _OptionalKioskContentId, UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	USuiFederationListForSaleRequest* Req = NewObject<USuiFederationListForSaleRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	Req->Body = NewObject<UListForSaleRequestArgs>(Req);
	Req->Body->ItemId = _ItemId;
	Req->Body->Price = _Price;
	Req->Body->OptionalKioskContentId = _OptionalKioskContentId;
	

	return Req;
}
