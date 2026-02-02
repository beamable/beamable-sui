
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/PlayerIdentityLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UPlayerIdentityLibrary::PlayerIdentityToJsonString(const UPlayerIdentity* Serializable, const bool Pretty)
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

UPlayerIdentity* UPlayerIdentityLibrary::Make(FString Microservice, FString NameSpace, FString Address, FString Type, UObject* Outer)
{
	auto Serializable = NewObject<UPlayerIdentity>(Outer);
	Serializable->Microservice = Microservice;
	Serializable->NameSpace = NameSpace;
	Serializable->Address = Address;
	Serializable->Type = Type;
	
	return Serializable;
}

void UPlayerIdentityLibrary::Break(const UPlayerIdentity* Serializable, FString& Microservice, FString& NameSpace, FString& Address, FString& Type)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		Microservice = Serializable->Microservice;
		NameSpace = Serializable->NameSpace;
		Address = Serializable->Address;
		Type = Serializable->Type;
	}
		
}

