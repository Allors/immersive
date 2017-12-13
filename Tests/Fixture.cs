using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using Mono.Cecil;

using NUnit.Framework;

public static class Fixture
{
    public static string BeforeAssemblyPath;
    public static readonly Assembly BeforeAssembly;

    public static string AfterAssemblyPath;
    public static readonly Assembly AfterAssembly;

    public static List<string> Infos;
    public static List<string> Errors;

    static Fixture()
    {
        Infos = new List<string>();
        Errors = new List<string>();

        BeforeAssemblyPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "AssemblyToProcess.dll");
        BeforeAssembly = Assembly.LoadFile(BeforeAssemblyPath);

        AfterAssemblyPath = BeforeAssemblyPath.Replace(".dll", "2.dll");
        File.Copy(BeforeAssemblyPath, AfterAssemblyPath, true);
       
        using (var assemblyResolver = new DefaultAssemblyResolver())
        {
            var directoryName = Path.GetDirectoryName(BeforeAssemblyPath);
            assemblyResolver.AddSearchDirectory(directoryName);

            using (var moduleDefinition = ModuleDefinition.ReadModule(BeforeAssemblyPath))
            {
                var weavingTask = new ModuleWeaver
                                      {
                                          ModuleDefinition = moduleDefinition,
                                          AssemblyResolver = assemblyResolver,
                                          LogInfo = v =>
                                              {
                                                  Infos.Add(v);
                                                  Trace.TraceInformation(v);
                                              },
                                          LogError = v =>
                                              {
                                                  Errors.Add(v);
                                                  Trace.TraceError(v);
                                              },
                };

                weavingTask.Execute();
                moduleDefinition.Write(AfterAssemblyPath);
            }
        }

        AfterAssembly = Assembly.LoadFile(AfterAssemblyPath);
    }
}