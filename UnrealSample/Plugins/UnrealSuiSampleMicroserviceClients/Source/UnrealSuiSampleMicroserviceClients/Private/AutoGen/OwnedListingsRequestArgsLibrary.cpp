
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/OwnedListingsRequestArgsLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UOwnedListingsRequestArgsLibrary::OwnedListingsRequestArgsToJsonString(const UOwnedListingsRequestArgs* Serializable, const bool Pretty)
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

UOwnedListingsRequestArgs* UOwnedListingsRequestArgsLibrary::Make(FString OptionalKioskContentId, UObject* Outer)
{
	auto Serializable = NewObject<UOwnedListingsRequestArgs>(Outer);
	Serializable->OptionalKioskContentId = OptionalKioskContentId;
	
	return Serializable;
}

void UOwnedListingsRequestArgsLibrary::Break(const UOwnedListingsRequestArgs* Serializable, FString& OptionalKioskContentId)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		OptionalKioskContentId = Serializable->OptionalKioskContentId;
	}
		
}

