
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/ImportRealmAccountRequestArgsLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UImportRealmAccountRequestArgsLibrary::ImportRealmAccountRequestArgsToJsonString(const UImportRealmAccountRequestArgs* Serializable, const bool Pretty)
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

UImportRealmAccountRequestArgs* UImportRealmAccountRequestArgsLibrary::Make(FString PrivateKey, UObject* Outer)
{
	auto Serializable = NewObject<UImportRealmAccountRequestArgs>(Outer);
	Serializable->PrivateKey = PrivateKey;
	
	return Serializable;
}

void UImportRealmAccountRequestArgsLibrary::Break(const UImportRealmAccountRequestArgs* Serializable, FString& PrivateKey)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		PrivateKey = Serializable->PrivateKey;
	}
		
}

