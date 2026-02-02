
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/AllListingsRequestArgsLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UAllListingsRequestArgsLibrary::AllListingsRequestArgsToJsonString(const UAllListingsRequestArgs* Serializable, const bool Pretty)
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

UAllListingsRequestArgs* UAllListingsRequestArgsLibrary::Make(FString OptionalKioskContentId, int32 Page, int32 PageSize, UObject* Outer)
{
	auto Serializable = NewObject<UAllListingsRequestArgs>(Outer);
	Serializable->OptionalKioskContentId = OptionalKioskContentId;
	Serializable->Page = Page;
	Serializable->PageSize = PageSize;
	
	return Serializable;
}

void UAllListingsRequestArgsLibrary::Break(const UAllListingsRequestArgs* Serializable, FString& OptionalKioskContentId, int32& Page, int32& PageSize)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		OptionalKioskContentId = Serializable->OptionalKioskContentId;
		Page = Serializable->Page;
		PageSize = Serializable->PageSize;
	}
		
}

