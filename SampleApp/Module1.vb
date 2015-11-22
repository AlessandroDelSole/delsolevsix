Imports DelSole.VSIX, DelSole.VSIX.VsiTools
Module Module1

    Sub Main()
        'Open an existing Vsix package and get an instance of VSIXPackage
        Dim package = VSIXPackage.OpenVsix("c:\temp\wpc2015.vsix")
        Console.ReadLine()

        'Extract the whole content of a .vsix
        VSIXPackage.ExtractVsix("C:\temp\vbwpfsnippets.vsix", "C:\scac", False)
        Console.ReadLine()

        'Create a SnippetInfo for each code snippet you want to package
        Dim Snippet1 As New SnippetInfo
        Snippet1.SnippetFileName = "UnprotectDocument.snippet"
        Snippet1.SnippetPath = "C:\temp"
        Snippet1.SnippetLanguage = SnippetInfo.GetSnippetLanguage(Snippet1.SnippetPathName)
        Snippet1.SnippetDescription = SnippetInfo.GetSnippetDescription(Snippet1.SnippetFileName)

        Dim Snippet2 As New SnippetInfo
        Snippet2.SnippetFileName = "CreateATable.snippet"
        Snippet2.SnippetPath = "C:\temp"
        Snippet2.SnippetLanguage = SnippetInfo.GetSnippetLanguage(Snippet2.SnippetPathName)
        Snippet2.SnippetDescription = SnippetInfo.GetSnippetDescription(Snippet2.SnippetFileName)

        'Create a new package
        Dim Vsix As New VSIXPackage
        AddHandler Vsix.VsixGenerationStarted, Sub() Console.WriteLine("Generation started.")
        AddHandler Vsix.VsixGenerationCompleted, Sub() Console.WriteLine("Generation completed.")
        AddHandler Vsix.FileAddedToPackage, Sub(sender, e) Console.WriteLine(e.FileName)

        'Populate the collection of snippets
        Vsix.CodeSnippets.Add(Snippet1)
        Vsix.CodeSnippets.Add(Snippet2)

        'Set package metadata information
        Vsix.SnippetFolderName = "VSIX test pack"
        Vsix.Tags = "Word"
        Vsix.PackageAuthor = "Alessandro Del Sole"
        Vsix.PackageDescription = "A test VSIX package with snippets"
        Vsix.License = "C:\temp\MIT_License.txt"
        Vsix.MoreInfoURL = "https://github.com/alessandrodelsole/delsolevsix"
        'Assign other properties here...

        'Go build it!
        Vsix.Build("C:\temp\Sample.vsix")
        Console.WriteLine("Packaged with " & Vsix.CodeSnippets.Count & " snippets")

        'VsiService.Vsi2Vsix("C:\temp\VBWPFSnippets.vsi", "C:\temp\VBWPFSnippets.vsix",
        '                     "VB WPF Snippets", "Alessandro Del Sole", "VB Snippets for WPF", "A common set of WPF Snippets for VB",
        '                     Nothing, Nothing, "https://github.com/alessandrodelsole/delsolevsix")
        'Console.WriteLine("Package created. Signing...")

        Dim pwd = "SignExtension".ToCharArray
        Dim securePwd As New Security.SecureString
        For Each cc In pwd
            securePwd.AppendChar(cc)
        Next

        VSIXPackage.SignVsix("C:\temp\Sample.vsix", "C:\temp\SignExtension.pfx", securePwd)
        Console.WriteLine("Done")
        Console.ReadLine()
    End Sub

End Module
