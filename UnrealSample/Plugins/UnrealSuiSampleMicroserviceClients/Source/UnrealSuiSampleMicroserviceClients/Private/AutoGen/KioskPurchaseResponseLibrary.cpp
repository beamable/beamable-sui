
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskPurchaseResponseLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UKioskPurchaseResponseLibrary::KioskPurchaseResponseToJsonString(const UKioskPurchaseResponse* Serializable, const bool Pretty)
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

UKioskPurchaseResponse* UKioskPurchaseResponseLibrary::Make(UObject* Outer)
{
	auto Serializable = NewObject<UKioskPurchaseResponse>(Outer);
	
	return Serializable;
}



