// Copyright Epic Games, Inc. All Rights Reserved.

using System.IO;
using UnrealBuildTool;

public class UnrealSuiSample : ModuleRules
{
	public UnrealSuiSample(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;
        
        		string compactLibInclude = Path.Combine(ModuleDirectory, "ThirdParty", "Compact25519/");
        		string blake2bLibInclude = Path.Combine(ModuleDirectory, "ThirdParty", "Blake2b/");
        		string libsodiumLibInclude = Path.Combine(ModuleDirectory, "ThirdParty", "libsodium/");
        
        		PublicIncludePaths.AddRange(new string[]
        		{
        			compactLibInclude,
        			blake2bLibInclude
        		});
        
        
        		PublicDependencyModuleNames.AddRange(
        			new string[]
        			{
        				"Core",
				        "BeamableCore"
        			}
        		);
        
        		PrivateDependencyModuleNames.AddRange(
        			new string[]
        			{
        				"CoreUObject",
        				"Engine",
        				"Slate",
        				"SlateCore",
        				// ... add private dependencies that you statically link with here ...	
        			}
        		);
        
        		UnrealSuiSampleMicroserviceClients.AddMicroserviceClients(this);
        		Beam.AddRuntimeModuleDependencies(this);
	}
}
