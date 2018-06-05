using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Fody;
using Xunit;
#pragma warning disable 618

namespace Immersive.Tests
{
    public static class Fixture
    {
        public static TestResult TestResult;

        public static Assembly BeforeAssembly;

        static Fixture()
        {
            var weavingTask = new ModuleWeaver();
            TestResult = weavingTask.ExecuteTestRun("AssemblyToProcess.dll");

            var path = new DirectoryInfo(".").GetFiles("AssemblyToProcess.dll").First().FullName;
            BeforeAssembly = Assembly.LoadFrom(path);
        }

    }
}
