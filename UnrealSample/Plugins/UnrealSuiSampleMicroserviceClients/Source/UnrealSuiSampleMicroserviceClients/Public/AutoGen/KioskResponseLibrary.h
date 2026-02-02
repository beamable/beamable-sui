#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskResponse.h"

#include "KioskResponseLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UKioskResponseLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="KioskResponse To JSON String")
	static FString KioskResponseToJsonString(const UKioskResponse* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make KioskResponse", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UKioskResponse* Make(FString KioskContentId, FString ItemContentType, FString PriceContentId, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break KioskResponse", meta=(NativeBreakFunc))
	static void Break(const UKioskResponse* Serializable, FString& KioskContentId, FString& ItemContentType, FString& PriceContentId);
};