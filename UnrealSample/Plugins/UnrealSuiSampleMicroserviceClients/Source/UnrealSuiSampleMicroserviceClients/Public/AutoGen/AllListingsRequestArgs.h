#pragma once

#include "CoreMinimal.h"

#include "Serialization/BeamJsonSerializable.h"
#include "Serialization/BeamJsonUtils.h"

#include "AllListingsRequestArgs.generated.h"

UCLASS(BlueprintType, Category="Beam", DefaultToInstanced, EditInlineNew)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UAllListingsRequestArgs : public UObject, public IBeamJsonSerializableUObject
{
	GENERATED_BODY()

public:
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Optional Kiosk Content Id", Category="Beam")
	FString OptionalKioskContentId = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Page", Category="Beam")
	int32 Page = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Page Size", Category="Beam")
	int32 PageSize = {};

	

	virtual void BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const override;
	virtual void BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const override;
	virtual void BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag) override;
	
};