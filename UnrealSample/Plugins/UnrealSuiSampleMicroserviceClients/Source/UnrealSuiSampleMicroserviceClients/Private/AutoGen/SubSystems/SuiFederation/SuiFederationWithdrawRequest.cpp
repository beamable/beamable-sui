
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/SuiFederation/SuiFederationWithdrawRequest.h"

void USuiFederationWithdrawRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void USuiFederationWithdrawRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_SuiFederation/Withdraw");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void USuiFederationWithdrawRequest::BuildBody(FString& BodyString) const
{
	ensureAlways(Body);

	TUnrealJsonSerializer JsonSerializer = TJsonStringWriter<TCondensedJsonPrintPolicy<TCHAR>>::Create(&BodyString);
	Body->BeamSerialize(JsonSerializer);
	JsonSerializer->Close();
}

USuiFederationWithdrawRequest* USuiFederationWithdrawRequest::Make(FString _ContentId, int64 _Amount, UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	USuiFederationWithdrawRequest* Req = NewObject<USuiFederationWithdrawRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	Req->Body = NewObject<UWithdrawRequestArgs>(Req);
	Req->Body->ContentId = _ContentId;
	Req->Body->Amount = _Amount;
	

	return Req;
}
