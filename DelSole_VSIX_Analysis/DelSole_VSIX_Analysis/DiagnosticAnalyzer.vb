<DiagnosticAnalyzer(LanguageNames.VisualBasic)>
Public Class SnippetServiceLibraryAnalyzerAnalyzer
    Inherits DiagnosticAnalyzer

    Public Const DiagnosticId = "SSL001"

    ' You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
    Private Shared ReadOnly Title As LocalizableString = New LocalizableResourceString(NameOf(My.Resources.AnalyzerTitle), My.Resources.ResourceManager, GetType(My.Resources.Resources))
    Private Shared ReadOnly MessageFormat As LocalizableString = New LocalizableResourceString(NameOf(My.Resources.AnalyzerMessageFormat), My.Resources.ResourceManager, GetType(My.Resources.Resources))
    Private Shared ReadOnly Description As LocalizableString = New LocalizableResourceString(NameOf(My.Resources.AnalyzerDescription), My.Resources.ResourceManager, GetType(My.Resources.Resources))
    Private Const Category = "Syntax"

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
        If memberAccessExpr?.Name.ToString <> "Build" Then Return
        'If the method name is ParseFeedAsync, check for the symbol info
        'and see if the return type matches
        Dim memberSymbol = TryCast(context.SemanticModel.
      GetSymbolInfo(memberAccessExpr).Symbol, IMethodSymbol)
        If memberSymbol Is Nothing Then Return
        Dim result = memberSymbol.ToString

        If Not memberSymbol?.ContainingSymbol.ToString = "DelSole.VSIX.VSIXPackage" Then Return
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
       "The specified file name might not be a valid code snippet file")
            context.ReportDiagnostic(diagn)
        End If
    End Sub

End Class