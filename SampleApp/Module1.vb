Imports DelSole.VSIX
Module Module1

    Sub Main()
        ''Create a SnippetInfo for each code snippet you want to package
        'Dim Snippet1 As New SnippetInfo
        'Snippet1.SnippetFileName = "UnprotectDocument.snippet"
        'Snippet1.SnippetPath = "C:\temp"
        'Snippet1.SnippetLanguage = SnippetInfo.GetSnippetLanguage(Snippet1.SnippetPathName)
        'Snippet1.SnippetDescription = SnippetInfo.GetSnippetDescription(Snippet1.SnippetFileName)

        'Dim Snippet2 As New SnippetInfo
        'Snippet2.SnippetFileName = "CreateATable.snippet"
        'Snippet2.SnippetPath = "C:\temp"
        'Snippet2.SnippetLanguage = SnippetInfo.GetSnippetLanguage(Snippet2.SnippetPathName)
        'Snippet2.SnippetDescription = SnippetInfo.GetSnippetDescription(Snippet2.SnippetFileName)

        ''Create a new package
        'Dim Vsix As New VSIXPackage

        ''Populate the collection of snippets
        'Vsix.CodeSnippets.Add(Snippet1)
        'Vsix.CodeSnippets.Add(Snippet2)

        ''Set package metadata information
        'Vsix.SnippetFolderName = "VSIX test pack"
        'Vsix.Tags = "Word"
        'Vsix.PackageAuthor = "Alessandro Del Sole"
        'Vsix.PackageDescription = "A test VSIX package with snippets"
        'Vsix.License = "C:\temp\MIT_License.txt"
        'Vsix.MoreInfoURL = "https://github.com/alessandrodelsole/delsolevsix"
        ''Assign other properties here...

        ''Go build it!
        'Vsix.Build("C:\temp\Sample.vsix")

        VSIXPackage.Vsi2Vsix("C:\temp\VBWPFSnippets.vsi", "C:\temp\VBWPFSnippets.vsix",
                             "VB WPF Snippets", Nothing, "VB Snippets for WPF", "A common set of WPF Snippets for VB",
                             Nothing, Nothing, "https://github.com/alessandrodelsole/delsolevsix")
        Console.WriteLine("Package created. Starting...")
        Process.Start("C:\temp\VBWPFSnippets.vsix")
        Console.ReadLine()
    End Sub

End Module
