using System;

using Mono.Cecil;

public class ModuleWeaver
{
    // Will log an informational message to MSBuild
    public Action<string> LogInfo { get; set; }

    // An instance of Mono.Cecil.ModuleDefinition for processing
    public ModuleDefinition ModuleDefinition { get; set; }

    public TypeSystem TypeSystem { get; set; }

    // Init logging delegates to make testing easier
    public ModuleWeaver()
    {
        this.LogInfo = m => { };
    }

    public void Execute()
    {
        this.TypeSystem = this.ModuleDefinition.TypeSystem;

        this.LogInfo("Immersed library.");
    }
}