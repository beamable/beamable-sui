
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/LiveOpsDemoMSAttachNftResponseLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString ULiveOpsDemoMSAttachNftResponseLibrary::LiveOpsDemoMSAttachNftResponseToJsonString(const ULiveOpsDemoMSAttachNftResponse* Serializable, const bool Pretty)
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

ULiveOpsDemoMSAttachNftResponse* ULiveOpsDemoMSAttachNftResponseLibrary::Make(bool bValue, UObject* Outer)
{
	auto Serializable = NewObject<ULiveOpsDemoMSAttachNftResponse>(Outer);
	Serializable->bValue = bValue;
	
	return Serializable;
}

void ULiveOpsDemoMSAttachNftResponseLibrary::Break(const ULiveOpsDemoMSAttachNftResponse* Serializable, bool& bValue)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		bValue = Serializable->bValue;
	}
		
}

