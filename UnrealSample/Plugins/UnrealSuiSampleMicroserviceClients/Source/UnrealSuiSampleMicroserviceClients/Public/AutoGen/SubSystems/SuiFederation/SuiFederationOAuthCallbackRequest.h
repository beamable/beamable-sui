
#pragma once

#include "CoreMinimal.h"
#include "BeamBackend/BeamBaseRequestInterface.h"
#include "BeamBackend/BeamRequestContext.h"
#include "BeamBackend/BeamErrorResponse.h"
#include "BeamBackend/BeamFullResponse.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/OauthCallbackResponse.h"

#include "SuiFederationOAuthCallbackRequest.generated.h"

UCLASS(BlueprintType)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationOAuthCallbackRequest : public UObject, public IBeamBaseRequestInterface
{
	GENERATED_BODY()
	
public:

	// Path Params
	
	
	// Query Params
	

	// Body Params
	

	// Beam Base Request Declaration
	USuiFederationOAuthCallbackRequest() = default;

	virtual void BuildVerb(FString& VerbString) const override;
	virtual void BuildRoute(FString& RouteString) const override;
	virtual void BuildBody(FString& BodyString) const override;

	UFUNCTION(BlueprintPure, BlueprintInternalUseOnly, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationOAuthCallback",  meta=(DefaultToSelf="RequestOwner", AdvancedDisplay="RequestOwner", AutoCreateRefTerm="CustomHeaders"))
	static USuiFederationOAuthCallbackRequest* Make(UObject* RequestOwner, TMap<FString, FString> CustomHeaders);
};

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationOAuthCallbackSuccess, FBeamRequestContext, Context, USuiFederationOAuthCallbackRequest*, Request, UOauthCallbackResponse*, Response);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationOAuthCallbackError, FBeamRequestContext, Context, USuiFederationOAuthCallbackRequest*, Request, FBeamErrorResponse, Error);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_TwoParams(FOnSuiFederationOAuthCallbackComplete, FBeamRequestContext, Context, USuiFederationOAuthCallbackRequest*, Request);

using FSuiFederationOAuthCallbackFullResponse = FBeamFullResponse<USuiFederationOAuthCallbackRequest*, UOauthCallbackResponse*>;
DECLARE_DELEGATE_OneParam(FOnSuiFederationOAuthCallbackFullResponse, FSuiFederationOAuthCallbackFullResponse);
