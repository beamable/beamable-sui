#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationImportRealmAccountResponse.h"

#include "SuiFederationImportRealmAccountResponseLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationImportRealmAccountResponseLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="SuiFederationImportRealmAccountResponse To JSON String")
	static FString SuiFederationImportRealmAccountResponseToJsonString(const USuiFederationImportRealmAccountResponse* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationImportRealmAccountResponse", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static USuiFederationImportRealmAccountResponse* Make(FString Value, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break SuiFederationImportRealmAccountResponse", meta=(NativeBreakFunc))
	static void Break(const USuiFederationImportRealmAccountResponse* Serializable, FString& Value);
};