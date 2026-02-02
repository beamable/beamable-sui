#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationGenerateRealmAccountResponse.h"

#include "SuiFederationGenerateRealmAccountResponseLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationGenerateRealmAccountResponseLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="SuiFederationGenerateRealmAccountResponse To JSON String")
	static FString SuiFederationGenerateRealmAccountResponseToJsonString(const USuiFederationGenerateRealmAccountResponse* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationGenerateRealmAccountResponse", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static USuiFederationGenerateRealmAccountResponse* Make(FString Value, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break SuiFederationGenerateRealmAccountResponse", meta=(NativeBreakFunc))
	static void Break(const USuiFederationGenerateRealmAccountResponse* Serializable, FString& Value);
};