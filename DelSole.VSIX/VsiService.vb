Imports System.IO
Imports System.IO.Compression
Imports Ionic.Zip

Namespace VsiTools
    ''' <summary>
    ''' Provide APIs to work with old-style .vsi packages (VS 2005, 2008, 2010)
    ''' </summary>
    Public Class VsiService
        ''' <summary>
        ''' Convert an obsolete VSI package into a VSIX package. The source VSI package must contain only code snippets
        ''' </summary>
        ''' <param name="vsiFileName">The source .vsi pathname</param>
        ''' <param name="vsixFileName">The target .vsix pathname</param>
        ''' <param name="snippetFolderName">The folder name that appears inside IntelliSense</param>
        ''' <param name="packageAuthor">The Vsix author</param>
        ''' <param name="packageName">The Vsix product name</param>
        ''' <param name="packageDescription">The Vsix description</param>
        ''' <param name="iconPath">The icon for the Vsix in the VS Gallery</param>
        ''' <param name="imagePath">The preview image for the Vsix in the VS Gallery</param>
        ''' <param name="moreInfoUrl">The URL string for More Info</param>
        Public Shared Sub Vsi2Vsix(vsiFileName As String, vsixFileName As String, snippetFolderName As String,
                                   packageAuthor As String, packageName As String, packageDescription As String,
                                   iconPath As String, imagePath As String, moreInfoUrl As String)
            If Not IO.File.Exists(vsiFileName) Then
                Throw New ArgumentException("File not found", NameOf(vsiFileName))
            End If

            'Get a temporary folder
            Dim tempFolder = Path.GetTempPath & IO.Path.GetFileNameWithoutExtension(vsiFileName)

            'Extract the old vsi package into a temp folder
            Dim oldVsi As New Ionic.Zip.ZipFile(vsiFileName)
            oldVsi.ExtractAll(tempFolder, ExtractExistingFileAction.OverwriteSilently)

            'Are there any snippets?
            Dim snippets = IO.Directory.EnumerateFiles(tempFolder, "*.*snippet")

            'If not, throw
            If snippets.Any = False Then
                Throw New InvalidOperationException("The specified .vsi package does not contain any code snippets")
            End If

            Dim newVsixPackage As New VSIXPackage

            'In a .vsi, EULA is a zip comment. So, has it comments/EULA?
            If oldVsi.Comment <> "" Or String.IsNullOrWhiteSpace(oldVsi.Comment) = False Then
                My.Computer.FileSystem.WriteAllText(tempFolder & "\EULA.txt", oldVsi.Comment, False)
                newVsixPackage.License = tempFolder & "\EULA.txt"
            End If

            'Iterate the list of extracted snippets from
            'the old Vsi and populate a SnippetInfo collection
            For Each oldSnip In snippets
                'Add the proper utf-8 encoding to the snippet file
                FixSnippetTerminatorAndEncoding(oldSnip)

                'Generate a new SnippetInfo object per snippet
                Dim snipInfo As New SnippetInfo
                snipInfo.SnippetDescription = SnippetInfo.GetSnippetDescription(oldSnip)
                snipInfo.SnippetLanguage = SnippetInfo.GetSnippetLanguage(oldSnip)
                snipInfo.SnippetPath = IO.Path.GetDirectoryName(oldSnip)
                snipInfo.SnippetFileName = IO.Path.GetFileName(oldSnip)
                newVsixPackage.CodeSnippets.Add(snipInfo)
            Next

            Dim defaultValue = IO.Path.GetFileNameWithoutExtension(vsiFileName)

            'Populate package metadata
            If packageAuthor <> "" Or String.IsNullOrEmpty(packageAuthor) = False Then
                newVsixPackage.PackageAuthor = packageAuthor
            End If

            If packageName <> "" Or String.IsNullOrEmpty(packageName) = False Then
                newVsixPackage.ProductName = packageName
            Else
                newVsixPackage.ProductName = defaultValue
            End If

            If packageDescription <> "" Or String.IsNullOrEmpty(packageDescription) = False Then
                newVsixPackage.PackageDescription = packageDescription
            Else
                newVsixPackage.PackageDescription = defaultValue
            End If

            If snippetFolderName = "" Or String.IsNullOrEmpty(snippetFolderName) Then
                newVsixPackage.SnippetFolderName = defaultValue
            Else
                newVsixPackage.SnippetFolderName = snippetFolderName
            End If

            If iconPath <> "" Or String.IsNullOrEmpty(iconPath) = False Then
                newVsixPackage.IconPath = iconPath
            End If

            If imagePath <> "" Or String.IsNullOrEmpty(imagePath) = False Then
                newVsixPackage.PreviewImagePath = imagePath
            End If

            If moreInfoUrl <> "" Or String.IsNullOrEmpty(moreInfoUrl) = False Then
                newVsixPackage.MoreInfoURL = moreInfoUrl
            End If

            'Generate a new Vsix package
            newVsixPackage.Build(vsixFileName)
        End Sub

        ''' <summary>
        ''' Fix very old .snippet files that might miss the utf-8 encoding specification
        ''' and that might have an EOF terminator that can't be used with .NET
        ''' </summary>
        ''' <param name="snippetFileName"></param>
        Private Shared Sub FixSnippetTerminatorAndEncoding(snippetFileName As String)
            'Read the specified snippet file
            Dim txt = My.Computer.FileSystem.ReadAllText(snippetFileName)
            'If the last char in file is not >,
            If Asc(txt.Last) <> 62 Then
                'Create a new string content and rewrite the .snippet file
                Dim newString As New String(txt.Take(txt.Count - 1).ToArray)
                My.Computer.FileSystem.WriteAllText(snippetFileName, newString, False)
            End If

            'Load the snippet file
            Dim xdoc = XDocument.Load(snippetFileName)
            'Adjust the declaration
            Dim xdecl As New XDeclaration(xdoc.Declaration)
            xdecl.Encoding = "utf-8"

            xdoc.Declaration = xdecl

            xdoc.Save(snippetFileName)
        End Sub

        ''' <summary>
        ''' Extract the content of the specified .vsi package into the specified folder
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <param name="targetDirectory"></param>
        ''' <param name="extractOnlySnippets">If true, extracts only the code snippet files otherwise the whole archive content</param>
        Public Shared Sub ExtractVsi(fileName As String, targetDirectory As String, extractOnlySnippets As Boolean)
            If extractOnlySnippets = False Then
                IO.Compression.ZipFile.ExtractToDirectory(fileName, targetDirectory)
                Exit Sub
            Else
                Dim arch = IO.Compression.ZipFile.OpenRead(fileName)
                Dim query = From entry In arch.Entries
                            Where entry.FullName.ToLower.EndsWith("snippet")
                            Select entry

                For Each file In query
                    file.ExtractToFile(Path.Combine(targetDirectory, file.Name))
                Next
                Exit Sub
            End If
        End Sub
    End Class
End Namespace
