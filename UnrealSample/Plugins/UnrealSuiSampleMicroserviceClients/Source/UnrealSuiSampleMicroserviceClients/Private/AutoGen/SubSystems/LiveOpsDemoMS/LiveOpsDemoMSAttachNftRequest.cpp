
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SubSystems/LiveOpsDemoMS/LiveOpsDemoMSAttachNftRequest.h"

void ULiveOpsDemoMSAttachNftRequest::BuildVerb(FString& VerbString) const
{
	VerbString = TEXT("POST");
}

void ULiveOpsDemoMSAttachNftRequest::BuildRoute(FString& RouteString) const
{
	FString Route = TEXT("micro_LiveOpsDemoMS/AttachNft");
	
	
	FString QueryParams = TEXT("");
	QueryParams.Reserve(1024);
	bool bIsFirstQueryParam = true;
	
	RouteString.Appendf(TEXT("%s%s"), *Route, *QueryParams);		
}

void ULiveOpsDemoMSAttachNftRequest::BuildBody(FString& BodyString) const
{
	ensureAlways(Body);

	TUnrealJsonSerializer JsonSerializer = TJsonStringWriter<TCondensedJsonPrintPolicy<TCHAR>>::Create(&BodyString);
	Body->BeamSerialize(JsonSerializer);
	JsonSerializer->Close();
}

ULiveOpsDemoMSAttachNftRequest* ULiveOpsDemoMSAttachNftRequest::Make(FString _ParentId, int64 _ParentsInstanceId, FString _NftId, UObject* RequestOwner, TMap<FString, FString> CustomHeaders)
{
	ULiveOpsDemoMSAttachNftRequest* Req = NewObject<ULiveOpsDemoMSAttachNftRequest>(RequestOwner);
	Req->CustomHeaders = TMap{CustomHeaders};

	// Pass in Path and Query Parameters (Blank if no path parameters exist)
	
	
	// Makes a body and fill up with parameters (Blank if no body parameters exist)
	Req->Body = NewObject<UAttachNftRequestArgs>(Req);
	Req->Body->ParentId = _ParentId;
	Req->Body->ParentsInstanceId = _ParentsInstanceId;
	Req->Body->NftId = _NftId;
	

	return Req;
}
