using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.CSharp.Errors;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;
using JetBrains.Util.Logging;

namespace AwesomeExtensions;

[QuickFix]
public class GenerateExtensionMethodQuickFix(NotResolvedError highlighting) : QuickFixBase
{
    private readonly IReference _reference = highlighting.Reference;

    [UsedImplicitly]
    public const string Id = "GenerateExtensionMethodQuickFix";
    
    protected override Action<ITextControl> ExecutePsiTransaction(
        ISolution solution,
        IProgressIndicator progress)
    {
        var treeNode = _reference.GetTreeNode();
        var factory = CSharpElementFactory.GetInstance(treeNode);
        var methodName = _reference.GetName();

        if (treeNode is not IReferenceExpression referenceExpression)
            return null;

        var qualifierExpression = referenceExpression.QualifierExpression;
        if (qualifierExpression == null)
            return null;

        var targetTypeNameWithDefaultStyle = qualifierExpression.GetExpressionType().GetPresentableName(CSharpLanguage.Instance, TypePresentationStyle.Default);
        var targetTypeNameWithNoStyle = qualifierExpression.Type().GetPresentableName(CSharpLanguage.Instance, new TypePresentationStyle { Options = 0 });

        var statement = factory.CreateStatement("throw new NotImplementedException();");
        var methodBody = factory.CreateEmptyBlock();
        methodBody.AddStatementBefore(statement, null);
        var methodDeclaration = factory
            .CreateTypeMemberDeclaration($"public static void {methodName}(this {targetTypeNameWithDefaultStyle} self)") as IMethodDeclaration;
        methodDeclaration?.SetBody(methodBody);

        var classDeclaration = factory
            .CreateTypeMemberDeclaration($"public static class {targetTypeNameWithNoStyle}Extensions") as IClassDeclaration;

        if (classDeclaration != null && methodDeclaration != null)
            classDeclaration.AddClassMemberDeclaration(methodDeclaration);

        if (treeNode.GetContainingFile() is not ICSharpFile file)
            return null;

        var namespaceDeclaration = file.Children<ICSharpNamespaceDeclaration>().FirstOrDefault();

        if (namespaceDeclaration != null)
        {
            namespaceDeclaration.AddTypeDeclarationAfter(classDeclaration, file.TypeDeclarations.LastOrDefault());
        }
        else
        {
            file.AddTypeDeclarationAfter(classDeclaration, file.TypeDeclarations.LastOrDefault());
        }

        return null;
    }

    public override string Text => "Generate extension method";

    public override bool IsAvailable(IUserDataHolder cache)
    {
        return _reference.IsValid();
    }
}
