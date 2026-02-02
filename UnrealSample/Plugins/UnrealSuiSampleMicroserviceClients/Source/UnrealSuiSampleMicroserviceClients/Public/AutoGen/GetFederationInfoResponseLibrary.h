#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/GetFederationInfoResponse.h"

#include "GetFederationInfoResponseLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UGetFederationInfoResponseLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="GetFederationInfoResponse To JSON String")
	static FString GetFederationInfoResponseToJsonString(const UGetFederationInfoResponse* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make GetFederationInfoResponse", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UGetFederationInfoResponse* Make(int64 GamerTag, FString Email, TArray<UPlayerIdentity*> ExternalIdentities, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break GetFederationInfoResponse", meta=(NativeBreakFunc))
	static void Break(const UGetFederationInfoResponse* Serializable, int64& GamerTag, FString& Email, TArray<UPlayerIdentity*>& ExternalIdentities);
};