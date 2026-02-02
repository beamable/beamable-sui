// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Content/SuiWeaponItems.h"
#include "SuiNftAddon.h"
#include "LaserModItem.generated.h"

/**
 * 
 */
UCLASS(BlueprintType)
class UNREALSUISAMPLE_API ULaserModItem : public USuiWeaponItems, public ISuiNftAddon
{
	GENERATED_BODY()
		
public:
	UFUNCTION()
	void GetContentType_ULaserModItems(FString& Result){ Result = TEXT("lasermoditem"); }

};
