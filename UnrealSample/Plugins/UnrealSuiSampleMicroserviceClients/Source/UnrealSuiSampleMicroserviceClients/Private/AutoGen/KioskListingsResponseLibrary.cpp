
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListingsResponseLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UKioskListingsResponseLibrary::KioskListingsResponseToJsonString(const UKioskListingsResponse* Serializable, const bool Pretty)
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

UKioskListingsResponse* UKioskListingsResponseLibrary::Make(TArray<UKioskListing*> Listings, UObject* Outer)
{
	auto Serializable = NewObject<UKioskListingsResponse>(Outer);
	Serializable->Listings = Listings;
	
	return Serializable;
}

void UKioskListingsResponseLibrary::Break(const UKioskListingsResponse* Serializable, TArray<UKioskListing*>& Listings)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		Listings = Serializable->Listings;
	}
		
}

