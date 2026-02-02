using System.IO;
using UnityEditor;
using UnityEngine;
using Mono.Cecil;
using Mono.Cecil.Cil;

public static class FindBadFieldRefs
{
    [MenuItem("Tools/Diagnostics/Find ScriptableObject::name field refs")]
    public static void Run()
    {
        var dirs = new[]
        {
            Path.Combine(Application.dataPath, "Plugins"),
            Path.Combine(Application.dataPath, "Generated"), // just in case
            Path.Combine(Directory.GetCurrentDirectory(), "Library/ScriptAssemblies")
        };

        foreach (var dir in dirs)
        {
            if (!Directory.Exists(dir)) continue;
            foreach (var dll in Directory.GetFiles(dir, "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    using var asm = AssemblyDefinition.ReadAssembly(dll, new ReaderParameters { ReadSymbols = false });
                    foreach (var module in asm.Modules)
                    foreach (var type in module.Types)
                    foreach (var method in type.Methods)
                    {
                        if (!method.HasBody) continue;
                        foreach (var ins in method.Body.Instructions)
                        {
                            if (ins.OpCode != OpCodes.Ldfld && ins.OpCode != OpCodes.Stfld) continue;
                            if (ins.Operand is FieldReference fref &&
                                fref.DeclaringType.FullName == "UnityEngine.ScriptableObject" &&
                                fref.FieldType.FullName == "System.String" &&
                                fref.Name == "name")
                            {
                                Debug.LogError($"BAD FIELD REF in {dll}\n  {type.FullName}::{method.Name}  -->  {fref.FullName}");
                            }
                        }
                    }
                }
                catch { /* ignore non-CLR files */ }
            }
        }
        Debug.Log("Scan complete.");
    }
}
