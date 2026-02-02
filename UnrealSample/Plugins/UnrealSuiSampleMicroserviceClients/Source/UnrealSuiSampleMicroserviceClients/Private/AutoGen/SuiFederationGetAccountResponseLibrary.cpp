
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationGetAccountResponseLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString USuiFederationGetAccountResponseLibrary::SuiFederationGetAccountResponseToJsonString(const USuiFederationGetAccountResponse* Serializable, const bool Pretty)
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

USuiFederationGetAccountResponse* USuiFederationGetAccountResponseLibrary::Make(FString Value, UObject* Outer)
{
	auto Serializable = NewObject<USuiFederationGetAccountResponse>(Outer);
	Serializable->Value = Value;
	
	return Serializable;
}

void USuiFederationGetAccountResponseLibrary::Break(const USuiFederationGetAccountResponse* Serializable, FString& Value)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		Value = Serializable->Value;
	}
		
}

