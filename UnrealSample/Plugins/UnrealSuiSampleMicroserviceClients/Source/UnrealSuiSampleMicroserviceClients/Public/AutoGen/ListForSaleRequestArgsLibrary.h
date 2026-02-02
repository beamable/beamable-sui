#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/ListForSaleRequestArgs.h"

#include "ListForSaleRequestArgsLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UListForSaleRequestArgsLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="ListForSaleRequestArgs To JSON String")
	static FString ListForSaleRequestArgsToJsonString(const UListForSaleRequestArgs* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make ListForSaleRequestArgs", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UListForSaleRequestArgs* Make(int64 ItemId, int64 Price, FString OptionalKioskContentId, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break ListForSaleRequestArgs", meta=(NativeBreakFunc))
	static void Break(const UListForSaleRequestArgs* Serializable, int64& ItemId, int64& Price, FString& OptionalKioskContentId);
};