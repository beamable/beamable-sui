#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/LiveOpsDemoMSAttachNftResponse.h"

#include "LiveOpsDemoMSAttachNftResponseLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API ULiveOpsDemoMSAttachNftResponseLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|LiveOpsDemoMS|Utils|Json", DisplayName="LiveOpsDemoMSAttachNftResponse To JSON String")
	static FString LiveOpsDemoMSAttachNftResponseToJsonString(const ULiveOpsDemoMSAttachNftResponse* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|LiveOpsDemoMS|Utils|Make/Break", DisplayName="Make LiveOpsDemoMSAttachNftResponse", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static ULiveOpsDemoMSAttachNftResponse* Make(bool bValue, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|LiveOpsDemoMS|Utils|Make/Break", DisplayName="Break LiveOpsDemoMSAttachNftResponse", meta=(NativeBreakFunc))
	static void Break(const ULiveOpsDemoMSAttachNftResponse* Serializable, bool& bValue);
};