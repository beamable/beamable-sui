
#pragma once

#include "CoreMinimal.h"
#include "BeamBackend/BeamBaseRequestInterface.h"
#include "BeamBackend/BeamRequestContext.h"
#include "BeamBackend/BeamErrorResponse.h"
#include "BeamBackend/BeamFullResponse.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskPurchaseRequestArgs.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskPurchaseResponse.h"

#include "SuiFederationKioskPurchaseRequest.generated.h"

UCLASS(BlueprintType)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationKioskPurchaseRequest : public UObject, public IBeamBaseRequestInterface
{
	GENERATED_BODY()
	
public:

	// Path Params
	
	
	// Query Params
	

	// Body Params
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="", Category="Beam")
	UKioskPurchaseRequestArgs* Body = {};

	// Beam Base Request Declaration
	USuiFederationKioskPurchaseRequest() = default;

	virtual void BuildVerb(FString& VerbString) const override;
	virtual void BuildRoute(FString& RouteString) const override;
	virtual void BuildBody(FString& BodyString) const override;

	UFUNCTION(BlueprintPure, BlueprintInternalUseOnly, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationKioskPurchase",  meta=(DefaultToSelf="RequestOwner", AdvancedDisplay="RequestOwner", AutoCreateRefTerm="CustomHeaders"))
	static USuiFederationKioskPurchaseRequest* Make(FString _ListingId, UObject* RequestOwner, TMap<FString, FString> CustomHeaders);
};

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationKioskPurchaseSuccess, FBeamRequestContext, Context, USuiFederationKioskPurchaseRequest*, Request, UKioskPurchaseResponse*, Response);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationKioskPurchaseError, FBeamRequestContext, Context, USuiFederationKioskPurchaseRequest*, Request, FBeamErrorResponse, Error);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_TwoParams(FOnSuiFederationKioskPurchaseComplete, FBeamRequestContext, Context, USuiFederationKioskPurchaseRequest*, Request);

using FSuiFederationKioskPurchaseFullResponse = FBeamFullResponse<USuiFederationKioskPurchaseRequest*, UKioskPurchaseResponse*>;
DECLARE_DELEGATE_OneParam(FOnSuiFederationKioskPurchaseFullResponse, FSuiFederationKioskPurchaseFullResponse);
