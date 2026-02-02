
#pragma once

#include "CoreMinimal.h"
#include "BeamBackend/BeamBaseRequestInterface.h"
#include "BeamBackend/BeamRequestContext.h"
#include "BeamBackend/BeamErrorResponse.h"
#include "BeamBackend/BeamFullResponse.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/ListForSaleRequestArgs.h"
#include "Serialization/BeamPlainTextResponseBody.h"

#include "SuiFederationListForSaleRequest.generated.h"

UCLASS(BlueprintType)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationListForSaleRequest : public UObject, public IBeamBaseRequestInterface
{
	GENERATED_BODY()
	
public:

	// Path Params
	
	
	// Query Params
	

	// Body Params
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="", Category="Beam")
	UListForSaleRequestArgs* Body = {};

	// Beam Base Request Declaration
	USuiFederationListForSaleRequest() = default;

	virtual void BuildVerb(FString& VerbString) const override;
	virtual void BuildRoute(FString& RouteString) const override;
	virtual void BuildBody(FString& BodyString) const override;

	UFUNCTION(BlueprintPure, BlueprintInternalUseOnly, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationListForSale",  meta=(DefaultToSelf="RequestOwner", AdvancedDisplay="RequestOwner", AutoCreateRefTerm="CustomHeaders"))
	static USuiFederationListForSaleRequest* Make(int64 _ItemId, int64 _Price, FString _OptionalKioskContentId, UObject* RequestOwner, TMap<FString, FString> CustomHeaders);
};

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationListForSaleSuccess, FBeamRequestContext, Context, USuiFederationListForSaleRequest*, Request, UBeamPlainTextResponseBody*, Response);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationListForSaleError, FBeamRequestContext, Context, USuiFederationListForSaleRequest*, Request, FBeamErrorResponse, Error);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_TwoParams(FOnSuiFederationListForSaleComplete, FBeamRequestContext, Context, USuiFederationListForSaleRequest*, Request);

using FSuiFederationListForSaleFullResponse = FBeamFullResponse<USuiFederationListForSaleRequest*, UBeamPlainTextResponseBody*>;
DECLARE_DELEGATE_OneParam(FOnSuiFederationListForSaleFullResponse, FSuiFederationListForSaleFullResponse);
