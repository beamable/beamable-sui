
#pragma once

#include "CoreMinimal.h"
#include "BeamBackend/BeamBaseRequestInterface.h"
#include "BeamBackend/BeamRequestContext.h"
#include "BeamBackend/BeamErrorResponse.h"
#include "BeamBackend/BeamFullResponse.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationGenerateRealmAccountResponse.h"

#include "SuiFederationGenerateRealmAccountRequest.generated.h"

UCLASS(BlueprintType)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationGenerateRealmAccountRequest : public UObject, public IBeamBaseRequestInterface
{
	GENERATED_BODY()
	
public:

	// Path Params
	
	
	// Query Params
	

	// Body Params
	

	// Beam Base Request Declaration
	USuiFederationGenerateRealmAccountRequest() = default;

	virtual void BuildVerb(FString& VerbString) const override;
	virtual void BuildRoute(FString& RouteString) const override;
	virtual void BuildBody(FString& BodyString) const override;

	UFUNCTION(BlueprintPure, BlueprintInternalUseOnly, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationGenerateRealmAccount",  meta=(DefaultToSelf="RequestOwner", AdvancedDisplay="RequestOwner", AutoCreateRefTerm="CustomHeaders"))
	static USuiFederationGenerateRealmAccountRequest* Make(UObject* RequestOwner, TMap<FString, FString> CustomHeaders);
};

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationGenerateRealmAccountSuccess, FBeamRequestContext, Context, USuiFederationGenerateRealmAccountRequest*, Request, USuiFederationGenerateRealmAccountResponse*, Response);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationGenerateRealmAccountError, FBeamRequestContext, Context, USuiFederationGenerateRealmAccountRequest*, Request, FBeamErrorResponse, Error);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_TwoParams(FOnSuiFederationGenerateRealmAccountComplete, FBeamRequestContext, Context, USuiFederationGenerateRealmAccountRequest*, Request);

using FSuiFederationGenerateRealmAccountFullResponse = FBeamFullResponse<USuiFederationGenerateRealmAccountRequest*, USuiFederationGenerateRealmAccountResponse*>;
DECLARE_DELEGATE_OneParam(FOnSuiFederationGenerateRealmAccountFullResponse, FSuiFederationGenerateRealmAccountFullResponse);
