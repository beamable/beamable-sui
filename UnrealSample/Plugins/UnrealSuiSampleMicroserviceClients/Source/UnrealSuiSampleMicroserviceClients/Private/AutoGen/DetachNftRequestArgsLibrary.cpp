
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/DetachNftRequestArgsLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UDetachNftRequestArgsLibrary::DetachNftRequestArgsToJsonString(const UDetachNftRequestArgs* Serializable, const bool Pretty)
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

UDetachNftRequestArgs* UDetachNftRequestArgsLibrary::Make(FString ParentId, int64 ParentsInstanceId, FString NftId, UObject* Outer)
{
	auto Serializable = NewObject<UDetachNftRequestArgs>(Outer);
	Serializable->ParentId = ParentId;
	Serializable->ParentsInstanceId = ParentsInstanceId;
	Serializable->NftId = NftId;
	
	return Serializable;
}

void UDetachNftRequestArgsLibrary::Break(const UDetachNftRequestArgs* Serializable, FString& ParentId, int64& ParentsInstanceId, FString& NftId)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		ParentId = Serializable->ParentId;
		ParentsInstanceId = Serializable->ParentsInstanceId;
		NftId = Serializable->NftId;
	}
		
}

