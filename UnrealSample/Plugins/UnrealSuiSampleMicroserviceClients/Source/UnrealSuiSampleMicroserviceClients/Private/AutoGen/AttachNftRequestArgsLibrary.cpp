
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/AttachNftRequestArgsLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UAttachNftRequestArgsLibrary::AttachNftRequestArgsToJsonString(const UAttachNftRequestArgs* Serializable, const bool Pretty)
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

UAttachNftRequestArgs* UAttachNftRequestArgsLibrary::Make(FString ParentId, int64 ParentsInstanceId, FString NftId, UObject* Outer)
{
	auto Serializable = NewObject<UAttachNftRequestArgs>(Outer);
	Serializable->ParentId = ParentId;
	Serializable->ParentsInstanceId = ParentsInstanceId;
	Serializable->NftId = NftId;
	
	return Serializable;
}

void UAttachNftRequestArgsLibrary::Break(const UAttachNftRequestArgs* Serializable, FString& ParentId, int64& ParentsInstanceId, FString& NftId)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		ParentId = Serializable->ParentId;
		ParentsInstanceId = Serializable->ParentsInstanceId;
		NftId = Serializable->NftId;
	}
		
}

