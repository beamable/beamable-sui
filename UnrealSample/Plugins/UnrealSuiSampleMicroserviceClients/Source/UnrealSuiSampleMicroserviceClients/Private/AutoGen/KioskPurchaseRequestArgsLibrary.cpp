
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskPurchaseRequestArgsLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UKioskPurchaseRequestArgsLibrary::KioskPurchaseRequestArgsToJsonString(const UKioskPurchaseRequestArgs* Serializable, const bool Pretty)
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

UKioskPurchaseRequestArgs* UKioskPurchaseRequestArgsLibrary::Make(FString ListingId, UObject* Outer)
{
	auto Serializable = NewObject<UKioskPurchaseRequestArgs>(Outer);
	Serializable->ListingId = ListingId;
	
	return Serializable;
}

void UKioskPurchaseRequestArgsLibrary::Break(const UKioskPurchaseRequestArgs* Serializable, FString& ListingId)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		ListingId = Serializable->ListingId;
	}
		
}

