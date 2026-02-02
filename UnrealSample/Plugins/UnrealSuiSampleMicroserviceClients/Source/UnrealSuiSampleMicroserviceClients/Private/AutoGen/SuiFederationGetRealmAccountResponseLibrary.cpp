
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/SuiFederationGetRealmAccountResponseLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString USuiFederationGetRealmAccountResponseLibrary::SuiFederationGetRealmAccountResponseToJsonString(const USuiFederationGetRealmAccountResponse* Serializable, const bool Pretty)
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

USuiFederationGetRealmAccountResponse* USuiFederationGetRealmAccountResponseLibrary::Make(FString Value, UObject* Outer)
{
	auto Serializable = NewObject<USuiFederationGetRealmAccountResponse>(Outer);
	Serializable->Value = Value;
	
	return Serializable;
}

void USuiFederationGetRealmAccountResponseLibrary::Break(const USuiFederationGetRealmAccountResponse* Serializable, FString& Value)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		Value = Serializable->Value;
	}
		
}

