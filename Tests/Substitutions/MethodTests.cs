using System;
using Xunit;

namespace Immersive.Tests
{
    public class MethodTests
    {
        [Fact]
        public void SubstituteWithType()
        {
            var type = Fixture.TestResult.Assembly.GetType("AssemblyToProcess.TestForm");

            Assert.Equal("Referenced: Show(False)", type.GetMethod("ShowMessageBox", new[] { typeof(bool) }).Invoke(null, new object[] { false }));

            Assert.Equal("Substitute: Referenced: Show(Test)", type.GetMethod("ShowMessageBox", new[] { typeof(string) }).Invoke(null, new object[] { "Test" }));
            Assert.Equal("Substitute: Referenced: Show(0)", type.GetMethod("ShowMessageBox", new[] { typeof(int) }).Invoke(null, new object[] { 0 }));
            Assert.Equal("Substitute: Referenced: Show(Test 0)", type.GetMethod("ShowMessageBox", new[] { typeof(string), typeof(int) }).Invoke(null, new object[] { "Test", 0 }));

            Assert.Equal("Substitute: Referenced: Show2(Test)", type.GetMethod("ShowMessageBox2", new[] { typeof(string) }).Invoke(null, new object[] { "Test" }));
            Assert.Equal("Substitute: Referenced: Show2(0)", type.GetMethod("ShowMessageBox2", new[] { typeof(int) }).Invoke(null, new object[] { 0 }));
            Assert.Equal("Substitute: Referenced: Show2(Test 0)", type.GetMethod("ShowMessageBox2", new[] { typeof(string), typeof(int) }).Invoke(null, new object[] { "Test", 0 }));
        }

        [Fact]
        public void SubstituteWithTypeAndMethodName()
        {
            var type = Fixture.TestResult.Assembly.GetType("AssemblyToProcess.TestForm");
            var instance = (dynamic)Activator.CreateInstance(type);

            Assert.Equal("Substitute: Referenced: ShowDialog()", instance.CallShowDialog());
        }
    }
}