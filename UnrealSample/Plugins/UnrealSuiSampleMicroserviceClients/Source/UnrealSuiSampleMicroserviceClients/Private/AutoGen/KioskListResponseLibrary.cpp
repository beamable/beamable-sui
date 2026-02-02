
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListResponseLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UKioskListResponseLibrary::KioskListResponseToJsonString(const UKioskListResponse* Serializable, const bool Pretty)
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

UKioskListResponse* UKioskListResponseLibrary::Make(TArray<UKioskResponse*> Kiosks, UObject* Outer)
{
	auto Serializable = NewObject<UKioskListResponse>(Outer);
	Serializable->Kiosks = Kiosks;
	
	return Serializable;
}

void UKioskListResponseLibrary::Break(const UKioskListResponse* Serializable, TArray<UKioskResponse*>& Kiosks)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		Kiosks = Serializable->Kiosks;
	}
		
}

