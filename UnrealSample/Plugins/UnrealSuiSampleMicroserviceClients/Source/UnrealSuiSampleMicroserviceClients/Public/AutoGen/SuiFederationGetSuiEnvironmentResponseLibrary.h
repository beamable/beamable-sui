#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationGetSuiEnvironmentResponse.h"

#include "SuiFederationGetSuiEnvironmentResponseLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationGetSuiEnvironmentResponseLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="SuiFederationGetSuiEnvironmentResponse To JSON String")
	static FString SuiFederationGetSuiEnvironmentResponseToJsonString(const USuiFederationGetSuiEnvironmentResponse* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationGetSuiEnvironmentResponse", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static USuiFederationGetSuiEnvironmentResponse* Make(FString Value, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break SuiFederationGetSuiEnvironmentResponse", meta=(NativeBreakFunc))
	static void Break(const USuiFederationGetSuiEnvironmentResponse* Serializable, FString& Value);
};