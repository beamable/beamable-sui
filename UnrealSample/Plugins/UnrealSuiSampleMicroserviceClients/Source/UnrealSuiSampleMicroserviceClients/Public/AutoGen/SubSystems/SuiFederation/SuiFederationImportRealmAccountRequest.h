
#pragma once

#include "CoreMinimal.h"
#include "BeamBackend/BeamBaseRequestInterface.h"
#include "BeamBackend/BeamRequestContext.h"
#include "BeamBackend/BeamErrorResponse.h"
#include "BeamBackend/BeamFullResponse.h"

#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/ImportRealmAccountRequestArgs.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationImportRealmAccountResponse.h"

#include "SuiFederationImportRealmAccountRequest.generated.h"

UCLASS(BlueprintType)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationImportRealmAccountRequest : public UObject, public IBeamBaseRequestInterface
{
	GENERATED_BODY()
	
public:

	// Path Params
	
	
	// Query Params
	

	// Body Params
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="", Category="Beam")
	UImportRealmAccountRequestArgs* Body = {};

	// Beam Base Request Declaration
	USuiFederationImportRealmAccountRequest() = default;

	virtual void BuildVerb(FString& VerbString) const override;
	virtual void BuildRoute(FString& RouteString) const override;
	virtual void BuildBody(FString& BodyString) const override;

	UFUNCTION(BlueprintPure, BlueprintInternalUseOnly, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationImportRealmAccount",  meta=(DefaultToSelf="RequestOwner", AdvancedDisplay="RequestOwner", AutoCreateRefTerm="CustomHeaders"))
	static USuiFederationImportRealmAccountRequest* Make(FString _PrivateKey, UObject* RequestOwner, TMap<FString, FString> CustomHeaders);
};

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationImportRealmAccountSuccess, FBeamRequestContext, Context, USuiFederationImportRealmAccountRequest*, Request, USuiFederationImportRealmAccountResponse*, Response);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_ThreeParams(FOnSuiFederationImportRealmAccountError, FBeamRequestContext, Context, USuiFederationImportRealmAccountRequest*, Request, FBeamErrorResponse, Error);

UDELEGATE(BlueprintAuthorityOnly)
DECLARE_DYNAMIC_DELEGATE_TwoParams(FOnSuiFederationImportRealmAccountComplete, FBeamRequestContext, Context, USuiFederationImportRealmAccountRequest*, Request);

using FSuiFederationImportRealmAccountFullResponse = FBeamFullResponse<USuiFederationImportRealmAccountRequest*, USuiFederationImportRealmAccountResponse*>;
DECLARE_DELEGATE_OneParam(FOnSuiFederationImportRealmAccountFullResponse, FSuiFederationImportRealmAccountFullResponse);
