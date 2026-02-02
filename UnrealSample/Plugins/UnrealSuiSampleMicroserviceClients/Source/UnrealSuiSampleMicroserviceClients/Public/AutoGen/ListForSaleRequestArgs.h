#pragma once

#include "CoreMinimal.h"

#include "Serialization/BeamJsonSerializable.h"
#include "Serialization/BeamJsonUtils.h"

#include "ListForSaleRequestArgs.generated.h"

UCLASS(BlueprintType, Category="Beam", DefaultToInstanced, EditInlineNew)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UListForSaleRequestArgs : public UObject, public IBeamJsonSerializableUObject
{
	GENERATED_BODY()

public:
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Item Id", Category="Beam")
	int64 ItemId = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Price", Category="Beam")
	int64 Price = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Optional Kiosk Content Id", Category="Beam")
	FString OptionalKioskContentId = {};

	

	virtual void BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const override;
	virtual void BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const override;
	virtual void BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag) override;
	
};