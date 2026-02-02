#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskPurchaseRequestArgs.h"

#include "KioskPurchaseRequestArgsLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UKioskPurchaseRequestArgsLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="KioskPurchaseRequestArgs To JSON String")
	static FString KioskPurchaseRequestArgsToJsonString(const UKioskPurchaseRequestArgs* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make KioskPurchaseRequestArgs", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UKioskPurchaseRequestArgs* Make(FString ListingId, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break KioskPurchaseRequestArgs", meta=(NativeBreakFunc))
	static void Break(const UKioskPurchaseRequestArgs* Serializable, FString& ListingId);
};