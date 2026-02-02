
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/LiveOpsDemoMS/LiveOpsDemoMSDetachNftRequest.h"

void ULiveOpsDemoMSDetachNftRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void ULiveOpsDemoMSDetachNftRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_LiveOpsDemoMS/DetachNft");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void ULiveOpsDemoMSDetachNftRequest::BuildBody(FString& BodyString) const
{
	ensureAlways(Body);

	TUnrealJsonSerializer JsonSerializer = TJsonStringWriter<TCondensedJsonPrintPolicy<TCHAR>>::Create(&BodyString);
	Body->BeamSerialize(JsonSerializer);
	JsonSerializer->Close();
}

ULiveOpsDemoMSDetachNftRequest* ULiveOpsDemoMSDetachNftRequest::Make(FString _ParentId, int64 _ParentsInstanceId, FString _NftId, UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	ULiveOpsDemoMSDetachNftRequest* Req = NewObject<ULiveOpsDemoMSDetachNftRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	Req->Body = NewObject<UDetachNftRequestArgs>(Req);
	Req->Body->ParentId = _ParentId;
	Req->Body->ParentsInstanceId = _ParentsInstanceId;
	Req->Body->NftId = _NftId;
	

	return Req;
}
