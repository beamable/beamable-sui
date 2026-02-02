#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/ImportAccountRequestArgs.h"

#include "ImportAccountRequestArgsLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UImportAccountRequestArgsLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="ImportAccountRequestArgs To JSON String")
	static FString ImportAccountRequestArgsToJsonString(const UImportAccountRequestArgs* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make ImportAccountRequestArgs", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UImportAccountRequestArgs* Make(FString Id, FString PrivateKey, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break ImportAccountRequestArgs", meta=(NativeBreakFunc))
	static void Break(const UImportAccountRequestArgs* Serializable, FString& Id, FString& PrivateKey);
};