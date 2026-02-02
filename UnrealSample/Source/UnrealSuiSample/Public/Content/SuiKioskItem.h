// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Content/BeamContentTypes/BeamItemContent.h"
#include "SuiKioskItem.generated.h"
UCLASS(BlueprintType)
class UNREALSUISAMPLE_API USuiKioskItem: public UBeamContentObject
{
	GENERATED_BODY()
	
public:
	UFUNCTION()
	void GetContentType_USuiKioskItem(FString& Result){ Result = TEXT("kiosk"); }

	UPROPERTY(EditAnywhere,BlueprintReadWrite)
	FName kioskName;
	UPROPERTY(EditAnywhere,BlueprintReadWrite)
	FString itemType;
	UPROPERTY(EditAnywhere,BlueprintReadWrite)
	FString currencySymbol;

	// ISuiNftBase implementations
	virtual FString GetName_Implementation() const { return kioskName.ToString(); }
	virtual FString GetItemType_Implementation() const { return itemType; }
	virtual FString GetCurrencySymbol_Implementation() const { return currencySymbol; }
};
