
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/LiveOpsDemoMSDetachNftResponseLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString ULiveOpsDemoMSDetachNftResponseLibrary::LiveOpsDemoMSDetachNftResponseToJsonString(const ULiveOpsDemoMSDetachNftResponse* Serializable, const bool Pretty)
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

ULiveOpsDemoMSDetachNftResponse* ULiveOpsDemoMSDetachNftResponseLibrary::Make(bool bValue, UObject* Outer)
{
	auto Serializable = NewObject<ULiveOpsDemoMSDetachNftResponse>(Outer);
	Serializable->bValue = bValue;
	
	return Serializable;
}

void ULiveOpsDemoMSDetachNftResponseLibrary::Break(const ULiveOpsDemoMSDetachNftResponse* Serializable, bool& bValue)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		bValue = Serializable->bValue;
	}
		
}

