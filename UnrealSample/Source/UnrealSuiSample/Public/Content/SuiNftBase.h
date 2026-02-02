#pragma once
#include "UObject/Interface.h"
#include "SuiNftBase.generated.h"

UINTERFACE(BlueprintType)
class UNREALSUISAMPLE_API USuiNftBase : public UInterface
{
	GENERATED_BODY()
};

/** Unreal analogue of C# INftBase */
class UNREALSUISAMPLE_API ISuiNftBase
{
	GENERATED_BODY()
public:
	UFUNCTION(BlueprintCallable, BlueprintNativeEvent, Category="Sui|NFT")
	FString GetName() const;

	UFUNCTION(BlueprintCallable, BlueprintNativeEvent, Category="Sui|NFT")
	FString GetDescription() const;

	UFUNCTION(BlueprintCallable, BlueprintNativeEvent, Category="Sui|NFT")
	FString GetImage() const;

	UFUNCTION(BlueprintCallable, BlueprintNativeEvent, Category="Sui|NFT")
	TMap<FString, FString> GetCustomProperties() const;
};
