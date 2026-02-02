#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/OauthCallbackResponse.h"

#include "OauthCallbackResponseLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UOauthCallbackResponseLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="OauthCallbackResponse To JSON String")
	static FString OauthCallbackResponseToJsonString(const UOauthCallbackResponse* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make OauthCallbackResponse", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UOauthCallbackResponse* Make(FString Message, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break OauthCallbackResponse", meta=(NativeBreakFunc))
	static void Break(const UOauthCallbackResponse* Serializable, FString& Message);
};