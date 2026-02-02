
#pragma once

#include "CoreMinimal.h"
#include "BeamBackend/BeamBaseRequestInterface.h"
#include "BeamBackend/BeamRequestContext.h"
#include "BeamBackend/BeamErrorResponse.h"
#include "BeamBackend/BeamFullResponse.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationInitializeContentContractsResponse.h"

#include "SuiFederationInitializeContentContractsRequest.generated.h"

UCLASS(BlueprintType)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationInitializeContentContractsRequest : public UObject, public IBeamBaseRequestInterface
{
	GENERATED_BODY()
	
public:

	// Path Params
	
	
	// Query Params
	

	// Body Params
	

	// Beam Base Request Declaration
	USuiFederationInitializeContentContractsRequest() = default;

	virtual void BuildVerb(FString& VerbString) const override;
	virtual void BuildRoute(FString& RouteString) const override;
	virtual void BuildBody(FString& BodyString) const override;

	UFUNCTION(BlueprintPure, BlueprintInternalUseOnly, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationInitializeContentContracts",  meta=(DefaultToSelf="RequestOwner", AdvancedDisplay="RequestOwner", AutoCreateRefTerm="CustomHeaders"))
	static USuiFederationInitializeContentContractsRequest* Make(UObject* RequestOwner, TMap<FString, FString> CustomHeaders);
};

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationInitializeContentContractsSuccess, FBeamRequestContext, Context, USuiFederationInitializeContentContractsRequest*, Request, USuiFederationInitializeContentContractsResponse*, Response);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationInitializeContentContractsError, FBeamRequestContext, Context, USuiFederationInitializeContentContractsRequest*, Request, FBeamErrorResponse, Error);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_TwoParams(FOnSuiFederationInitializeContentContractsComplete, FBeamRequestContext, Context, USuiFederationInitializeContentContractsRequest*, Request);

using FSuiFederationInitializeContentContractsFullResponse = FBeamFullResponse<USuiFederationInitializeContentContractsRequest*, USuiFederationInitializeContentContractsResponse*>;
DECLARE_DELEGATE_OneParam(FOnSuiFederationInitializeContentContractsFullResponse, FSuiFederationInitializeContentContractsFullResponse);
