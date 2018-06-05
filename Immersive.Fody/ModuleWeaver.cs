using System;
using System.Collections.Generic;
using System.Linq;
using Fody;
using Immersive.Fody;

public class ModuleWeaver : BaseModuleWeaver
{
    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "netstandard";
        yield return "mscorlib";
    }

    public override void Execute()
    {
        var substitutableAssembly = new Substitutables(this);

        var immersiveType = this.ModuleDefinition.Types.FirstOrDefault(v => v.Name.Equals("ImmersiveMarker") && v.IsClass);
        if (immersiveType == null)
        {
            this.LogInfo("No immersive assembly found");
        }
        else
        {
            var module = immersiveType.BaseType.Scope.Name;
            var moduleDefinition = this.ResolveAssembly(module).MainModule;
            var substitutes = new Substitutes(this, moduleDefinition);

            substitutableAssembly.Substitute(substitutes);

            this.LogInfo(moduleDefinition?.Name + " immersed in " + this.ModuleDefinition.Name + ".");
        }
    }
}