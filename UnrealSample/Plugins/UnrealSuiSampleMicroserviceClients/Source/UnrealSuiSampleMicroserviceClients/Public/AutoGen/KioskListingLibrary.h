#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListing.h"

#include "KioskListingLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UKioskListingLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="KioskListing To JSON String")
	static FString KioskListingToJsonString(const UKioskListing* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make KioskListing", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UKioskListing* Make(FString ListingId, FString KioskContentId, FString KioskName, FString ItemContentId, FString ItemProxyId, int64 ItemInventoryId, FString PriceContentId, int64 Price, int64 GamerTag, FString Wallet, FDateTime CreatedAt, FString Status, TArray<UItemProperties*> Properties, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break KioskListing", meta=(NativeBreakFunc))
	static void Break(const UKioskListing* Serializable, FString& ListingId, FString& KioskContentId, FString& KioskName, FString& ItemContentId, FString& ItemProxyId, int64& ItemInventoryId, FString& PriceContentId, int64& Price, int64& GamerTag, FString& Wallet, FDateTime& CreatedAt, FString& Status, TArray<UItemProperties*>& Properties);
};