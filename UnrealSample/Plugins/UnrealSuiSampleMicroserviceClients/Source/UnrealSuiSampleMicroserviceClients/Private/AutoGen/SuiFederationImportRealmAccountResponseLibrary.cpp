
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationImportRealmAccountResponseLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString USuiFederationImportRealmAccountResponseLibrary::SuiFederationImportRealmAccountResponseToJsonString(const USuiFederationImportRealmAccountResponse* Serializable, const bool Pretty)
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

USuiFederationImportRealmAccountResponse* USuiFederationImportRealmAccountResponseLibrary::Make(FString Value, UObject* Outer)
{
	auto Serializable = NewObject<USuiFederationImportRealmAccountResponse>(Outer);
	Serializable->Value = Value;
	
	return Serializable;
}

void USuiFederationImportRealmAccountResponseLibrary::Break(const USuiFederationImportRealmAccountResponse* Serializable, FString& Value)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		Value = Serializable->Value;
	}
		
}

