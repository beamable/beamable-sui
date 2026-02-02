
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationKioskPurchaseRequest.h"

void USuiFederationKioskPurchaseRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void USuiFederationKioskPurchaseRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_SuiFederation/KioskPurchase");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void USuiFederationKioskPurchaseRequest::BuildBody(FString& BodyString) const
{
	ensureAlways(Body);

	TUnrealJsonSerializer JsonSerializer = TJsonStringWriter<TCondensedJsonPrintPolicy<TCHAR>>::Create(&BodyString);
	Body->BeamSerialize(JsonSerializer);
	JsonSerializer->Close();
}

USuiFederationKioskPurchaseRequest* USuiFederationKioskPurchaseRequest::Make(FString _ListingId, UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	USuiFederationKioskPurchaseRequest* Req = NewObject<USuiFederationKioskPurchaseRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	Req->Body = NewObject<UKioskPurchaseRequestArgs>(Req);
	Req->Body->ListingId = _ListingId;
	

	return Req;
}
