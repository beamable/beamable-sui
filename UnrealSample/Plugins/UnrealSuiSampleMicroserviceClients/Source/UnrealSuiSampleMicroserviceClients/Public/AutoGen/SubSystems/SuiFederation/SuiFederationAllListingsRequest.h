
#pragma once

#include "CoreMinimal.h"
#include "BeamBackend/BeamBaseRequestInterface.h"
#include "BeamBackend/BeamRequestContext.h"
#include "BeamBackend/BeamErrorResponse.h"
#include "BeamBackend/BeamFullResponse.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/AllListingsRequestArgs.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListingsResponsePaginated.h"

#include "SuiFederationAllListingsRequest.generated.h"

UCLASS(BlueprintType)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationAllListingsRequest : public UObject, public IBeamBaseRequestInterface
{
	GENERATED_BODY()
	
public:

	// Path Params
	
	
	// Query Params
	

	// Body Params
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="", Category="Beam")
	UAllListingsRequestArgs* Body = {};

	// Beam Base Request Declaration
	USuiFederationAllListingsRequest() = default;

	virtual void BuildVerb(FString& VerbString) const override;
	virtual void BuildRoute(FString& RouteString) const override;
	virtual void BuildBody(FString& BodyString) const override;

	UFUNCTION(BlueprintPure, BlueprintInternalUseOnly, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationAllListings",  meta=(DefaultToSelf="RequestOwner", AdvancedDisplay="RequestOwner", AutoCreateRefTerm="CustomHeaders"))
	static USuiFederationAllListingsRequest* Make(FString _OptionalKioskContentId, int32 _Page, int32 _PageSize, UObject* RequestOwner, TMap<FString, FString> CustomHeaders);
};

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationAllListingsSuccess, FBeamRequestContext, Context, USuiFederationAllListingsRequest*, Request, UKioskListingsResponsePaginated*, Response);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationAllListingsError, FBeamRequestContext, Context, USuiFederationAllListingsRequest*, Request, FBeamErrorResponse, Error);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_TwoParams(FOnSuiFederationAllListingsComplete, FBeamRequestContext, Context, USuiFederationAllListingsRequest*, Request);

using FSuiFederationAllListingsFullResponse = FBeamFullResponse<USuiFederationAllListingsRequest*, UKioskListingsResponsePaginated*>;
DECLARE_DELEGATE_OneParam(FOnSuiFederationAllListingsFullResponse, FSuiFederationAllListingsFullResponse);
