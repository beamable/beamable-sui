// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Content/BeamContentTypes/BeamItemContent.h"
#include "SuiNftBase.h"
#include "SuiWeaponItems.generated.h"

/**
 * 
 */
UCLASS(BlueprintType)
class UNREALSUISAMPLE_API USuiWeaponItems : public UBeamItemContent , public ISuiNftBase
{
	GENERATED_BODY()
	
public:
	UFUNCTION()
	void GetContentType_USuiWeaponItems(FString& Result){ Result = TEXT("weapon"); }

	UPROPERTY(EditAnywhere,BlueprintReadWrite)
	FName name;
	UPROPERTY(EditAnywhere,BlueprintReadWrite)
	FString description;
	UPROPERTY(EditAnywhere,BlueprintReadWrite)
	FString image;
	UPROPERTY(EditAnywhere,BlueprintReadWrite)
	TMap<FString, FString> customProperties;

	// ISuiNftBase implementations
	virtual FString GetName_Implementation() const override { return name.ToString(); }
	virtual FString GetDescription_Implementation() const override { return description; }
	virtual FString GetImage_Implementation() const override { return image; }
	virtual TMap<FString,FString> GetCustomProperties_Implementation() const override { return customProperties; }
};