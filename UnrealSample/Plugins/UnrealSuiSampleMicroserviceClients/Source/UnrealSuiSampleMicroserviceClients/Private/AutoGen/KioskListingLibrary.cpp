
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListingLibrary.h"

#include "CoreMinimal.h"
#include "BeamCoreSettings.h"


FString UKioskListingLibrary::KioskListingToJsonString(const UKioskListing* Serializable, const bool Pretty)
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

UKioskListing* UKioskListingLibrary::Make(FString ListingId, FString KioskContentId, FString KioskName, FString ItemContentId, FString ItemProxyId, int64 ItemInventoryId, FString PriceContentId, int64 Price, int64 GamerTag, FString Wallet, FDateTime CreatedAt, FString Status, TArray<UItemProperties*> Properties, UObject* Outer)
{
	auto Serializable = NewObject<UKioskListing>(Outer);
	Serializable->ListingId = ListingId;
	Serializable->KioskContentId = KioskContentId;
	Serializable->KioskName = KioskName;
	Serializable->ItemContentId = ItemContentId;
	Serializable->ItemProxyId = ItemProxyId;
	Serializable->ItemInventoryId = ItemInventoryId;
	Serializable->PriceContentId = PriceContentId;
	Serializable->Price = Price;
	Serializable->GamerTag = GamerTag;
	Serializable->Wallet = Wallet;
	Serializable->CreatedAt = CreatedAt;
	Serializable->Status = Status;
	Serializable->Properties = Properties;
	
	return Serializable;
}

void UKioskListingLibrary::Break(const UKioskListing* Serializable, FString& ListingId, FString& KioskContentId, FString& KioskName, FString& ItemContentId, FString& ItemProxyId, int64& ItemInventoryId, FString& PriceContentId, int64& Price, int64& GamerTag, FString& Wallet, FDateTime& CreatedAt, FString& Status, TArray<UItemProperties*>& Properties)
{
	if(GetDefault<UBeamCoreSettings>()->BreakGuard(Serializable))
	{
		ListingId = Serializable->ListingId;
		KioskContentId = Serializable->KioskContentId;
		KioskName = Serializable->KioskName;
		ItemContentId = Serializable->ItemContentId;
		ItemProxyId = Serializable->ItemProxyId;
		ItemInventoryId = Serializable->ItemInventoryId;
		PriceContentId = Serializable->PriceContentId;
		Price = Serializable->Price;
		GamerTag = Serializable->GamerTag;
		Wallet = Serializable->Wallet;
		CreatedAt = Serializable->CreatedAt;
		Status = Serializable->Status;
		Properties = Serializable->Properties;
	}
		
}

