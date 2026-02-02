
#pragma once

#include "CoreMinimal.h"
#include "BeamBackend/BeamBaseRequestInterface.h"
#include "BeamBackend/BeamRequestContext.h"
#include "BeamBackend/BeamErrorResponse.h"
#include "BeamBackend/BeamFullResponse.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/GetFederationInfoResponse.h"

#include "SuiFederationGetFederationInfoRequest.generated.h"

UCLASS(BlueprintType)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationGetFederationInfoRequest : public UObject, public IBeamBaseRequestInterface
{
	GENERATED_BODY()
	
public:

	// Path Params
	
	
	// Query Params
	

	// Body Params
	

	// Beam Base Request Declaration
	USuiFederationGetFederationInfoRequest() = default;

	virtual void BuildVerb(FString& VerbString) const override;
	virtual void BuildRoute(FString& RouteString) const override;
	virtual void BuildBody(FString& BodyString) const override;

	UFUNCTION(BlueprintPure, BlueprintInternalUseOnly, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationGetFederationInfo",  meta=(DefaultToSelf="RequestOwner", AdvancedDisplay="RequestOwner", AutoCreateRefTerm="CustomHeaders"))
	static USuiFederationGetFederationInfoRequest* Make(UObject* RequestOwner, TMap<FString, FString> CustomHeaders);
};

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationGetFederationInfoSuccess, FBeamRequestContext, Context, USuiFederationGetFederationInfoRequest*, Request, UGetFederationInfoResponse*, Response);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationGetFederationInfoError, FBeamRequestContext, Context, USuiFederationGetFederationInfoRequest*, Request, FBeamErrorResponse, Error);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_TwoParams(FOnSuiFederationGetFederationInfoComplete, FBeamRequestContext, Context, USuiFederationGetFederationInfoRequest*, Request);

using FSuiFederationGetFederationInfoFullResponse = FBeamFullResponse<USuiFederationGetFederationInfoRequest*, UGetFederationInfoResponse*>;
DECLARE_DELEGATE_OneParam(FOnSuiFederationGetFederationInfoFullResponse, FSuiFederationGetFederationInfoFullResponse);
