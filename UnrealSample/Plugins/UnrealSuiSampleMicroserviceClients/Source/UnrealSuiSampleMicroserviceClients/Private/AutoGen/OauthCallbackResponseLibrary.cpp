
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/OauthCallbackResponseLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UOauthCallbackResponseLibrary::OauthCallbackResponseToJsonString(const UOauthCallbackResponse* Serializable, const bool Pretty)
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

UOauthCallbackResponse* UOauthCallbackResponseLibrary::Make(FString Message, UObject* Outer)
{
	auto Serializable = NewObject<UOauthCallbackResponse>(Outer);
	Serializable->Message = Message;
	
	return Serializable;
}

void UOauthCallbackResponseLibrary::Break(const UOauthCallbackResponse* Serializable, FString& Message)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		Message = Serializable->Message;
	}
		
}

