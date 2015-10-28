﻿Imports System.ComponentModel
Imports System.IO, System.IO.Packaging
Imports <xmlns="http://schemas.microsoft.com/developer/vsx-schema/2011">
Imports System.IO.Compression
Imports Ionic.Zip

''' <summary>
''' Represents a VSIX package with its contents and metadata,
''' and provides methods for VSIX generation
''' </summary>
Public Class VSIXPackage
    Implements INotifyPropertyChanged
    Implements IDataErrorInfo

    ''' <summary>
    ''' Notify callers for changes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    ''' <summary>
    ''' Raise the PropertyChanged event when instance data changes
    ''' </summary>
    ''' <param name="name"></param>
    Protected Sub OnPropertyChanged(ByVal name As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub

#Region "Backing fields"

    'Backing fields for properties that will be set by the user
    Dim _packageName As String
    Dim _packageAuthor As String
    Dim _packageDescription As String
    Dim _packageVersion As String
    Dim _license As String
    Dim _iconPath As String
    Dim _previewImagePath As String
    Dim _releaseNotes As String
    Dim _moreInfoURl As String
    Dim _gettingStartedGuide As String
    Dim _tags As String
    Dim _requiresAdminElevation As Boolean
    Dim _requiresWindowsInstaller As Boolean

    Dim _snippetFolderName As String
    Dim snippetFolder As String
#End Region

#Region "User-provided properties"
    ''' <summary>
    ''' The name of the VSIX package. 
    ''' </summary>
    ''' <returns>String</returns>
    Public Property ProductName As String
        Get
            Return _packageName
        End Get
        Set(value As String)
            _packageName = value
            OnPropertyChanged("ProductName")
            CheckValue("ProductName")
        End Set
    End Property

    ''' <summary>
    ''' The folder that contains the .snippet files
    ''' </summary>
    ''' <returns>String</returns>
    Public Property SnippetFolderName As String
        Get
            Return _snippetFolderName
        End Get
        Set(value As String)
            _snippetFolderName = value
            OnPropertyChanged("SnippetFolderName")
            CheckValue("SnippetFolderName")
        End Set
    End Property

    ''' <summary>
    ''' The VSIX package description. This will
    ''' also be shown in the VS Gallery
    ''' </summary>
    ''' <returns>String</returns>
    Public Property PackageDescription As String
        Get
            Return Me._packageDescription
        End Get
        Set(value As String)
            Me._packageDescription = value
            OnPropertyChanged("PackageDescription")
            CheckValue("PackageDescription")
        End Set
    End Property

    ''' <summary>
    ''' The VSIX package version number
    ''' </summary>
    ''' <returns>String</returns>
    Public Property PackageVersion As String
        Get
            Return Me._packageVersion
        End Get
        Set(value As String)
            Me._packageVersion = value
            OnPropertyChanged("PackageVersion")
            CheckValue("PackageVersion")
        End Set
    End Property

    ''' <summary>
    ''' The VSIX package's author name
    ''' </summary>
    ''' <returns>String</returns>
    Public Property PackageAuthor As String
        Get
            Return _packageAuthor
        End Get
        Set(value As String)
            Me._packageAuthor = value
            OnPropertyChanged("PackageAuthor")
            CheckValue("PackageAuthor")
        End Set
    End Property

    ''' <summary>
    ''' The collection of code snippets that will be
    ''' packaged into the VSIX. Each code snippet is 
    ''' represented by an object of type SnippetInfo
    ''' </summary>
    ''' <returns>SnippetInfoCollection</returns>
    Public Property CodeSnippets As SnippetInfoCollection

    ''' <summary>
    ''' The license agreement an user must accept to install the VSIX package.
    ''' This must be the pathname of a .txt or .rtf file
    ''' </summary>
    ''' <returns>String</returns>
    Public Property License As String
        Get
            Return Me._license
        End Get
        Set(value As String)
            Me._license = value
            OnPropertyChanged("License")
        End Set
    End Property

    ''' <summary>
    ''' The pathname of an icon file for the VSIX package.
    ''' Supported formats are .ico, .jpg, .png, .tif/.tiff, .bmp
    ''' </summary>
    ''' <returns>String</returns>
    Public Property IconPath As String
        Get
            Return _iconPath
        End Get
        Set(value As String)
            _iconPath = value
            OnPropertyChanged("IconPath")
        End Set
    End Property

    ''' <summary>
    ''' The pathname of an image file to be used as a preview.
    ''' Supported formats are .ico, .jpg, .png, .tif/.tiff, .bmp
    ''' </summary>
    ''' <returns>String</returns>
    Public Property PreviewImagePath As String
        Get
            Return _previewImagePath
        End Get
        Set(value As String)
            _previewImagePath = value
            OnPropertyChanged("PreviewImagePath")
        End Set
    End Property

    ''' <summary>
    ''' The pathname of a file containing release notes for the VSIX package. 
    ''' Supported formats: .html/.htm, .txt, .rtf
    ''' </summary>
    ''' <returns>String</returns>
    Public Property ReleaseNotes As String
        Get
            Return _releaseNotes
        End Get
        Set(value As String)
            _releaseNotes = value
            OnPropertyChanged("ReleaseNotes")
        End Set
    End Property

    ''' <summary>
    ''' A Web address where users can find additional info
    ''' about the generated VSIX package
    ''' </summary>
    ''' <returns>String</returns>
    Public Property MoreInfoURL As String
        Get
            Return _moreInfoURl
        End Get
        Set(value As String)
            _moreInfoURl = value
            OnPropertyChanged("MoreInfoURL")
        End Set
    End Property

    ''' <summary>
    ''' A Web address where users can find a guide to get
    ''' started with the contents of the VSIX package
    ''' </summary>
    ''' <returns>String</returns>
    Public Property GettingStartedGuide As String
        Get
            Return _gettingStartedGuide
        End Get
        Set(value As String)
            _gettingStartedGuide = value
            OnPropertyChanged("GettingStartedGuide")
        End Set
    End Property

    ''' <summary>
    ''' A comma-separated list of tags for categorizing
    ''' the contents of the VSIX package
    ''' </summary>
    ''' <returns>String</returns>
    Public Property Tags As String
        Get
            Return _tags
        End Get
        Set(value As String)
            _tags = value
            OnPropertyChanged("Tags")
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether the VSIX package requires
    ''' administrator privileges to be installed
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Property RequiresAdminElevation As Boolean
        Get
            Return _requiresAdminElevation
        End Get
        Set(value As Boolean)
            _requiresAdminElevation = value
            OnPropertyChanged("RequiresAdminElevation")
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether the VSIX package will be
    ''' installed via the VSIX engine or via Windows Installer
    ''' </summary>
    ''' <returns>Boolean</returns>
    ''' <remarks>Assign false if the VSIX package only contains code snippets</remarks>
    Public Property RequiresWindowsInstaller As Boolean
        Get
            Return _requiresWindowsInstaller
        End Get
        Set(value As Boolean)
            _requiresWindowsInstaller = value
            OnPropertyChanged("RequiresWindowsInstaller")
        End Set
    End Property

#End Region

#Region "Auto-populated properties"
    ''' <summary>
    ''' Get a unique identifier for the VSIX package
    ''' </summary>
    ''' <returns>Guid</returns>
    Public ReadOnly Property PackageGuid As Guid
    ''' <summary>
    ''' Return the list of dependencies for the VSIX package
    ''' </summary>
    ''' <returns>String</returns>
    Public ReadOnly Property Dependencies As String
    ''' <summary>
    ''' Return the list of assets for the VSIX package
    ''' </summary>
    ''' <returns>String</returns>
    Public ReadOnly Property Assets As String
    ''' <summary>
    ''' Return the list of supported Visual Studio editions
    ''' </summary>
    ''' <returns>String</returns>
    Public ReadOnly Property Targets As String

    ''' <summary>
    ''' Return the VSIX package ID
    ''' </summary>
    ''' <returns>String</returns>
    Public ReadOnly Property PackageID As String
        Get
            If Me.ProductName IsNot Nothing Then
                Dim noSpaces = Me.ProductName.Trim()
                noSpaces = noSpaces.Replace(" ", "")
                Return noSpaces + ".." + Me.PackageGuid.ToString
            Else
                Return String.Empty
            End If
        End Get
    End Property

    ''' <summary>
    ''' Return the package definition file name (.pkgdef)
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property PkgDefName As String
        Get
            If Me.ProductName IsNot Nothing Then
                Dim noSpaces = Me.ProductName.Trim()
                noSpaces = noSpaces.Replace(" ", "")
                'Return "Snippet\" + noSpaces + ".pkgdef"
                Return noSpaces + ".pkgdef"
            Else
                Return String.Empty
            End If
        End Get
    End Property
#End Region

    ''' <summary>
    ''' Create a temporary folder to handle package-required files.
    ''' If the folder already exists, it is first deleted
    ''' </summary>
    ''' <returns>String</returns>
    Private Function GetTempFolder() As String
        Dim tempFolder = IO.Path.GetTempPath + "SnippetPackageBuilder"
        If IO.Directory.Exists(tempFolder) Then
            IO.Directory.Delete(tempFolder, True)
        End If
        IO.Directory.CreateDirectory(tempFolder)

        Return tempFolder
    End Function

    ''' <summary>
    ''' Copy all snippet files to the temporary folder
    ''' so that they will be easily packaged into the VSIX
    ''' </summary>
    ''' <param name="tempFolder"></param>
    Private Sub CopySnippets(tempFolder As String)
        Try
            'Create a subfolder that stores all snippets
            Me.snippetFolder = tempFolder + "\" + Me.SnippetFolderName.Replace(" ", "%20")
            IO.Directory.CreateDirectory(snippetFolder)

            'Copy all snippets in the collection to the above subfolder
            For Each item1 In Me.CodeSnippets
                Dim tempName As String = snippetFolder + "\" + item1.SnippetFileName.Replace(" ", "%20")
                If IO.File.Exists(tempName) Then IO.File.Delete(tempName)

                IO.File.Copy(item1.SnippetPath + "\" + item1.SnippetFileName, tempName)
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Generate the .Pkgdef file into the Temporary folder
    ''' </summary>
    Private Sub GeneratePkgDefToTempFolder()
        Try
            'Generate a .pkgdef file content based on the language of the 
            'first snippet in the list
            'Dim noSpaces = Me.ProductName.Trim()
            'noSpaces = noSpaces.Replace(" ", "")
            Dim lang As String
            'Language literal is the same of .snippet files, except for Visual Basic
            If Me.CodeSnippets.First.SnippetLanguage.ToLower = "vb" Then
                lang = "Basic"
            Else
                lang = Me.CodeSnippets.First.SnippetLanguage
            End If
            Dim packageDef As String = "[$RootKey$\Languages\CodeExpansions\" + lang + "\Paths]" + Environment.NewLine + Chr(34) + IO.Path.GetFileNameWithoutExtension(PkgDefName) + Chr(34) + "=""$PackageFolder$"""

            Dim packageDefPathName = snippetFolder + "\" + PkgDefName
            My.Computer.FileSystem.WriteAllText(packageDefPathName, packageDef, False)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Generate the VSIX Manifest (extension.vsixmanifest file)
    ''' Invoked internally by the <seealso cref="Build(String)"/> method
    ''' </summary>
    ''' <param name="targetFolder"></param>
    Private Sub GenerateVsixManifest(targetFolder As String)

        Dim VsixManifest As XElement = <PackageManifest Version="2.0.0" xmlns="http://schemas.microsoft.com/developer/vsx-schema/2011">
                                           <Metadata>
                                               <Identity Id=<%= Me.PackageID %> Version="1.0" Language="en-US" Publisher=<%= Me.PackageAuthor %>/>
                                               <DisplayName><%= Me.ProductName %></DisplayName>
                                               <Description xml:space="preserve"><%= Me.PackageDescription %></Description>
                                               <GettingStartedGuide><%= Me.GettingStartedGuide %></GettingStartedGuide>
                                               <Icon><%= IO.Path.GetFileName(Me.IconPath) %></Icon>
                                               <PreviewImage><%= IO.Path.GetFileName(Me.PreviewImagePath) %></PreviewImage>
                                           </Metadata>
                                           <Installation>
                                               <InstallationTarget Id="Microsoft.VisualStudio.Community" Version="[12.0,14.0]"/>
                                               <InstallationTarget Version="[12.0,14.0]" Id="Microsoft.VisualStudio.VWDExpress"/>
                                               <%= If(Me.CodeSnippets.First.SnippetLanguage.ToUpper = "JAVASCRIPT", Nothing, <InstallationTarget Version="[12.0,14.0]" Id="Microsoft.VisualStudio.VSWinDesktopExpress"/>) %>
                                               <InstallationTarget Version="[12.0,14.0]" Id="Microsoft.VisualStudio.VPDExpress"/>
                                               <InstallationTarget Version="[12.0,14.0]" Id="Microsoft.VisualStudio.VSWinExpress"/>
                                           </Installation>
                                           <Dependencies>
                                               <Dependency Id="Microsoft.Framework.NDP" DisplayName="Microsoft .NET Framework" Version="4.5"/>
                                           </Dependencies>
                                           <Assets>
                                               <Asset Type="Microsoft.VisualStudio.VsPackage" Path=<%= "Snippets\" & Me.PkgDefName %>/>
                                           </Assets>
                                       </PackageManifest>

        'For some reasons, the License field is the only one you have to check
        'If the XElement is empty, the whole package won't work
        If Me.License <> "" Or Me.License IsNot Nothing Then
            VsixManifest.Elements.First.Add(<License><%= IO.Path.GetFileName(Me.License) %></License>)
        End If

        'Save the manifest and content types to the temp folder
        VsixManifest.Save(targetFolder + "\extension.vsixmanifest")
    End Sub

    ''' <summary>
    ''' Build a VSIX package that contains the code snippets hold by the CodeSnippets property. Properties representing the package manifest must be populated before invoking this method.
    ''' </summary>
    ''' <param name="fileName"></param>
    Public Sub Build(fileName As String)
        If HasErrors Then
            Throw New InvalidOperationException("Cannot build the VSIX package. The current instance of the VSIXPackage class has errors that must be fixed.")
        End If

        'Create a temporary folder that stores all the archive content
        'and that will be later zipped into a VSIX
        Dim tempFolder = GetTempFolder()

        'Generate the VSIX manifest file
        GenerateVsixManifest(tempFolder)

        'Copy assets to the temp folder
        If Me.License <> "" Then
            IO.File.Copy(Me.License, tempFolder + "\" + IO.Path.GetFileName(Me.License))
        End If

        If Me.IconPath <> "" Then
            IO.File.Copy(Me.IconPath, tempFolder + "\" + IO.Path.GetFileName(Me.IconPath))
        End If

        If Me.PreviewImagePath <> "" Then
            IO.File.Copy(Me.PreviewImagePath, tempFolder + "\" + IO.Path.GetFileName(Me.PreviewImagePath))
        End If

        If Me.GettingStartedGuide <> "" Then
            IO.File.Copy(Me.GettingStartedGuide, tempFolder + "\" + IO.Path.GetFileName(Me.GettingStartedGuide))
        End If

        'Create a subfolder that stores all snippets        
        CopySnippets(tempFolder)

        'Generate a .pkgdef file content based on the language of the 
        GeneratePkgDefToTempFolder()

        'Zip the package into VSIX
        Me.Zip(fileName, tempFolder)
    End Sub

    ''' <summary>
    ''' Create a Zip file according to the Open Packaging Conventions, that is what the VSIX format relies on
    ''' </summary>
    ''' <param name="zipFileName"></param>
    ''' <param name="startFolder"></param>
    Private Sub Zip(zipFileName As String, startFolder As String)
        If IO.File.Exists(zipFileName) Then IO.File.Delete(zipFileName)

        Using zip As Package = Package.Open(zipFileName, FileMode.OpenOrCreate)
            For Each ioFile In IO.Directory.EnumerateFiles(startFolder, "*.*")
                Compression.AddFileToZip(zip, ioFile)
            Next

            For Each ioFile In IO.Directory.EnumerateFiles(snippetFolder, "*.*")
                Compression.AddFileToZip(zip, ioFile, Me.SnippetFolderName.Replace(" ", "%20"))
            Next
        End Using
    End Sub

#Region "Validation"
    ''' <summary>
    ''' Holds a collection of validation errors
    ''' </summary>
    Private validationErrors As New Dictionary(Of String, String)

    ''' <summary>
    ''' Add a validation error to the collection of validation errors
    ''' passing the property name and the error message
    ''' </summary>
    ''' <param name="columnName"></param>
    ''' <param name="msg"></param>
    Protected Sub AddError(ByVal columnName As String, ByVal msg As String)
        If Not validationErrors.ContainsKey(columnName) Then
            validationErrors.Add(columnName, msg)
        End If
    End Sub

    ''' <summary>
    ''' Remove a validation error from the collection
    ''' </summary>
    ''' <param name="columnName"></param>
    Protected Sub RemoveError(ByVal columnName As String)
        If validationErrors.ContainsKey(columnName) Then
            validationErrors.Remove(columnName)
        End If
    End Sub

    ''' <summary>
    ''' Return True if the current instance has validation errors
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Overridable ReadOnly Property HasErrors() As Boolean
        Get
            Return (validationErrors.Count > 0)
        End Get
    End Property

    ''' <summary>
    ''' Return an error message
    ''' </summary>
    ''' <returns>String</returns>
    Public ReadOnly Property [Error]() As String _
        Implements System.ComponentModel.IDataErrorInfo.Error
        Get
            If validationErrors.Count > 0 Then
                Return $"{TypeName(Me)} data is invalid."
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Default property for the specified property name
    ''' </summary>
    ''' <param name="columnName"></param>
    ''' <returns>String</returns>
    Default Public ReadOnly Property Item(ByVal columnName As String) As String _
        Implements System.ComponentModel.IDataErrorInfo.Item
        Get
            If validationErrors.ContainsKey(columnName) Then
                Return validationErrors(columnName).ToString
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Check for required values and add/removes
    ''' validation errors
    ''' </summary>
    ''' <param name="value"></param>
    Private Sub CheckValue(ByVal value As String)
        Select Case value
            Case Is = "ProductName"
                If Me.ProductName = "" Or String.IsNullOrEmpty(Me.ProductName) Then
                    Me.AddError("ProductName", "Value cannot be null")
                Else
                    Me.RemoveError("ProductName")
                End If
            Case Is = "PackageDescription"
                If Me.PackageDescription = "" Or String.IsNullOrEmpty(Me.PackageDescription) Then
                    Me.AddError("PackageDescription", "Value cannot be null")
                Else
                    Me.RemoveError("PackageDescription")
                End If
            Case Is = "PackageAuthor"
                If Me.PackageAuthor = "" Or String.IsNullOrEmpty(Me.PackageAuthor) Then
                    Me.AddError("PackageAuthor", "Value cannot be null")
                Else
                    Me.RemoveError("PackageAuthor")
                End If
            Case Is = "SnippetFolderName"
                If Me.SnippetFolderName = "" Or String.IsNullOrEmpty(Me.SnippetFolderName) Then
                    Me.AddError("SnippetFolderName", "Value cannot be null")
                Else
                    Me.RemoveError("SnippetFolderName")
                End If
        End Select
    End Sub

#End Region

    ''' <summary>
    ''' Create an instance of the class, supplying default values
    ''' for every property
    ''' </summary>
    Public Sub New()
        'Initializes properties with sample values
        Me.CodeSnippets = New SnippetInfoCollection
        Me.PackageAuthor = My.Computer.Name
        Me.PackageGuid = Guid.NewGuid
        Me.PackageVersion = "1.0"
        Me.ProductName = "My sample package"
        Me.PackageDescription = "Package description goes here"
        Me.SnippetFolderName = "My custom snippets"
    End Sub

    ''' <summary>
    ''' Return True if code snippets target more than one programming language
    ''' </summary>
    ''' <returns></returns>
    Public Function TestLanguageGroup() As Boolean
        Dim query = From item In Me.CodeSnippets
                    Group item By item.SnippetLanguage Into Count()
                    Select SnippetLanguage, Count

        If query.Count > 1 Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Merge the specified VSIX packages into the current VSIXPackage instance
    ''' </summary>
    ''' <param name="packages"></param>
    Public Sub MergeVsix(packages() As String)
        Throw New NotImplementedException
    End Sub

    ''' <summary>
    ''' [INCOMPLETE] Merge the specified VSIX packages into one destination package
    ''' </summary>
    ''' <param name="sourcePackages"></param>
    ''' <param name="destinationPackage"></param>
    Public Shared Sub MergeVsix(sourcePackages() As String, destinationPackage As String)
        Dim tempFolder = Path.GetTempPath
        Dim allPackFolders As List(Of String) = Nothing
        Dim newPackageDefinition As String = ""

        For Each fileName In sourcePackages
            'Check if all source packages exist
            If Not IO.File.Exists(fileName) Then
                Throw New FileNotFoundException("File not found", fileName)
            End If
        Next

        For Each fileName In sourcePackages
            'Generate temp folder for each package
            Dim newFolder = tempFolder & ("\") & IO.Path.GetFileNameWithoutExtension(fileName)
            If allPackFolders Is Nothing Then
                allPackFolders = New List(Of String)
            End If
            allPackFolders.Add(newFolder)

            'Extract the VSIX package
            ExtractVsix(fileName, newFolder)
        Next

        For Each folder In allPackFolders
            'Enumerate and read pkgdef from each unzipped folder
            Dim pkgDef = IO.Directory.EnumerateFiles(folder, "*.pkgdef").FirstOrDefault
            Dim pkgDefContent = My.Computer.FileSystem.ReadAllText(pkgDef)

            'Concatenate the pkgdef
            newPackageDefinition = newPackageDefinition & Environment.NewLine & pkgDefContent
        Next

        'Create temp pkgdef
        Dim tempPkgdefName = tempFolder & IO.Path.GetFileNameWithoutExtension(destinationPackage) & ".pkgdef"
        My.Computer.FileSystem.WriteAllText(tempPkgdefName, newPackageDefinition, False)

        Using zip As Package = Package.Open(destinationPackage, FileMode.OpenOrCreate)
            'Package the content of VSIX files into one package
            For Each startFolder In allPackFolders
                For Each ioFile In IO.Directory.EnumerateFiles(startFolder, "*.*")
                    Compression.AddFileToZip(zip, ioFile)
                Next
            Next
            'Add the pkgdef
            Compression.AddFileToZip(zip, tempPkgdefName)
        End Using
    End Sub

    ''' <summary>
    ''' Extract the content of the specified VSIX package into the specified folder
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <param name="targetDirectory"></param>
    Public Shared Sub ExtractVsix(fileName As String, targetDirectory As String)
        IO.Compression.ZipFile.ExtractToDirectory(fileName, targetDirectory)
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
    Public Shared Sub Vsi2Vsix(vsiFileName As String, vsixFileName As String, snippetFolderName As String,
                               packageAuthor As String, packageName As String, packageDescription As String,
                               iconPath As String, imagePath As String, moreInfoUrl As String)

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

        'Has comments/EULA?
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
End Class

#Region "Zip Support"
''' <summary>
''' Provides support for zip file compression
''' </summary>
Public Class Compression

    Const BUFFER_SIZE As Long = 4096

    ''' <summary>
    ''' Return the proper MIME type for the specified file name
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <returns>String</returns>
    Private Shared Function getContentType(fileName As String) As String
        Dim contentType As String
        Select Case IO.Path.GetExtension(fileName).ToLower
            Case Is = ".jpg"
                contentType = System.Net.Mime.MediaTypeNames.Image.Jpeg
            Case Is = ".pkgdef"
                contentType = System.Net.Mime.MediaTypeNames.Text.Plain
            Case Is = ".txt"
                contentType = System.Net.Mime.MediaTypeNames.Text.Plain
            Case Is = ".vsixmanifest"
                contentType = System.Net.Mime.MediaTypeNames.Text.Xml
            Case Else
                contentType = System.Net.Mime.MediaTypeNames.Application.Octet
        End Select

        Return contentType
    End Function

    ''' <summary>
    ''' Add the specified file to a zip archive
    ''' </summary>
    ''' <param name="zip">A System.IO.Package zip</param>
    ''' <param name="fileToAdd">the file name to add</param>
    ''' <param name="directoryFile">the target folder</param>
    ''' <returns>Boolean</returns>
    Public Shared Function AddFileToZip(ByVal zip As Package,
                             ByVal fileToAdd As String,
                             ByVal directoryFile As String) As Boolean
        Try

            Dim destFilename As String = ".\" & directoryFile & "\" & Path.GetFileName(fileToAdd)
            Dim uri As Uri = PackUriHelper.CreatePartUri(New Uri(destFilename, UriKind.Relative))
            If zip.PartExists(uri) Then
                zip.DeletePart(uri)
            End If

            Dim contentType As String = getContentType(fileToAdd)

            Dim part As PackagePart = zip.CreatePart(uri, contentType, CompressionOption.Normal)

            Using fileStream As New FileStream(fileToAdd, FileMode.Open, FileAccess.Read)
                Using dest As Stream = part.GetStream()
                    CopyStream(fileStream, dest)
                End Using
            End Using
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Add the specified file to a zip archive
    ''' </summary>
    ''' <param name="zipFileName">The target zip archive's name</param>
    ''' <param name="fileToAdd">the file name to add</param>
    ''' <param name="directoryFile">the target folder</param>
    ''' <returns>Boolean</returns>
    Public Shared Function AddFileToZip(ByVal zipFilename As String,
                                 ByVal fileToAdd As String,
                                 ByVal directoryFile As String) As Boolean
        Try
            Dim contentType As String = getContentType(fileToAdd)
            Using zip As Package = System.IO.Packaging.Package.Open(zipFilename, FileMode.OpenOrCreate)
                Dim destFilename As String = ".\" & directoryFile & "\" & Path.GetFileName(fileToAdd)
                Dim uri As Uri = PackUriHelper.CreatePartUri(New Uri(destFilename, UriKind.Relative))
                If zip.PartExists(uri) Then
                    zip.DeletePart(uri)
                End If
                Dim part As PackagePart = zip.CreatePart(uri, contentType, CompressionOption.Normal)

                Using fileStream As New FileStream(fileToAdd, FileMode.Open, FileAccess.Read)
                    Using dest As Stream = part.GetStream()
                        CopyStream(fileStream, dest)
                    End Using
                End Using
            End Using
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Add the specified file to a zip archive
    ''' </summary>
    ''' <param name="zipFileName">The target zip archive's name</param>
    ''' <param name="fileToAdd">the file name to add</param>
    ''' <returns>Boolean</returns>
    Public Shared Function AddFileToZip(ByVal zipFilename As String, ByVal fileToAdd As String) As Boolean

        Try
            Dim contentType As String = getContentType(fileToAdd)

            Using zip As Package = System.IO.Packaging.Package.Open(zipFilename, FileMode.OpenOrCreate)
                Dim destFilename As String = ".\" & Path.GetFileName(fileToAdd)
                Dim uri As Uri = PackUriHelper.CreatePartUri(New Uri(destFilename, UriKind.Relative))
                If zip.PartExists(uri) Then
                    zip.DeletePart(uri)
                End If
                Dim part As PackagePart = zip.CreatePart(uri, contentType, CompressionOption.Normal)
                Using fileStream As New FileStream(fileToAdd, FileMode.Open, FileAccess.Read)
                    Using dest As Stream = part.GetStream()
                        CopyStream(fileStream, dest)
                    End Using
                End Using
            End Using
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Add the specified file to a zip archive
    ''' </summary>
    ''' <param name="zip">The target System.IO.Package zip archive</param>
    ''' <param name="fileToAdd">the file name to add</param>
    ''' <returns>Boolean</returns>
    Public Shared Function AddFileToZip(ByVal zip As Package, ByVal fileToAdd As String) As Boolean

        Try
            Dim destFilename As String = ".\" & Path.GetFileName(fileToAdd)
            Dim uri As Uri = PackUriHelper.CreatePartUri(New Uri(destFilename, UriKind.Relative))
            If zip.PartExists(uri) Then
                zip.DeletePart(uri)
            End If

            Dim contentType As String = getContentType(fileToAdd)

            Dim part As PackagePart = zip.CreatePart(uri, contentType, CompressionOption.Normal)
            Using fileStream As New FileStream(fileToAdd, FileMode.Open, FileAccess.Read)
                Using dest As Stream = part.GetStream()
                    CopyStream(fileStream, dest)
                End Using
            End Using
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Copy a stream into another
    ''' </summary>
    ''' <param name="inputStream"></param>
    ''' <param name="outputStream"></param>
    Private Shared Sub CopyStream(ByVal inputStream As System.IO.FileStream, ByVal outputStream As System.IO.Stream)
        Dim bufferSize As Long = If(inputStream.Length < BUFFER_SIZE, inputStream.Length, BUFFER_SIZE)
        Dim buffer As Byte() = New Byte(CInt(bufferSize) - 1) {}
        Dim bytesRead As Integer = 0
        Dim bytesWritten As Long = 0
        While (InlineAssignHelper(bytesRead, inputStream.Read(buffer, 0, buffer.Length))) <> 0
            outputStream.Write(buffer, 0, bytesRead)
            bytesWritten += bufferSize
        End While
    End Sub

    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
        target = value
        Return value
    End Function

End Class
#End Region