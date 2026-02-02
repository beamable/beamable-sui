#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/AllListingsRequestArgs.h"

#include "AllListingsRequestArgsLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UAllListingsRequestArgsLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="AllListingsRequestArgs To JSON String")
	static FString AllListingsRequestArgsToJsonString(const UAllListingsRequestArgs* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make AllListingsRequestArgs", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UAllListingsRequestArgs* Make(FString OptionalKioskContentId, int32 Page, int32 PageSize, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break AllListingsRequestArgs", meta=(NativeBreakFunc))
	static void Break(const UAllListingsRequestArgs* Serializable, FString& OptionalKioskContentId, int32& Page, int32& PageSize);
};