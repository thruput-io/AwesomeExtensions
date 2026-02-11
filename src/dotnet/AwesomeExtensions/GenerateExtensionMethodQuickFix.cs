using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace AwesomeExtensions;

[QuickFix]
public class GenerateExtensionMethodQuickFix(IReference reference) : QuickFixBase
{
    protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
    {
        var factory = CSharpElementFactory.GetInstance(reference.GetTreeNode());
        var methodName = reference.GetName();
        
        var referenceExpression = reference.GetTreeNode() as IReferenceExpression;
        var targetTypeNameWithDefaultStyle = referenceExpression.QualifierExpression.GetExpressionType().GetPresentableName(CSharpLanguage.Instance, TypePresentationStyle.Default);
        var targetTypeNameWithNoStyle = referenceExpression.QualifierExpression.Type().GetPresentableName(CSharpLanguage.Instance, new TypePresentationStyle { Options = 0 });

        var statement = factory.CreateStatement("throw new NotImplementedException();");
        var methodBody = factory.CreateEmptyBlock();
        methodBody.AddStatementBefore(statement, null);
        var methodDeclaration = factory
            .CreateTypeMemberDeclaration($"public static void {methodName}(this {targetTypeNameWithDefaultStyle} self)") as IMethodDeclaration;
        methodDeclaration.SetBody(methodBody);

        var classDeclaration = factory
            .CreateTypeMemberDeclaration($"public static class {targetTypeNameWithNoStyle}Extensions") as IClassDeclaration;
        
        classDeclaration.AddClassMemberDeclaration(methodDeclaration);
        
        var file = reference.GetTreeNode().GetContainingFile() as ICSharpFile;
        var namespaceDeclaration = file.Children<ICSharpNamespaceDeclaration>().FirstNotNull();
        
        namespaceDeclaration.AddTypeDeclarationAfter(classDeclaration, file.TypeDeclarations.LastOrDefault());

                
        return null;
    }

    public override string Text => "Generate an extension method";

    public override bool IsAvailable(IUserDataHolder cache)
    {
        return reference.IsValid();
    }
}
