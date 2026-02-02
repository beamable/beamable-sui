#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/DetachNftRequestArgs.h"

#include "DetachNftRequestArgsLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UDetachNftRequestArgsLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|LiveOpsDemoMS|Utils|Json", DisplayName="DetachNftRequestArgs To JSON String")
	static FString DetachNftRequestArgsToJsonString(const UDetachNftRequestArgs* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|LiveOpsDemoMS|Utils|Make/Break", DisplayName="Make DetachNftRequestArgs", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UDetachNftRequestArgs* Make(FString ParentId, int64 ParentsInstanceId, FString NftId, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|LiveOpsDemoMS|Utils|Make/Break", DisplayName="Break DetachNftRequestArgs", meta=(NativeBreakFunc))
	static void Break(const UDetachNftRequestArgs* Serializable, FString& ParentId, int64& ParentsInstanceId, FString& NftId);
};