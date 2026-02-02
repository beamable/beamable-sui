#pragma once

#include "CoreMinimal.h"

#include "Serialization/BeamJsonSerializable.h"
#include "Serialization/BeamJsonUtils.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/ItemProperties.h"

#include "KioskListing.generated.h"

UCLASS(BlueprintType, Category="Beam", DefaultToInstanced, EditInlineNew)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UKioskListing : public UObject, public IBeamJsonSerializableUObject
{
	GENERATED_BODY()

public:
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Listing Id", Category="Beam")
	FString ListingId = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Kiosk Content Id", Category="Beam")
	FString KioskContentId = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Kiosk Name", Category="Beam")
	FString KioskName = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Item Content Id", Category="Beam")
	FString ItemContentId = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Item Proxy Id", Category="Beam")
	FString ItemProxyId = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Item Inventory Id", Category="Beam")
	int64 ItemInventoryId = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Price Content Id", Category="Beam")
	FString PriceContentId = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Price", Category="Beam")
	int64 Price = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Gamer Tag", Category="Beam")
	int64 GamerTag = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Wallet", Category="Beam")
	FString Wallet = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Created At", Category="Beam")
	FDateTime CreatedAt = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Status", Category="Beam")
	FString Status = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Properties", Category="Beam")
	TArray<UItemProperties*> Properties = {};

	

	virtual void BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const override;
	virtual void BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const override;
	virtual void BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag) override;
	
};