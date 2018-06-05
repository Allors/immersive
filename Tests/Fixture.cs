using System.IO;
using System.Linq;
using System.Reflection;
using Fody;

#pragma warning disable 618

namespace Immersive.Tests
{
    using System;

    public static class Fixture
    {
        public static ModuleWeaver ModuleWeaver;

        public static TestResult TestResult;

        public static Assembly BeforeAssembly;

        static Fixture()
        {
            ModuleWeaver = new ModuleWeaver { LogInfo = Console.WriteLine };
            TestResult = ModuleWeaver.ExecuteTestRun("AssemblyToProcess.dll");

            var path = new DirectoryInfo(".").GetFiles("AssemblyToProcess.dll").First().FullName;
            BeforeAssembly = Assembly.LoadFrom(path);
        }

    }
}
