using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using NUnit.Framework;

namespace AwesomeExtensions.Tests;

[TestFixture]
public class GenerateExtensionMethodQuickFixTest : QuickFixTestBase<GenerateExtensionMethodQuickFix>
{
    protected override string RelativeTestDataPath => @"QuickFixes\GenerateExtensionMethod";

    [Test]
    public void Test01()
    {
        DoTestFiles("test01.cs");
    }

    [Test]
    public void Test02()
    {
        DoTestFiles("test02.cs");
    }
}
