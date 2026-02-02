#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationGetAccountResponse.h"

#include "SuiFederationGetAccountResponseLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationGetAccountResponseLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="SuiFederationGetAccountResponse To JSON String")
	static FString SuiFederationGetAccountResponseToJsonString(const USuiFederationGetAccountResponse* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationGetAccountResponse", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static USuiFederationGetAccountResponse* Make(FString Value, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break SuiFederationGetAccountResponse", meta=(NativeBreakFunc))
	static void Break(const USuiFederationGetAccountResponse* Serializable, FString& Value);
};