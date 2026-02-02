
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/GetFederationInfoResponseLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UGetFederationInfoResponseLibrary::GetFederationInfoResponseToJsonString(const UGetFederationInfoResponse* Serializable, const bool Pretty)
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

UGetFederationInfoResponse* UGetFederationInfoResponseLibrary::Make(int64 GamerTag, FString Email, TArray<UPlayerIdentity*> ExternalIdentities, UObject* Outer)
{
	auto Serializable = NewObject<UGetFederationInfoResponse>(Outer);
	Serializable->GamerTag = GamerTag;
	Serializable->Email = Email;
	Serializable->ExternalIdentities = ExternalIdentities;
	
	return Serializable;
}

void UGetFederationInfoResponseLibrary::Break(const UGetFederationInfoResponse* Serializable, int64& GamerTag, FString& Email, TArray<UPlayerIdentity*>& ExternalIdentities)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		GamerTag = Serializable->GamerTag;
		Email = Serializable->Email;
		ExternalIdentities = Serializable->ExternalIdentities;
	}
		
}

