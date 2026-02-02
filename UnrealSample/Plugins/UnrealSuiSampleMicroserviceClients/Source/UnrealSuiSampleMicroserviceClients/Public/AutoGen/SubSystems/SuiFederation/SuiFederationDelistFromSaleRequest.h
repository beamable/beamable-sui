
#pragma once

#include "CoreMinimal.h"
#include "BeamBackend/BeamBaseRequestInterface.h"
#include "BeamBackend/BeamRequestContext.h"
#include "BeamBackend/BeamErrorResponse.h"
#include "BeamBackend/BeamFullResponse.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/DelistFromSaleRequestArgs.h"
#include "Serialization/BeamPlainTextResponseBody.h"

#include "SuiFederationDelistFromSaleRequest.generated.h"

UCLASS(BlueprintType)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationDelistFromSaleRequest : public UObject, public IBeamBaseRequestInterface
{
	GENERATED_BODY()
	
public:

	// Path Params
	
	
	// Query Params
	

	// Body Params
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="", Category="Beam")
	UDelistFromSaleRequestArgs* Body = {};

	// Beam Base Request Declaration
	USuiFederationDelistFromSaleRequest() = default;

	virtual void BuildVerb(FString& VerbString) const override;
	virtual void BuildRoute(FString& RouteString) const override;
	virtual void BuildBody(FString& BodyString) const override;

	UFUNCTION(BlueprintPure, BlueprintInternalUseOnly, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationDelistFromSale",  meta=(DefaultToSelf="RequestOwner", AdvancedDisplay="RequestOwner", AutoCreateRefTerm="CustomHeaders"))
	static USuiFederationDelistFromSaleRequest* Make(FString _ListingId, UObject* RequestOwner, TMap<FString, FString> CustomHeaders);
};

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationDelistFromSaleSuccess, FBeamRequestContext, Context, USuiFederationDelistFromSaleRequest*, Request, UBeamPlainTextResponseBody*, Response);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationDelistFromSaleError, FBeamRequestContext, Context, USuiFederationDelistFromSaleRequest*, Request, FBeamErrorResponse, Error);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_TwoParams(FOnSuiFederationDelistFromSaleComplete, FBeamRequestContext, Context, USuiFederationDelistFromSaleRequest*, Request);

using FSuiFederationDelistFromSaleFullResponse = FBeamFullResponse<USuiFederationDelistFromSaleRequest*, UBeamPlainTextResponseBody*>;
DECLARE_DELEGATE_OneParam(FOnSuiFederationDelistFromSaleFullResponse, FSuiFederationDelistFromSaleFullResponse);
