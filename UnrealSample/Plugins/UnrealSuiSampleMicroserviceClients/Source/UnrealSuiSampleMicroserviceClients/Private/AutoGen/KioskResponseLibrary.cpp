
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskResponseLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UKioskResponseLibrary::KioskResponseToJsonString(const UKioskResponse* Serializable, const bool Pretty)
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

UKioskResponse* UKioskResponseLibrary::Make(FString KioskContentId, FString ItemContentType, FString PriceContentId, UObject* Outer)
{
	auto Serializable = NewObject<UKioskResponse>(Outer);
	Serializable->KioskContentId = KioskContentId;
	Serializable->ItemContentType = ItemContentType;
	Serializable->PriceContentId = PriceContentId;
	
	return Serializable;
}

void UKioskResponseLibrary::Break(const UKioskResponse* Serializable, FString& KioskContentId, FString& ItemContentType, FString& PriceContentId)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		KioskContentId = Serializable->KioskContentId;
		ItemContentType = Serializable->ItemContentType;
		PriceContentId = Serializable->PriceContentId;
	}
		
}

