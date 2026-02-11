using System.Threading;
using AwesomeExtensions;
using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;
using NUnit.Framework;

[assembly: Apartment(ApartmentState.STA)]

namespace ThreeMonkeys.AwesomeExtensions.Tests
{
    [ZoneDefinition]
    public class AwesomeExtensionsTestEnvironmentZone : ITestsEnvZone, IRequire<PsiFeatureTestZone>, IRequire<IAwesomeExtensionsZone> { }

    [ZoneMarker]
    public class ZoneMarker : IRequire<ICodeEditingZone>, IRequire<ILanguageCSharpZone>, IRequire<AwesomeExtensionsTestEnvironmentZone> { }

    [SetUpFixture]
    public class AwesomeExtensionsTestsAssembly : ExtensionTestEnvironmentAssembly<AwesomeExtensionsTestEnvironmentZone> { }
}
