
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/WithdrawRequestArgsLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UWithdrawRequestArgsLibrary::WithdrawRequestArgsToJsonString(const UWithdrawRequestArgs* Serializable, const bool Pretty)
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

UWithdrawRequestArgs* UWithdrawRequestArgsLibrary::Make(FString ContentId, int64 Amount, UObject* Outer)
{
	auto Serializable = NewObject<UWithdrawRequestArgs>(Outer);
	Serializable->ContentId = ContentId;
	Serializable->Amount = Amount;
	
	return Serializable;
}

void UWithdrawRequestArgsLibrary::Break(const UWithdrawRequestArgs* Serializable, FString& ContentId, int64& Amount)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		ContentId = Serializable->ContentId;
		Amount = Serializable->Amount;
	}
		
}

