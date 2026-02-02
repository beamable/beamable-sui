#pragma once

#include "CoreMinimal.h"
#include "BeamBackend/BeamBaseResponseBodyInterface.h"
#include "Serialization/BeamJsonSerializable.h"
#include "Serialization/BeamJsonUtils.h"
#include "UnrealSuiSampleMicroserviceClients/Public/AutoGen/KioskListing.h"

#include "KioskListingsResponsePaginated.generated.h"

UCLASS(BlueprintType, Category="Beam", DefaultToInstanced, EditInlineNew)
class UNREALSUISAMPLEMICROSERVICECLIENTS_API UKioskListingsResponsePaginated : public UObject, public IBeamJsonSerializableUObject, public IBeamBaseResponseBodyInterface
{
	GENERATED_BODY()

public:
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Page", Category="Beam")
	int32 Page = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Page Size", Category="Beam")
	int32 PageSize = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Total Count", Category="Beam")
	int64 TotalCount = {};
	UPROPERTY(EditAnywhere, BlueprintReadWrite, DisplayName="Listings", Category="Beam")
	TArray<UKioskListing*> Listings = {};

	virtual void DeserializeRequestResponse(UObject* RequestData, FString ResponseContent) override;

	virtual void BeamSerializeProperties(TUnrealJsonSerializer& Serializer) const override;
	virtual void BeamSerializeProperties(TUnrealPrettyJsonSerializer& Serializer) const override;
	virtual void BeamDeserializeProperties(const TSharedPtr<FJsonObject>& Bag) override;
	
};