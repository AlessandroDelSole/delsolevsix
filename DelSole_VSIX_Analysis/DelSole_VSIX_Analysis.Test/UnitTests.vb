Imports DelSole_VSIX_Analysis
Imports DelSole_VSIX_Analysis.Test.TestHelper
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.CodeFixes
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace DelSole_VSIX_Analysis.Test
    <TestClass>
    Public Class UnitTest
        Inherits CodeFixVerifier

        'No diagnostics expected to show up
        <TestMethod>
        Public Sub TestMethod1()
            Dim test = ""
            VerifyBasicDiagnostic(test)
        End Sub

        'Diagnostic And CodeFix both triggered And checked for
        <TestMethod>
        Public Sub TestMethod2()

            Dim test = "
Module Module1

    Sub Main()

    End Sub

End Module"
            Dim expected = New DiagnosticResult With {.Id = "DelSole_VSIX_Analysis",
                .Message = String.Format("Type name '{0}' contains lowercase letters", "Module1"),
                .Severity = DiagnosticSeverity.Warning,
                .Locations = New DiagnosticResultLocation() {
                        New DiagnosticResultLocation("Test0.vb", 2, 8)
                    }
            }


            VerifyBasicDiagnostic(test, expected)

            Dim fixtest = "
Module MODULE1

    Sub Main()

    End Sub

End Module"
            VerifyBasicFix(test, fixtest)
        End Sub

    End Class
End Namespace