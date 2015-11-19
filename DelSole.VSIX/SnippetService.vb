Imports System.Collections.ObjectModel
Imports System.ComponentModel

Namespace SnippetTools
    ''' <summary>
    ''' Expose APIs to support the generation of IntelliSense code snippet files (.snippet)
    ''' </summary>
    Public Class SnippetService

        ''' <summary>
        ''' Return a schema-compliant string representing the code snippet kind
        ''' </summary>
        ''' <param name="kind"></param>
        ''' <returns>String</returns>
        Private Function ReturnSnippetKind(kind As CodeSnippetKinds) As String
            Dim snippetKind As String
            Select Case kind
                Case CodeSnippetKinds.MethodBody
                    snippetKind = "method body"
                Case CodeSnippetKinds.MethodDeclaration
                    snippetKind = "method decl"
                Case CodeSnippetKinds.File
                    snippetKind = "file"
                Case CodeSnippetKinds.TypeDeclaration
                    snippetKind = "type decl"
                Case Else
                    snippetKind = "any"
            End Select

            Return snippetKind
        End Function

        ''' <summary>
        ''' Return a schema-compliant string representing the code snippet language
        ''' </summary>
        ''' <param name="snippetLanguage"></param>
        ''' <returns>String</returns>
        Private Function ReturnSnippetLanguage(snippetLanguage As SnippetLanguages) As String
            Dim codeSnippetLanguage As String = "VB"
            Select Case snippetLanguage
                Case SnippetLanguages.VB
                    codeSnippetLanguage = "VB"
                Case SnippetLanguages.CSharp
                    codeSnippetLanguage = "CSharp"
                Case SnippetLanguages.XML
                    codeSnippetLanguage = "XML"
                Case SnippetLanguages.SQL
                    codeSnippetLanguage = "SQL"
                Case SnippetLanguages.CPP
                    codeSnippetLanguage = "CPP"
                Case SnippetLanguages.HTML
                    codeSnippetLanguage = "Html"
                Case SnippetLanguages.JavaScript
                    codeSnippetLanguage = "JavaScript"
            End Select
            Return codeSnippetLanguage
        End Function

        ''' <summary>
        ''' Create and save a code snippet file to disk.
        ''' </summary>
        ''' <param name="fileName">The target file name</param>
        ''' <param name="kind">The code snippet kind</param>
        ''' <param name="snippetLanguage">The code snippet language</param>
        ''' <param name="snippetTitle">The code snippet title</param>
        ''' <param name="snippetDescription">The code snippet description</param>
        ''' <param name="snippetHelpUrl">The code snippet help URL</param>
        ''' <param name="snippetAuthor">The code snippet author</param>
        ''' <param name="snippetShortcut">The code snippet shortcut</param>
        ''' <param name="snippetCode">The source code text of the snippet</param>
        ''' <param name="importDirectives">A collection of Imports directives (VB Only). Use null for C#</param>
        ''' <param name="references">A collection of Assembly references (VB Only). Use null for C#</param>
        ''' <param name="declarations">A collection of replacements that will be highlighted in the IntelliSense</param>
        ''' <param name="keywords">A collection of keywords that IntelliSense may use to identify the snippet</param>
        Public Sub SaveSnippet(fileName As String, kind As CodeSnippetKinds,
                                snippetLanguage As SnippetLanguages, snippetTitle As String,
                                snippetDescription As String, snippetHelpUrl As String,
                                snippetAuthor As String, snippetShortcut As String,
                                snippetCode As String, importDirectives As [Imports],
                                references As References, declarations As Declarations,
                                keywords As IEnumerable(Of String))

            Dim snippetKind As String = ReturnSnippetKind(kind)
            Dim codeSnippetLanguage As String = ReturnSnippetLanguage(snippetLanguage)

            Dim editedCode = snippetCode
            For Each decl In declarations
                editedCode = editedCode.Replace(decl.Default, "$" & decl.ID & "$")
            Next

            If keywords Is Nothing Then
                keywords = New List(Of String) From {String.Empty}
            End If

            Dim cdata As New XCData(snippetCode)
            Dim doc = <?xml version="1.0" encoding="utf-8"?>
                      <CodeSnippets xmlns="http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet">
                          <CodeSnippet Format="1.0.0">
                              <Header>
                                  <Title><%= snippetTitle %></Title>
                                  <Author><%= snippetAuthor %></Author>
                                  <Description><%= snippetDescription %></Description>
                                  <HelpUrl><%= snippetHelpUrl %></HelpUrl>
                                  <SnippetTypes>
                                      <SnippetType>Expansion</SnippetType>
                                  </SnippetTypes>
                                  <Keywords>
                                      <%= From key In keywords
                                          Select <Keyword><%= key %></Keyword> %>
                                  </Keywords>
                                  <Shortcut><%= snippetShortcut %></Shortcut>
                              </Header>
                              <Snippet>
                                  <References>
                                      <%= From ref In references
                                          Select <Reference>
                                                     <Assembly><%= ref.Assembly %></Assembly>
                                                     <Url><%= ref.Url %></Url>
                                                 </Reference> %>
                                  </References>
                                  <Imports>
                                      <%= From imp In importDirectives
                                          Select <Import>
                                                     <Namespace><%= imp.ImportDirective %></Namespace>
                                                 </Import> %>
                                  </Imports>
                                  <Declarations>
                                      <%= From decl In declarations
                                          Where decl.ReplacementType.ToLower = "object"
                                          Select <Object Editable="true">
                                                     <ID><%= decl.ID %></ID>
                                                     <Type><%= decl.Type %></Type>
                                                     <ToolTip><%= decl.ToolTip %></ToolTip>
                                                     <Default><%= decl.Default %></Default>
                                                     <Function><%= decl.Function %></Function>
                                                 </Object> %>
                                      <%= From decl In declarations
                                          Where decl.ReplacementType.ToLower = "literal"
                                          Select <Literal Editable="true">
                                                     <ID><%= decl.ID %></ID>
                                                     <ToolTip><%= decl.ToolTip %></ToolTip>
                                                     <Default><%= decl.Default %></Default>
                                                     <Function><%= decl.Function %></Function>
                                                 </Literal> %>
                                  </Declarations>
                                  <Code Language=<%= codeSnippetLanguage %> Kind=<%= snippetKind %>
                                      Delimiter="$"></Code>
                              </Snippet>
                          </CodeSnippet>
                      </CodeSnippets>

            doc...<Code>.First.Add(cdata)

            doc.Save(fileName)
        End Sub
    End Class

    ''' <summary>
    ''' Enumerate the code snippet kinds. 
    ''' Kind means the code snippet can be a method body, 
    ''' a method declaration, a type declaration, an entire source file,
    ''' or any other kind
    ''' </summary>
    Public Enum CodeSnippetKinds
        MethodBody
        MethodDeclaration
        TypeDeclaration
        File
        Any
    End Enum

    ''' <summary>
    ''' Enumerate the supported programming languages
    ''' </summary>
    Public Enum SnippetLanguages
        VB
        CSharp
        SQL
        XML
        CPP
        JavaScript
        HTML
    End Enum

    ''' <summary>
    ''' Represent an Imports directive (VB only). 
    ''' The XML schema for code snippets does not support C# using
    ''' </summary>
    Public Class Import
        ''' <summary>
        ''' The namespace without the Imports keyword (e.g. System.Diagnostics)
        ''' </summary>
        ''' <returns>String</returns>
        Property ImportDirective As String
    End Class

    ''' <summary>
    ''' A collection of <seealso cref="Import"/> directives/>
    ''' </summary>
    Public Class [Imports]
        Inherits ObservableCollection(Of Import)
    End Class

    ''' <summary>
    ''' Represent a reference to a .NET assembly (VB Only).
    ''' The XML schema for code snippets does not support C# references
    ''' </summary>
    Public Class Reference
        Property Assembly As String
        Property Url As String
    End Class

    ''' <summary>
    ''' A collection of <seealso cref="Reference"/> objects
    ''' </summary>
    Public Class References
        Inherits ObservableCollection(Of Reference)
    End Class

    ''' <summary>
    ''' Represent a code replacement. Replacements are highlighted in the code editor and the user can
    ''' follow the suggestions offered by IntelliSense
    ''' </summary>
    Public Class Declaration
        Implements INotifyPropertyChanged

        Property Editable As Boolean = True

        Private _id As String

        ''' <summary>
        ''' The replacement ID
        ''' </summary>
        Property ID As String
            Get
                Return _id
            End Get
            Set(value As String)
                _id = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ID)))
            End Set
        End Property

        Private _type As String
        ''' <summary>
        ''' The .NET type of the object 
        ''' </summary>
        ''' <returns></returns>
        Property [Type] As String
            Get
                Return _type
            End Get
            Set(value As String)
                _type = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Type)))
            End Set
        End Property

        Private _toolTip As String
        ''' <summary>
        ''' A description explaining how the user can replace the code
        ''' </summary>
        ''' <returns></returns>
        Property ToolTip As String
            Get
                Return _toolTip
            End Get
            Set(value As String)
                _toolTip = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ToolTip)))
            End Set
        End Property

        Private _default As String
        ''' <summary>
        ''' The default value for the replacement
        ''' </summary>
        ''' <returns></returns>
        Property [Default] As String
            Get
                Return _default
            End Get
            Set(value As String)
                _default = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf([Default])))
            End Set
        End Property

        Private _function As String
        ''' <summary>
        ''' A method that will be invoked when the snippet is inserted
        ''' </summary>
        ''' <returns></returns>
        Property [Function] As String
            Get
                Return _function
            End Get
            Set(value As String)
                _function = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf([Function])))
            End Set
        End Property

        Property ReplacementType As String = "Literal"

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class

    ''' <summary>
    ''' A collection of <seealso cref="Declaration"/> objects
    ''' </summary>
    Public Class Declarations
        Inherits ObservableCollection(Of Declaration)
    End Class
End Namespace
