
#pragma once

#include "CoreMinimal.h"
#include "BeamBackend/BeamBaseRequestInterface.h"
#include "BeamBackend/BeamRequestContext.h"
#include "BeamBackend/BeamErrorResponse.h"
#include "BeamBackend/BeamFullResponse.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/DetachNftRequestArgs.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/LiveOpsDemoMSDetachNftResponse.h"

#include "LiveOpsDemoMSDetachNftRequest.generated.h"

UCLASS(BlueprintType)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API ULiveOpsDemoMSDetachNftRequest : public UObject, public IBeamBaseRequestInterface
{
	GENERATED_BODY()
	
public:

	// Path Params
	
	
	// Query Params
	

	// Body Params
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="", Category="Beam")
	UDetachNftRequestArgs* Body = {};

	// Beam Base Request Declaration
	ULiveOpsDemoMSDetachNftRequest() = default;

	virtual void BuildVerb(FString& VerbString) const override;
	virtual void BuildRoute(FString& RouteString) const override;
	virtual void BuildBody(FString& BodyString) const override;

	UFUNCTION(BlueprintPure, BlueprintInternalUseOnly, Category="Beam|LiveOpsDemoMS|Utils|Make/Break", DisplayName="Make LiveOpsDemoMSDetachNft",  meta=(DefaultToSelf="RequestOwner", AdvancedDisplay="RequestOwner", AutoCreateRefTerm="CustomHeaders"))
	static ULiveOpsDemoMSDetachNftRequest* Make(FString _ParentId, int64 _ParentsInstanceId, FString _NftId, UObject* RequestOwner, TMap<FString, FString> CustomHeaders);
};

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnLiveOpsDemoMSDetachNftSuccess, FBeamRequestContext, Context, ULiveOpsDemoMSDetachNftRequest*, Request, ULiveOpsDemoMSDetachNftResponse*, Response);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnLiveOpsDemoMSDetachNftError, FBeamRequestContext, Context, ULiveOpsDemoMSDetachNftRequest*, Request, FBeamErrorResponse, Error);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_TwoParams(FOnLiveOpsDemoMSDetachNftComplete, FBeamRequestContext, Context, ULiveOpsDemoMSDetachNftRequest*, Request);

using FLiveOpsDemoMSDetachNftFullResponse = FBeamFullResponse<ULiveOpsDemoMSDetachNftRequest*, ULiveOpsDemoMSDetachNftResponse*>;
DECLARE_DELEGATE_OneParam(FOnLiveOpsDemoMSDetachNftFullResponse, FLiveOpsDemoMSDetachNftFullResponse);
