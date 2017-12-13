using NUnit.Framework;

[TestFixture]
public class AssemblyTests
{
    [Test]
    public void PeVerify()
    {
#if (DEBUG && NET452)
        Verifier.Verify(Fixture.BeforeAssemblyPath, Fixture.AfterAssemblyPath);
#endif
    }
}