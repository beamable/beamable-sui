
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/DelistFromSaleRequestArgsLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UDelistFromSaleRequestArgsLibrary::DelistFromSaleRequestArgsToJsonString(const UDelistFromSaleRequestArgs* Serializable, const bool Pretty)
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

UDelistFromSaleRequestArgs* UDelistFromSaleRequestArgsLibrary::Make(FString ListingId, UObject* Outer)
{
	auto Serializable = NewObject<UDelistFromSaleRequestArgs>(Outer);
	Serializable->ListingId = ListingId;
	
	return Serializable;
}

void UDelistFromSaleRequestArgsLibrary::Break(const UDelistFromSaleRequestArgs* Serializable, FString& ListingId)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		ListingId = Serializable->ListingId;
	}
		
}

