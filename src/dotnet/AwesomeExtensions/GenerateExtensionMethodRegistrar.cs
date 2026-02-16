using System;
using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.Application.Parts;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.CSharp.Errors;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.Util;

namespace AwesomeExtensions;

[SolutionComponent(Instantiation.DemandAnyThreadSafe)]
public class GenerateExtensionMethodRegistrar : IQuickFixesProvider
{
    public void Register(IQuickFixesRegistrar registrar)
    {
        registrar.RegisterQuickFix<NotResolvedError>(
            Lifetime.Eternal,
            h => new GenerateExtensionMethodQuickFix(h.Reference),
            typeof(GenerateExtensionMethodQuickFix));
    }

    public IEnumerable<Type> Dependencies => EmptyArray<Type>.Instance;
}