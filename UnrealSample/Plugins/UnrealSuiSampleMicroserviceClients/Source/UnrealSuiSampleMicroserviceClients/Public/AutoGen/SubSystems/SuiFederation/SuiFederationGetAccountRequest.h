
#pragma once

#include "CoreMinimal.h"
#include "BeamBackend/BeamBaseRequestInterface.h"
#include "BeamBackend/BeamRequestContext.h"
#include "BeamBackend/BeamErrorResponse.h"
#include "BeamBackend/BeamFullResponse.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/GetAccountRequestArgs.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationGetAccountResponse.h"

#include "SuiFederationGetAccountRequest.generated.h"

UCLASS(BlueprintType)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationGetAccountRequest : public UObject, public IBeamBaseRequestInterface
{
	GENERATED_BODY()
	
public:

	// Path Params
	
	
	// Query Params
	

	// Body Params
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="", Category="Beam")
	UGetAccountRequestArgs* Body = {};

	// Beam Base Request Declaration
	USuiFederationGetAccountRequest() = default;

	virtual void BuildVerb(FString& VerbString) const override;
	virtual void BuildRoute(FString& RouteString) const override;
	virtual void BuildBody(FString& BodyString) const override;

	UFUNCTION(BlueprintPure, BlueprintInternalUseOnly, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationGetAccount",  meta=(DefaultToSelf="RequestOwner", AdvancedDisplay="RequestOwner", AutoCreateRefTerm="CustomHeaders"))
	static USuiFederationGetAccountRequest* Make(FString _Id, UObject* RequestOwner, TMap<FString, FString> CustomHeaders);
};

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationGetAccountSuccess, FBeamRequestContext, Context, USuiFederationGetAccountRequest*, Request, USuiFederationGetAccountResponse*, Response);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationGetAccountError, FBeamRequestContext, Context, USuiFederationGetAccountRequest*, Request, FBeamErrorResponse, Error);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_TwoParams(FOnSuiFederationGetAccountComplete, FBeamRequestContext, Context, USuiFederationGetAccountRequest*, Request);

using FSuiFederationGetAccountFullResponse = FBeamFullResponse<USuiFederationGetAccountRequest*, USuiFederationGetAccountResponse*>;
DECLARE_DELEGATE_OneParam(FOnSuiFederationGetAccountFullResponse, FSuiFederationGetAccountFullResponse);
