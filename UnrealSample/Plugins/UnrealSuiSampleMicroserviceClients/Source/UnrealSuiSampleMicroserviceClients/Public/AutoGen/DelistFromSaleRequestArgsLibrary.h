#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/DelistFromSaleRequestArgs.h"

#include "DelistFromSaleRequestArgsLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UDelistFromSaleRequestArgsLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="DelistFromSaleRequestArgs To JSON String")
	static FString DelistFromSaleRequestArgsToJsonString(const UDelistFromSaleRequestArgs* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make DelistFromSaleRequestArgs", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UDelistFromSaleRequestArgs* Make(FString ListingId, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break DelistFromSaleRequestArgs", meta=(NativeBreakFunc))
	static void Break(const UDelistFromSaleRequestArgs* Serializable, FString& ListingId);
};