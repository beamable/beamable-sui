
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationOwnedListingsRequest.h"

void USuiFederationOwnedListingsRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void USuiFederationOwnedListingsRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_SuiFederation/OwnedListings");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void USuiFederationOwnedListingsRequest::BuildBody(FString& BodyString) const
{
	ensureAlways(Body);

	TUnrealJsonSerializer JsonSerializer = TJsonStringWriter<TCondensedJsonPrintPolicy<TCHAR>>::Create(&BodyString);
	Body->BeamSerialize(JsonSerializer);
	JsonSerializer->Close();
}

USuiFederationOwnedListingsRequest* USuiFederationOwnedListingsRequest::Make(FString _OptionalKioskContentId, UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	USuiFederationOwnedListingsRequest* Req = NewObject<USuiFederationOwnedListingsRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	Req->Body = NewObject<UOwnedListingsRequestArgs>(Req);
	Req->Body->OptionalKioskContentId = _OptionalKioskContentId;
	

	return Req;
}
