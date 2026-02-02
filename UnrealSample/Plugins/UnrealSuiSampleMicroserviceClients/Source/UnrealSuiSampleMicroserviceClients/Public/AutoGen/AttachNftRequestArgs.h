#pragma once

#include "CoreMinimal.h"

#include "Serialization/BeamJsonSerializable.h"
#include "Serialization/BeamJsonUtils.h"

#include "AttachNftRequestArgs.generated.h"

UCLASS(BlueprintType, Category="Beam", DefaultToInstanced, EditInlineNew)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UAttachNftRequestArgs : public UObject, public IBeamJsonSerializableUObject
{
	GENERATED_BODY()

public:
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Parent Id", Category="Beam")
	FString ParentId = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Parents Instance Id", Category="Beam")
	int64 ParentsInstanceId = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Nft Id", Category="Beam")
	FString NftId = {};

	

	virtual void BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const override;
	virtual void BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const override;
	virtual void BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag) override;
	
};