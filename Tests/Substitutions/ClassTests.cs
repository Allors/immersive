using System;
using System.Reflection;

using NUnit.Framework;

[TestFixture]
public class ClassTests
{
    [Test]
    public void Default()
    {
        var type = Fixture.AfterAssembly.GetType("AssemblyToProcess.TestForm");
        var instance = (dynamic)Activator.CreateInstance(type);

        var constructorCalledFieldInfo = type.GetField("constructorCalled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
        var baseConstructorCalledFieldInfo = type.GetField("baseConstructorCalled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
        var assemblyConstructorCalledFieldInfo = type.GetField("assemblyConstructorCalled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);

        var button1FieldInfo = type.GetField("button1", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
        var textBox1FieldInfo = type.GetField("textBox1", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
        var nadaFieldInfo = type.GetField("nada", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
        var sealedSingleFieldInfo = type.GetField("sealedSingle", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
        var sealedHierarchyFieldInfo = type.GetField("sealedHierarchy", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

        Assert.AreEqual("Tests.Immersive.Form", instance.GetType().BaseType.FullName);

        var constructorCalled = (bool)constructorCalledFieldInfo.GetValue(instance);
        Assert.IsTrue(constructorCalled);

        var baseConstructorCalled = (bool)baseConstructorCalledFieldInfo.GetValue(instance);
        Assert.IsTrue(baseConstructorCalled);

        var assemblyConstructorCalled = (bool)assemblyConstructorCalledFieldInfo.GetValue(instance);
        Assert.IsTrue(assemblyConstructorCalled);

        Assert.AreEqual("Tests.Referenced.Button", button1FieldInfo.FieldType.FullName);
        object button1 = button1FieldInfo.GetValue(instance);
        Assert.AreEqual("Tests.Immersive.Button", button1.GetType().FullName);

        Assert.AreEqual("Tests.Referenced.TextBox", textBox1FieldInfo.FieldType.FullName);
        object textBox1 = textBox1FieldInfo.GetValue(instance);
        Assert.AreEqual("Tests.Referenced.TextBox", textBox1.GetType().FullName);

        Assert.AreEqual("Tests.Referenced.Nada", nadaFieldInfo.FieldType.FullName);
        object nada = nadaFieldInfo.GetValue(instance);
        Assert.AreEqual("Tests.Referenced.Nada", nada.GetType().FullName);

        Assert.AreEqual("Tests.Immersive.SealedSingle", sealedSingleFieldInfo.FieldType.FullName);
        object sealedSingle = sealedSingleFieldInfo.GetValue(instance);
        Assert.AreEqual("Tests.Immersive.SealedSingle", sealedSingle.GetType().FullName);

        Assert.AreEqual("Tests.Immersive.SealedHierarchy", sealedHierarchyFieldInfo.FieldType.FullName);
        object sealedHierarchy = sealedHierarchyFieldInfo.GetValue(instance);
        Assert.AreEqual("Tests.Immersive.SealedHierarchy", sealedHierarchy.GetType().FullName);
    }
}