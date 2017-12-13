using System;

using AssemblyToProcess;

using NUnit.Framework;

[TestFixture]
public class MethodTests
{
    [Test]
    public void SubstituteWithType()
    {
        var type = Fixture.AfterAssembly.GetType("AssemblyToProcess.TestForm");

        Assert.AreEqual("Referenced: Show(False)", type.GetMethod("ShowMessageBox", new[] { typeof(bool) }).Invoke(null, new object[] { false }));

        Assert.AreEqual("Substitute: Referenced: Show(Test)", type.GetMethod("ShowMessageBox", new[] { typeof(string) }).Invoke(null, new object[] { "Test" }));
        Assert.AreEqual("Substitute: Referenced: Show(0)", type.GetMethod("ShowMessageBox", new[] { typeof(int) }).Invoke(null, new object[] { 0 }));
        Assert.AreEqual("Substitute: Referenced: Show(Test 0)", type.GetMethod("ShowMessageBox", new[] { typeof(string), typeof(int) }).Invoke(null, new object[] { "Test", 0 }));

        Assert.AreEqual("Substitute: Referenced: Show2(Test)", type.GetMethod("ShowMessageBox2", new[] { typeof(string) }).Invoke(null, new object[] { "Test" }));
        Assert.AreEqual("Substitute: Referenced: Show2(0)", type.GetMethod("ShowMessageBox2", new[] { typeof(int) }).Invoke(null, new object[] { 0 }));
        Assert.AreEqual("Substitute: Referenced: Show2(Test 0)", type.GetMethod("ShowMessageBox2", new[] { typeof(string), typeof(int) }).Invoke(null, new object[] { "Test", 0 }));
    }

    [Test]
    public void SubstituteWithTypeAndMethodName()
    {
        var type = Fixture.AfterAssembly.GetType("AssemblyToProcess.TestForm");
        var instance = (dynamic)Activator.CreateInstance(type);

        Assert.AreEqual("Substitute: Referenced: ShowDialog()", instance.CallShowDialog());
    }
}