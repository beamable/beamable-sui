
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/ListForSaleRequestArgsLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UListForSaleRequestArgsLibrary::ListForSaleRequestArgsToJsonString(const UListForSaleRequestArgs* Serializable, const bool Pretty)
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

UListForSaleRequestArgs* UListForSaleRequestArgsLibrary::Make(int64 ItemId, int64 Price, FString OptionalKioskContentId, UObject* Outer)
{
	auto Serializable = NewObject<UListForSaleRequestArgs>(Outer);
	Serializable->ItemId = ItemId;
	Serializable->Price = Price;
	Serializable->OptionalKioskContentId = OptionalKioskContentId;
	
	return Serializable;
}

void UListForSaleRequestArgsLibrary::Break(const UListForSaleRequestArgs* Serializable, int64& ItemId, int64& Price, FString& OptionalKioskContentId)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		ItemId = Serializable->ItemId;
		Price = Serializable->Price;
		OptionalKioskContentId = Serializable->OptionalKioskContentId;
	}
		
}

