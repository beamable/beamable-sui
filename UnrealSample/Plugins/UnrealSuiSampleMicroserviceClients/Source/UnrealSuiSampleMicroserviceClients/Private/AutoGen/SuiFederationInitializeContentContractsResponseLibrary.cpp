
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationInitializeContentContractsResponseLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString USuiFederationInitializeContentContractsResponseLibrary::SuiFederationInitializeContentContractsResponseToJsonString(const USuiFederationInitializeContentContractsResponse* Serializable, const bool Pretty)
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

USuiFederationInitializeContentContractsResponse* USuiFederationInitializeContentContractsResponseLibrary::Make(FString Value, UObject* Outer)
{
	auto Serializable = NewObject<USuiFederationInitializeContentContractsResponse>(Outer);
	Serializable->Value = Value;
	
	return Serializable;
}

void USuiFederationInitializeContentContractsResponseLibrary::Break(const USuiFederationInitializeContentContractsResponse* Serializable, FString& Value)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		Value = Serializable->Value;
	}
		
}

