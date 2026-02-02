#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/LiveOpsDemoMSDetachNftResponse.h"

#include "LiveOpsDemoMSDetachNftResponseLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API ULiveOpsDemoMSDetachNftResponseLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|LiveOpsDemoMS|Utils|Json", DisplayName="LiveOpsDemoMSDetachNftResponse To JSON String")
	static FString LiveOpsDemoMSDetachNftResponseToJsonString(const ULiveOpsDemoMSDetachNftResponse* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|LiveOpsDemoMS|Utils|Make/Break", DisplayName="Make LiveOpsDemoMSDetachNftResponse", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static ULiveOpsDemoMSDetachNftResponse* Make(bool bValue, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|LiveOpsDemoMS|Utils|Make/Break", DisplayName="Break LiveOpsDemoMSDetachNftResponse", meta=(NativeBreakFunc))
	static void Break(const ULiveOpsDemoMSDetachNftResponse* Serializable, bool& bValue);
};