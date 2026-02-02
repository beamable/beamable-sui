
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/ItemPropertiesLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UItemPropertiesLibrary::ItemPropertiesToJsonString(const UItemProperties* Serializable, const bool Pretty)
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

UItemProperties* UItemPropertiesLibrary::Make(FString Name, FString Value, UObject* Outer)
{
	auto Serializable = NewObject<UItemProperties>(Outer);
	Serializable->Name = Name;
	Serializable->Value = Value;
	
	return Serializable;
}

void UItemPropertiesLibrary::Break(const UItemProperties* Serializable, FString& Name, FString& Value)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		Name = Serializable->Name;
		Value = Serializable->Value;
	}
		
}

