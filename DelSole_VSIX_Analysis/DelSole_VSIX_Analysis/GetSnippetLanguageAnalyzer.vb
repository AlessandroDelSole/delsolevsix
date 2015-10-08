Imports System.Collections.Immutable
Imports System.Threading
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.Text
Imports Microsoft.CodeAnalysis.VisualBasic
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

<DiagnosticAnalyzer(LanguageNames.VisualBasic)>
Public Class GetSnippetLanguageAnalyzer
    Inherits DiagnosticAnalyzer

    Public Const DiagnosticId = "SSL003"
    Friend Shared ReadOnly Title As LocalizableString = "GetSnippetDescription has an invalid file name"
    Friend Shared ReadOnly MessageFormat As LocalizableString = "'{0}'"
    Friend Shared ReadOnly Description As LocalizableString = "Snippet file name extensions should end with .*snippet"
    Friend Const Category = "Naming"

    Private Shared Rule As New DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault:=True, description:=Description)

    Public Overrides ReadOnly Property SupportedDiagnostics As ImmutableArray(Of DiagnosticDescriptor)
        Get
            Return ImmutableArray.Create(Rule)
        End Get
    End Property

    Public Overrides Sub Initialize(context As AnalysisContext)
        ' Register an action when compilation starts
        context.RegisterCompilationStartAction(
            Sub(ctx As CompilationStartAnalysisContext)
                'Detect if the type metadata
                'exists in the compilation context
                Dim myLibraryType =
        ctx.Compilation.
          GetTypeByMetadataName("DelSole.VSIX.VSIXPackage")
                'If not, return
                '(no reference to the library)
                If myLibraryType Is Nothing Then Return
                'Register an action against
                'an InvocationExpression
                ctx.RegisterSyntaxNodeAction(
          AddressOf AnalyzeMethodInvocation,
          SyntaxKind.InvocationExpression)
            End Sub)
    End Sub

    Private Shared Sub AnalyzeMethodInvocation(context As SyntaxNodeAnalysisContext)
        'Convert the current syntax node into an InvocationExpressionSyntax
        'which represents a method call
        Dim invocationExpr = CType(context.Node, InvocationExpressionSyntax)
        'Convert the associated expression into a MemberAccessExpressionSyntax
        'which represents a method's information
        'If the expression Is Not a MemberAccessExpressionSyntax, return
        If TypeOf invocationExpr.Expression IsNot MemberAccessExpressionSyntax Then Return
        Dim memberAccessExpr = DirectCast(invocationExpr.Expression,
    MemberAccessExpressionSyntax)
        'If the method name Is Not ParseFeedAsync, return
        If memberAccessExpr?.Name.ToString <> "GetSnippetLanguage" Then Return
        'If the method name is ParseFeedAsync, check for the symbol info
        'and see if the return type matches
        Dim memberSymbol = TryCast(context.SemanticModel.
      GetSymbolInfo(memberAccessExpr).Symbol, IMethodSymbol)
        If memberSymbol Is Nothing Then Return
        Dim result = memberSymbol.ToString

        If Not memberSymbol?.ContainingSymbol.ToString = "DelSole.VSIX.SnippetInfo" Then Return
        'Check if the method call has the required argument number
        Dim argumentList = invocationExpr.ArgumentList
        If argumentList?.Arguments.Count <> 1 Then Return
        'Convert the expression for the first method argument into
        'a LiteralExpressionSyntax. If null, return
        Dim fileNameLiteral =
    DirectCast(invocationExpr.ArgumentList.Arguments(0).GetExpression,
      LiteralExpressionSyntax)
        If fileNameLiteral Is Nothing Then Return
        'Convert the actual value for the method argument into string
        'If null, return
        Dim fileNameLiteralOpt = context.SemanticModel.GetConstantValue(fileNameLiteral)
        Dim fileNameValue = DirectCast(fileNameLiteralOpt.Value, String)
        If fileNameValue Is Nothing Then Return

        'If the URL Is Not well-formed, create a diagnostic
        If Not IO.Path.GetExtension(fileNameValue).ToLower.EndsWith("snippet") Then
            Dim diagn = Diagnostic.Create(Rule, fileNameLiteral.GetLocation,
       "Make sure the file extension is a valid .*snippet extension")
            context.ReportDiagnostic(diagn)
        End If
    End Sub
End Class