#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/ImportRealmAccountRequestArgs.h"

#include "ImportRealmAccountRequestArgsLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UImportRealmAccountRequestArgsLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="ImportRealmAccountRequestArgs To JSON String")
	static FString ImportRealmAccountRequestArgsToJsonString(const UImportRealmAccountRequestArgs* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make ImportRealmAccountRequestArgs", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UImportRealmAccountRequestArgs* Make(FString PrivateKey, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break ImportRealmAccountRequestArgs", meta=(NativeBreakFunc))
	static void Break(const UImportRealmAccountRequestArgs* Serializable, FString& PrivateKey);
};