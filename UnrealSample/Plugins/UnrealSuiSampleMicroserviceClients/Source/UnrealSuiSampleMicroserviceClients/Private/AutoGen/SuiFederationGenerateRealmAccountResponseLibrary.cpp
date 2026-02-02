
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationGenerateRealmAccountResponseLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString USuiFederationGenerateRealmAccountResponseLibrary::SuiFederationGenerateRealmAccountResponseToJsonString(const USuiFederationGenerateRealmAccountResponse* Serializable, const bool Pretty)
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

USuiFederationGenerateRealmAccountResponse* USuiFederationGenerateRealmAccountResponseLibrary::Make(FString Value, UObject* Outer)
{
	auto Serializable = NewObject<USuiFederationGenerateRealmAccountResponse>(Outer);
	Serializable->Value = Value;
	
	return Serializable;
}

void USuiFederationGenerateRealmAccountResponseLibrary::Break(const USuiFederationGenerateRealmAccountResponse* Serializable, FString& Value)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		Value = Serializable->Value;
	}
		
}

