
#pragma once

#include "CoreMinimal.h"
#include "BeamBackend/BeamBaseRequestInterface.h"
#include "BeamBackend/BeamRequestContext.h"
#include "BeamBackend/BeamErrorResponse.h"
#include "BeamBackend/BeamFullResponse.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/AttachNftRequestArgs.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/LiveOpsDemoMSAttachNftResponse.h"

#include "LiveOpsDemoMSAttachNftRequest.generated.h"

UCLASS(BlueprintType)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API ULiveOpsDemoMSAttachNftRequest : public UObject, public IBeamBaseRequestInterface
{
	GENERATED_BODY()
	
public:

	// Path Params
	
	
	// Query Params
	

	// Body Params
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="", Category="Beam")
	UAttachNftRequestArgs* Body = {};

	// Beam Base Request Declaration
	ULiveOpsDemoMSAttachNftRequest() = default;

	virtual void BuildVerb(FString& VerbString) const override;
	virtual void BuildRoute(FString& RouteString) const override;
	virtual void BuildBody(FString& BodyString) const override;

	UFUNCTION(BlueprintPure, BlueprintInternalUseOnly, Category="Beam|LiveOpsDemoMS|Utils|Make/Break", DisplayName="Make LiveOpsDemoMSAttachNft",  meta=(DefaultToSelf="RequestOwner", AdvancedDisplay="RequestOwner", AutoCreateRefTerm="CustomHeaders"))
	static ULiveOpsDemoMSAttachNftRequest* Make(FString _ParentId, int64 _ParentsInstanceId, FString _NftId, UObject* RequestOwner, TMap<FString, FString> CustomHeaders);
};

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnLiveOpsDemoMSAttachNftSuccess, FBeamRequestContext, Context, ULiveOpsDemoMSAttachNftRequest*, Request, ULiveOpsDemoMSAttachNftResponse*, Response);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnLiveOpsDemoMSAttachNftError, FBeamRequestContext, Context, ULiveOpsDemoMSAttachNftRequest*, Request, FBeamErrorResponse, Error);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_TwoParams(FOnLiveOpsDemoMSAttachNftComplete, FBeamRequestContext, Context, ULiveOpsDemoMSAttachNftRequest*, Request);

using FLiveOpsDemoMSAttachNftFullResponse = FBeamFullResponse<ULiveOpsDemoMSAttachNftRequest*, ULiveOpsDemoMSAttachNftResponse*>;
DECLARE_DELEGATE_OneParam(FOnLiveOpsDemoMSAttachNftFullResponse, FLiveOpsDemoMSAttachNftFullResponse);
