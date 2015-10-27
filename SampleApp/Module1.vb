Imports DelSole.VSIX
Module Module1

    Sub Main()
        'Create a SnippetInfo for each code snippet you want to package
        'Dim Snippet1 As New SnippetInfo
        'Snippet1.SnippetFileName = "UnprotectDocument.snippet"
        'Snippet1.SnippetPath = "C:\MySnippets"
        'Snippet1.SnippetLanguage = SnippetInfo.GetSnippetLanguage(Snippet1.SnippetPathName)
        'Snippet1.SnippetDescription = SnippetInfo.GetSnippetDescription(Snippet1.SnippetFileName)

        ''Create a new package
        'Dim Vsix As New VSIXPackage

        ''Populate the collection of snippets
        'Vsix.CodeSnippets.Add(Snippet1)

        ''Set package metadata information
        'Vsix.Tags = "Word"
        'Vsix.PackageAuthor = "Alessandro Del Sole"
        'Vsix.PackageDescription = "A test VSIX package with snippets"
        'Vsix.License = "C:\temp\MIT_License.txt"
        'Vsix.MoreInfoURL = "https://github.com/alessandrodelsole/delsolevsix"
        ''Assign other properties here...

        ''Go build it!
        'Vsix.Build("C:\temp\Sample.vsix")

        VSIXPackage.Vsi2Vsix("C:\temp\VBWPFSnippets.vsi", "C:\temp\VBWPFSnippets.vsix")
        Console.WriteLine("Done")
        Console.ReadLine()
    End Sub

End Module
