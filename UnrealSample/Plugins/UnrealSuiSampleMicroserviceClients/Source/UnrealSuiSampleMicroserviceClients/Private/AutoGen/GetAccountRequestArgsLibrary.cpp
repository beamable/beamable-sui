
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/GetAccountRequestArgsLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UGetAccountRequestArgsLibrary::GetAccountRequestArgsToJsonString(const UGetAccountRequestArgs* Serializable, const bool Pretty)
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

UGetAccountRequestArgs* UGetAccountRequestArgsLibrary::Make(FString Id, UObject* Outer)
{
	auto Serializable = NewObject<UGetAccountRequestArgs>(Outer);
	Serializable->Id = Id;
	
	return Serializable;
}

void UGetAccountRequestArgsLibrary::Break(const UGetAccountRequestArgs* Serializable, FString& Id)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		Id = Serializable->Id;
	}
		
}

