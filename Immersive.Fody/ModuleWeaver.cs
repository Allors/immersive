using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Immersive.Fody;
using Mono.Cecil;

public class ModuleWeaver
{
    public XElement Config { get; set; }
    
    public Action<string> LogInfo { get; set; }

    public Action<string> LogError { get; set; }

    public IAssemblyResolver AssemblyResolver { get; set; }

    public ModuleDefinition ModuleDefinition { get; set; }

    public TypeSystem TypeSystem { get; set; }

    // Init logging delegates to make testing easier
    public ModuleWeaver()
    {
        this.LogInfo = m => { };
        this.LogError = m => { };
    }

    public void Execute()
    {
        this.TypeSystem = this.ModuleDefinition.TypeSystem;

        var substitutableAssembly = new SubstitutableAssembly(this.ModuleDefinition);

        var immersiveType = this.ModuleDefinition.Types.FirstOrDefault(v => v.Name.Equals("ImmersiveMarker") && v.IsClass);
        if (immersiveType == null)
        {
            this.LogInfo("No immersive assembly found");
        }
        else
        {
            var references = this.ModuleDefinition.AssemblyReferences
                .Select(v => this.AssemblyResolver.Resolve(v).MainModule)
                .ToArray();

            var module = references.FirstOrDefault(v => v.Types.Any(w => w.FullName.Equals(immersiveType.BaseType.FullName)));
            var substitutes = new Substitutes(module);

            substitutableAssembly.Substitute(substitutes);

            this.LogInfo(module.Name + " immersed in " + this.ModuleDefinition.Name + ".");
        }
    }
}