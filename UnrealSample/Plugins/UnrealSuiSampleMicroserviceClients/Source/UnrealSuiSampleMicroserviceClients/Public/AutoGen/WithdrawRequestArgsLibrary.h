#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/WithdrawRequestArgs.h"

#include "WithdrawRequestArgsLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UWithdrawRequestArgsLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="WithdrawRequestArgs To JSON String")
	static FString WithdrawRequestArgsToJsonString(const UWithdrawRequestArgs* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make WithdrawRequestArgs", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UWithdrawRequestArgs* Make(FString ContentId, int64 Amount, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break WithdrawRequestArgs", meta=(NativeBreakFunc))
	static void Break(const UWithdrawRequestArgs* Serializable, FString& ContentId, int64& Amount);
};