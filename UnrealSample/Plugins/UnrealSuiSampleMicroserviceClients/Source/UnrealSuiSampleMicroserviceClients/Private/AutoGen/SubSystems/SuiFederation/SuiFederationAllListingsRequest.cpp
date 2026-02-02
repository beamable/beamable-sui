
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationAllListingsRequest.h"

void USuiFederationAllListingsRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void USuiFederationAllListingsRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_SuiFederation/AllListings");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void USuiFederationAllListingsRequest::BuildBody(FString& BodyString) const
{
	ensureAlways(Body);

	TUnrealJsonSerializer JsonSerializer = TJsonStringWriter<TCondensedJsonPrintPolicy<TCHAR>>::Create(&BodyString);
	Body->BeamSerialize(JsonSerializer);
	JsonSerializer->Close();
}

USuiFederationAllListingsRequest* USuiFederationAllListingsRequest::Make(FString _OptionalKioskContentId, int32 _Page, int32 _PageSize, UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	USuiFederationAllListingsRequest* Req = NewObject<USuiFederationAllListingsRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	Req->Body = NewObject<UAllListingsRequestArgs>(Req);
	Req->Body->OptionalKioskContentId = _OptionalKioskContentId;
	Req->Body->Page = _Page;
	Req->Body->PageSize = _PageSize;
	

	return Req;
}
