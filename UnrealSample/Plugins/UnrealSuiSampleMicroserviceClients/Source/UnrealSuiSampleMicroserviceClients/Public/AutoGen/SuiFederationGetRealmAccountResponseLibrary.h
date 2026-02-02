#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationGetRealmAccountResponse.h"

#include "SuiFederationGetRealmAccountResponseLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationGetRealmAccountResponseLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="SuiFederationGetRealmAccountResponse To JSON String")
	static FString SuiFederationGetRealmAccountResponseToJsonString(const USuiFederationGetRealmAccountResponse* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationGetRealmAccountResponse", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static USuiFederationGetRealmAccountResponse* Make(FString Value, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break SuiFederationGetRealmAccountResponse", meta=(NativeBreakFunc))
	static void Break(const USuiFederationGetRealmAccountResponse* Serializable, FString& Value);
};