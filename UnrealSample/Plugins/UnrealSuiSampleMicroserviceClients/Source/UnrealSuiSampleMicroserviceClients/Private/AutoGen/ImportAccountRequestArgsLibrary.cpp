
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/ImportAccountRequestArgsLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UImportAccountRequestArgsLibrary::ImportAccountRequestArgsToJsonString(const UImportAccountRequestArgs* Serializable, const bool Pretty)
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

UImportAccountRequestArgs* UImportAccountRequestArgsLibrary::Make(FString Id, FString PrivateKey, UObject* Outer)
{
	auto Serializable = NewObject<UImportAccountRequestArgs>(Outer);
	Serializable->Id = Id;
	Serializable->PrivateKey = PrivateKey;
	
	return Serializable;
}

void UImportAccountRequestArgsLibrary::Break(const UImportAccountRequestArgs* Serializable, FString& Id, FString& PrivateKey)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		Id = Serializable->Id;
		PrivateKey = Serializable->PrivateKey;
	}
		
}

