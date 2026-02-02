#pragma once
#include "UObject/Interface.h"
#include "SuiNftAddon.generated.h"

UINTERFACE(BlueprintType)
class UNREALSUISAMPLE_API USuiNftAddon : public UInterface
{
	GENERATED_BODY()
};

/** Marker like C# INftAddon */
class UNREALSUISAMPLE_API ISuiNftAddon
{
	GENERATED_BODY()
	// No methods
};
