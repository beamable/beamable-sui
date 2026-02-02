#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationInitializeContentContractsResponse.h"

#include "SuiFederationInitializeContentContractsResponseLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API USuiFederationInitializeContentContractsResponseLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="SuiFederationInitializeContentContractsResponse To JSON String")
	static FString SuiFederationInitializeContentContractsResponseToJsonString(const USuiFederationInitializeContentContractsResponse* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make SuiFederationInitializeContentContractsResponse", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static USuiFederationInitializeContentContractsResponse* Make(FString Value, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break SuiFederationInitializeContentContractsResponse", meta=(NativeBreakFunc))
	static void Break(const USuiFederationInitializeContentContractsResponse* Serializable, FString& Value);
};