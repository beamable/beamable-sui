
#pragma once

#include "CoreMinimal.h"
#include "BeamBackend/BeamBaseRequestInterface.h"
#include "BeamBackend/BeamRequestContext.h"
#include "BeamBackend/BeamErrorResponse.h"
#include "BeamBackend/BeamFullResponse.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationGetRealmAccountResponse.h"

#include "SuiFederationGetRealmAccountRequest.generated.h"

UCLASS(BlueprintType)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationGetRealmAccountRequest : public UObject, public IBeamBaseRequestInterface
{
	GENERATED_BODY()
	
public:

	// Path Params
	
	
	// Query Params
	

	// Body Params
	

	// Beam Base Request Declaration
	USuiFederationGetRealmAccountRequest() = default;

	virtual void BuildVerb(FString& VerbString) const override;
	virtual void BuildRoute(FString& RouteString) const override;
	virtual void BuildBody(FString& BodyString) const override;

	UFUNCTION(BlueprintPure, BlueprintInternalUseOnly, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationGetRealmAccount",  meta=(DefaultToSelf="RequestOwner", AdvancedDisplay="RequestOwner", AutoCreateRefTerm="CustomHeaders"))
	static USuiFederationGetRealmAccountRequest* Make(UObject* RequestOwner, TMap<FString, FString> CustomHeaders);
};

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationGetRealmAccountSuccess, FBeamRequestContext, Context, USuiFederationGetRealmAccountRequest*, Request, USuiFederationGetRealmAccountResponse*, Response);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationGetRealmAccountError, FBeamRequestContext, Context, USuiFederationGetRealmAccountRequest*, Request, FBeamErrorResponse, Error);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_TwoParams(FOnSuiFederationGetRealmAccountComplete, FBeamRequestContext, Context, USuiFederationGetRealmAccountRequest*, Request);

using FSuiFederationGetRealmAccountFullResponse = FBeamFullResponse<USuiFederationGetRealmAccountRequest*, USuiFederationGetRealmAccountResponse*>;
DECLARE_DELEGATE_OneParam(FOnSuiFederationGetRealmAccountFullResponse, FSuiFederationGetRealmAccountFullResponse);
