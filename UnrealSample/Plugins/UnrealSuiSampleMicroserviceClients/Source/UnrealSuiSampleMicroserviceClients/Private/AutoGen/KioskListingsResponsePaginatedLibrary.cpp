
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListingsResponsePaginatedLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UKioskListingsResponsePaginatedLibrary::KioskListingsResponsePaginatedToJsonString(const UKioskListingsResponsePaginated* Serializable, const bool Pretty)
{
	FString Result = FString{};
	if(Pretty)
	{
		TUnrealPrettyJsonSerializer JsonSerializer = TJsonStringWriter<TPrettyJsonPrintPolicy<TCHAR>>::Create(&Result);
		Serializable->BeamSerialize(JsonSerializer);
		JsonSerializer->Close();
	}
	else
	{
		TUnrealJsonSerializer JsonSerializer = TJsonStringWriter<TCondensedJsonPrintPolicy<TCHAR>>::Create(&Result);
		Serializable->BeamSerialize(JsonSerializer);
		JsonSerializer->Close();			
	}
	return Result;
}	

UKioskListingsResponsePaginated* UKioskListingsResponsePaginatedLibrary::Make(int32 Page, int32 PageSize, int64 TotalCount, TArray<UKioskListing*> Listings, UObject* Outer)
{
	auto Serializable = NewObject<UKioskListingsResponsePaginated>(Outer);
	Serializable->Page = Page;
	Serializable->PageSize = PageSize;
	Serializable->TotalCount = TotalCount;
	Serializable->Listings = Listings;
	
	return Serializable;
}

void UKioskListingsResponsePaginatedLibrary::Break(const UKioskListingsResponsePaginated* Serializable, int32& Page, int32& PageSize, int64& TotalCount, TArray<UKioskListing*>& Listings)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		Page = Serializable->Page;
		PageSize = Serializable->PageSize;
		TotalCount = Serializable->TotalCount;
		Listings = Serializable->Listings;
	}
		
}

