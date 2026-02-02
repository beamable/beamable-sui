#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/OwnedListingsRequestArgs.h"

#include "OwnedListingsRequestArgsLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UOwnedListingsRequestArgsLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="OwnedListingsRequestArgs To JSON String")
	static FString OwnedListingsRequestArgsToJsonString(const UOwnedListingsRequestArgs* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make OwnedListingsRequestArgs", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UOwnedListingsRequestArgs* Make(FString OptionalKioskContentId, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break OwnedListingsRequestArgs", meta=(NativeBreakFunc))
	static void Break(const UOwnedListingsRequestArgs* Serializable, FString& OptionalKioskContentId);
};