#pragma once

#include "CoreMinimal.h"

#include "Serialization/BeamJsonSerializable.h"
#include "Serialization/BeamJsonUtils.h"

#include "KioskResponse.generated.h"

UCLASS(BlueprintType, Category="Beam", DefaultToInstanced, EditInlineNew)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UKioskResponse : public UObject, public IBeamJsonSerializableUObject
{
	GENERATED_BODY()

public:
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Kiosk Content Id", Category="Beam")
	FString KioskContentId = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Item Content Type", Category="Beam")
	FString ItemContentType = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Price Content Id", Category="Beam")
	FString PriceContentId = {};

	

	virtual void BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const override;
	virtual void BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const override;
	virtual void BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag) override;
	
};