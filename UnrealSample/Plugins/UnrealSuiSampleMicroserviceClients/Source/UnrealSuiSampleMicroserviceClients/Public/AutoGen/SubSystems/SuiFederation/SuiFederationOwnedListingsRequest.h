
#pragma once

#include "CoreMinimal.h"
#include "BeamBackend/BeamBaseRequestInterface.h"
#include "BeamBackend/BeamRequestContext.h"
#include "BeamBackend/BeamErrorResponse.h"
#include "BeamBackend/BeamFullResponse.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/OwnedListingsRequestArgs.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListingsResponse.h"

#include "SuiFederationOwnedListingsRequest.generated.h"

UCLASS(BlueprintType)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationOwnedListingsRequest : public UObject, public IBeamBaseRequestInterface
{
	GENERATED_BODY()
	
public:

	// Path Params
	
	
	// Query Params
	

	// Body Params
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="", Category="Beam")
	UOwnedListingsRequestArgs* Body = {};

	// Beam Base Request Declaration
	USuiFederationOwnedListingsRequest() = default;

	virtual void BuildVerb(FString& VerbString) const override;
	virtual void BuildRoute(FString& RouteString) const override;
	virtual void BuildBody(FString& BodyString) const override;

	UFUNCTION(BlueprintPure, BlueprintInternalUseOnly, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationOwnedListings",  meta=(DefaultToSelf="RequestOwner", AdvancedDisplay="RequestOwner", AutoCreateRefTerm="CustomHeaders"))
	static USuiFederationOwnedListingsRequest* Make(FString _OptionalKioskContentId, UObject* RequestOwner, TMap<FString, FString> CustomHeaders);
};

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationOwnedListingsSuccess, FBeamRequestContext, Context, USuiFederationOwnedListingsRequest*, Request, UKioskListingsResponse*, Response);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationOwnedListingsError, FBeamRequestContext, Context, USuiFederationOwnedListingsRequest*, Request, FBeamErrorResponse, Error);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_TwoParams(FOnSuiFederationOwnedListingsComplete, FBeamRequestContext, Context, USuiFederationOwnedListingsRequest*, Request);

using FSuiFederationOwnedListingsFullResponse = FBeamFullResponse<USuiFederationOwnedListingsRequest*, UKioskListingsResponse*>;
DECLARE_DELEGATE_OneParam(FOnSuiFederationOwnedListingsFullResponse, FSuiFederationOwnedListingsFullResponse);
