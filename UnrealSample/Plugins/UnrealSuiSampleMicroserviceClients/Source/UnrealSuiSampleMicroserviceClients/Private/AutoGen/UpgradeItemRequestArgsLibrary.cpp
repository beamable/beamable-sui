
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/UpgradeItemRequestArgsLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UUpgradeItemRequestArgsLibrary::UpgradeItemRequestArgsToJsonString(const UUpgradeItemRequestArgs* Serializable, const bool Pretty)
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

UUpgradeItemRequestArgs* UUpgradeItemRequestArgsLibrary::Make(FString ContentId, int64 ItemInstanceId, FString NewLevel, UObject* Outer)
{
	auto Serializable = NewObject<UUpgradeItemRequestArgs>(Outer);
	Serializable->ContentId = ContentId;
	Serializable->ItemInstanceId = ItemInstanceId;
	Serializable->NewLevel = NewLevel;
	
	return Serializable;
}

void UUpgradeItemRequestArgsLibrary::Break(const UUpgradeItemRequestArgs* Serializable, FString& ContentId, int64& ItemInstanceId, FString& NewLevel)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		ContentId = Serializable->ContentId;
		ItemInstanceId = Serializable->ItemInstanceId;
		NewLevel = Serializable->NewLevel;
	}
		
}

