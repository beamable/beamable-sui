#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/PlayerIdentity.h"

#include "PlayerIdentityLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UPlayerIdentityLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="PlayerIdentity To JSON String")
	static FString PlayerIdentityToJsonString(const UPlayerIdentity* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make PlayerIdentity", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UPlayerIdentity* Make(FString Microservice, FString NameSpace, FString Address, FString Type, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break PlayerIdentity", meta=(NativeBreakFunc))
	static void Break(const UPlayerIdentity* Serializable, FString& Microservice, FString& NameSpace, FString& Address, FString& Type);
};