#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/AttachNftRequestArgs.h"

#include "AttachNftRequestArgsLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UAttachNftRequestArgsLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|LiveOpsDemoMS|Utils|Json", DisplayName="AttachNftRequestArgs To JSON String")
	static FString AttachNftRequestArgsToJsonString(const UAttachNftRequestArgs* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|LiveOpsDemoMS|Utils|Make/Break", DisplayName="Make AttachNftRequestArgs", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UAttachNftRequestArgs* Make(FString ParentId, int64 ParentsInstanceId, FString NftId, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|LiveOpsDemoMS|Utils|Make/Break", DisplayName="Break AttachNftRequestArgs", meta=(NativeBreakFunc))
	static void Break(const UAttachNftRequestArgs* Serializable, FString& ParentId, int64& ParentsInstanceId, FString& NftId);
};