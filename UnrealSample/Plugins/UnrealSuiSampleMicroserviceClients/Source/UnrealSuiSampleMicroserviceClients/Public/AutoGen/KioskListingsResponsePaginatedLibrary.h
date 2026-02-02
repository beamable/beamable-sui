#pragma once

#include "CoreMinimal.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListingsResponsePaginated.h"

#include "KioskListingsResponsePaginatedLibrary.generated.h"


UCLASS(BlueprintType, Category="Beam")
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UKioskListingsResponsePaginatedLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Json", DisplayName="KioskListingsResponsePaginated To JSON String")
	static FString KioskListingsResponsePaginatedToJsonString(const UKioskListingsResponsePaginated* Serializable, const bool Pretty);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Make KioskListingsResponsePaginated", meta=(DefaultToSelf="Outer", AdvancedDisplay="Outer", NativeMakeFunc))
	static UKioskListingsResponsePaginated* Make(int32 Page, int32 PageSize, int64 TotalCount, TArray<UKioskListing*> Listings, UObject* Outer);

	UFUNCTION(BlueprintPure, Category="Beam|SuiFederation|Utils|Make/Break", DisplayName="Break KioskListingsResponsePaginated", meta=(NativeBreakFunc))
	static void Break(const UKioskListingsResponsePaginated* Serializable, int32& Page, int32& PageSize, int64& TotalCount, TArray<UKioskListing*>& Listings);
};