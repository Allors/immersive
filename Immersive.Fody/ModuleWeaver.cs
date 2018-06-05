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
        var substitutableAssembly = new SubstitutableAssembly(this.ModuleDefinition);

        var immersiveType = this.ModuleDefinition.Types.FirstOrDefault(v => v.Name.Equals("ImmersiveMarker") && v.IsClass);
        if (immersiveType == null)
        {
            this.LogInfo("No immersive assembly found");
        }
        else
        {
            var references = this.ModuleDefinition.AssemblyReferences
                .Select(v => this.ResolveAssembly(v.FullName).MainModule)
                .ToArray();

            var module = references.FirstOrDefault(v => v.Types.Any(w => w.FullName.Equals(immersiveType.BaseType.FullName)));
            var substitutes = new Substitutes(module);

            substitutableAssembly.Substitute(substitutes);

            this.LogInfo(module?.Name + " immersed in " + this.ModuleDefinition.Name + ".");
        }
    }
}