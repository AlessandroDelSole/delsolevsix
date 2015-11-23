Imports System.ComponentModel
Imports <xmlns="http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet">

Namespace SnippetTools
    ''' <summary>
    ''' Represent a code snippet and expose APIs to create and save a code snippet file. 
    ''' It differs from <seealso cref="SnippetInfo"/> which instead serves as a building block for generating .Vsix packages
    ''' </summary>
    Public Class CodeSnippet
        Implements INotifyPropertyChanged
        Implements IDataErrorInfo

        Private _author As String
        Private _title As String
        Private _description As String
        Private _helpUrl As String
        Private _shortcut As String
        Private _keywords As String
        Private _kind As CodeSnippetKinds
        Private _namespaces As [Imports]
        Private _references As References
        Private _declarations As Declarations
        Private _code As String
        Private _language As String

        ''' <summary>
        ''' The code snippet author
        ''' </summary>
        ''' <returns>String</returns>
        <Category("Properties")>
        <DisplayName("Author")>
        <Description("Author of code snippet. This is a required value.")>
        Public Property Author As String
            Get
                Return _author
            End Get
            Set(value As String)
                _author = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Author)))
            End Set
        End Property

        ''' <summary>
        ''' The code snippet title
        ''' </summary>
        ''' <returns>String</returns>
        <Category("Properties")>
        <DisplayName("Title")>
        <Description("Title of code snippet. This is a required value.")>
        Public Property Title As String
            Get
                Return _title
            End Get
            Set(value As String)
                _title = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Title)))
            End Set
        End Property

        ''' <summary>
        ''' The code snippet description
        ''' </summary>
        ''' <returns>String</returns>
        <Category("Properties")>
        <DisplayName("Description")>
        <Description("Description of code snippet. This is a required value.")>
        Public Property Description As String
            Get
                Return _description
            End Get
            Set(value As String)
                _description = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Description)))
            End Set
        End Property

        ''' <summary>
        ''' An optional URL that users can search for more information
        ''' </summary>
        ''' <returns>String</returns>
        <Category("Properties")>
        <DisplayName("HelpUrl")>
        <Description("URL where users can find help. This is an optional value.")>
        Public Property HelpUrl As String
            Get
                Return _helpUrl
            End Get
            Set(value As String)
                _helpUrl = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(HelpUrl)))
            End Set
        End Property

        ''' <summary>
        ''' A keyboard shortcut that users can use to insert the code snippet quickly
        ''' </summary>
        ''' <returns></returns>
        <Category("Properties")>
        <DisplayName("Shortcut")>
        <Description("Keyboard shortcut for IntelliSense. This is an optional value.")>
        Public Property Shortcut As String
            Get
                Return _shortcut
            End Get
            Set(value As String)
                _shortcut = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Shortcut)))
            End Set
        End Property

        ''' <summary>
        ''' A comma-separated list of words that help categorize a code snippet
        ''' </summary>
        ''' <returns></returns>
        <Category("Properties")>
        <DisplayName("Keywords")>
        <Description("Comma separated list of keywords that help identifying a code snippet.")>
        Public Property Keywords As String
            Get
                Return _keywords
            End Get
            Set(value As String)
                _keywords = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Keywords)))
            End Set
        End Property

        ''' <summary>
        ''' Represent the kind of code snippet
        ''' </summary>
        ''' <returns><seealso cref="CodeSnippetKinds"/></returns>
        <Category("Properties")>
        <DisplayName("Kind")>
        <Description("Determine the kind of code snippet.")>
        Public Property Kind As CodeSnippetKinds
            Get
                Return _kind
            End Get
            Set(value As CodeSnippetKinds)
                _kind = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Kind)))
            End Set
        End Property

        ''' <summary>
        ''' VB Only: a list of namespaces that are required for the code snippet to work, without the Imports keyword
        ''' </summary>
        ''' <returns><seealso cref="[Imports]"/></returns>
        <Category("Properties")>
        <DisplayName("Namespaces")>
        <Description("The list of VB namespaces that the code snippet requires to work.")>
        Public Property Namespaces As [Imports]
            Get
                Return _namespaces
            End Get
            Set(value As [Imports])
                _namespaces = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Namespaces)))
            End Set
        End Property

        ''' <summary>
        ''' VB only: a list of assembly references that are required for the code snippet to work
        ''' </summary>
        ''' <returns><seealso cref="References"/></returns>
        <Category("Properties")>
        <DisplayName("References")>
        <Description("The list of assembly references that the code snippet requires to work (VB only).")>
        Public Property References As References
            Get
                Return _references
            End Get
            Set(value As References)
                _references = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(References)))
            End Set
        End Property

        ''' <summary>
        ''' The list of identifiers/words and related suggestions the IntelliSense shows 
        ''' </summary>
        ''' <returns><seealso cref="Declarations"/></returns>
        <Category("Properties")>
        <DisplayName("Declarations")>
        <Description("The list of words/suggestions that the code editor will display.")>
        Public Property Declarations As Declarations
            Get
                Return _declarations
            End Get
            Set(value As Declarations)
                _declarations = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Declarations)))
            End Set
        End Property

        ''' <summary>
        ''' The actual source code of the snippet
        ''' </summary>
        ''' <returns>String</returns>
        <Category("Properties")>
        <DisplayName("Code")>
        <Description("The actual code for the code snippet.")>
        Public Property Code As String
            Get
                Return _code
            End Get
            Set(value As String)
                _code = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Code)))
            End Set
        End Property

        ''' <summary>
        ''' The programming language the code snippet is written with.
        ''' Supported values are: VB, CSHARP, SQL, XML, CPP, HTML, JAVASCRIPT
        ''' </summary>
        ''' <returns></returns>
        <Category("Properties")>
        <DisplayName("Language")>
        <Description("The programming language the snippet is written with.")>
        Public Property Language As String
            Get
                Return _language
            End Get
            Set(value As String)
                _language = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Language)))
            End Set
        End Property

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
                Return (validationErrors.Any)
            End Get
        End Property

        ''' <summary>
        ''' Return an error message
        ''' </summary>
        ''' <returns>String</returns>
        Public ReadOnly Property [Error]() As String _
        Implements System.ComponentModel.IDataErrorInfo.Error
            Get
                If validationErrors.Any Then
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
                Case Is = "Title"
                    If Me.Title = "" Or String.IsNullOrEmpty(Me.Title) Then
                        Me.AddError("Title", "Value cannot be null")
                    Else
                        Me.RemoveError("Title")
                    End If
                Case Is = "Code"
                    If Me.Author = "" Or String.IsNullOrEmpty(Me.Author) Then
                        Me.AddError("Code", "Value cannot be null")
                    Else
                        Me.RemoveError("Code")
                    End If
                Case Is = "Author"
                    If Me.Author = "" Or String.IsNullOrEmpty(Me.Author) Then
                        Me.AddError("Author", "Value cannot be null")
                    Else
                        Me.RemoveError("Author")
                    End If
                Case Is = "Language"
                    If Me.Language = "" Or String.IsNullOrEmpty(Me.Language) Then
                        Me.AddError("Language", "Value cannot be null")
                    ElseIf supportedLanguages.Contains(Language.ToUpper) = False
                        Me.AddError("Language", $"{Language} language is not supported")
                    Else
                        Me.RemoveError("Language")
                    End If
            End Select
        End Sub

        ''' <summary>
        ''' An array representing the supported programming languages
        ''' </summary>
        Private supportedLanguages() As String = {"VB", "CSHARP", "SQL", "XML", "CPP", "HTML", "JAVASCRIPT"}

        ''' <summary>
        ''' Raised when the value of a property changes.
        ''' </summary>
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        ''' <summary>
        ''' Type initialization
        ''' </summary>
        Public Sub New()
            Keywords = ""
            HelpUrl = ""
            Kind = CodeSnippetKinds.MethodBody
            Namespaces = New [Imports]
            References = New References
            Declarations = New Declarations
        End Sub

        ''' <summary>
        ''' Return a schema-compliant string representing the code snippet kind
        ''' </summary>
        ''' <param name="kind"></param>
        ''' <returns>String</returns>
        Private Shared Function ReturnSnippetKind(kind As CodeSnippetKinds) As String
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
        ''' Create and save a code snippet file to disk
        ''' </summary>
        ''' <param name="fileName">The target snippet file name</param>
        ''' <param name="snippetData">The instance of <seealso cref="CodeSnippet"/> </param>
        Public Shared Sub SaveSnippet(fileName As String, snippetData As CodeSnippet)
            Dim snippetKind As String = ReturnSnippetKind(snippetData.Kind)

            Dim editedCode = snippetData.Code

            If snippetData.Declarations.Any Then
                For Each decl In snippetData.Declarations
                    editedCode = editedCode.Replace(decl.Default, "$" & decl.ID & "$")
                Next
            End If

            Dim keywords = snippetData.Keywords.Split(","c).AsEnumerable

            Dim cdata As New XCData(editedCode)
            Dim doc = <?xml version="1.0" encoding="utf-8"?>
                      <CodeSnippets xmlns="http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet">
                          <CodeSnippet Format="1.0.0">
                              <Header>
                                  <Title><%= snippetData.Title %></Title>
                                  <Author><%= snippetData.Author %></Author>
                                  <Description><%= snippetData.Description %></Description>
                                  <HelpUrl><%= snippetData.HelpUrl %></HelpUrl>
                                  <SnippetTypes>
                                      <SnippetType>Expansion</SnippetType>
                                  </SnippetTypes>
                                  <Keywords>
                                      <%= From key In snippetData.Keywords
                                          Select <Keyword><%= key %></Keyword> %>
                                  </Keywords>
                                  <Shortcut><%= snippetData.Shortcut %></Shortcut>
                              </Header>
                              <Snippet>
                                  <References>
                                      <%= From ref In snippetData.References
                                          Select <Reference>
                                                     <Assembly><%= ref.Assembly %></Assembly>
                                                     <Url><%= ref.Url %></Url>
                                                 </Reference> %>
                                  </References>
                                  <Imports>
                                      <%= From imp In snippetData.Namespaces
                                          Select <Import>
                                                     <Namespace><%= imp.ImportDirective %></Namespace>
                                                 </Import> %>
                                  </Imports>
                                  <Declarations>
                                      <%= From decl In snippetData.Declarations
                                          Where decl.ReplacementType.ToLower = "object"
                                          Select <Object Editable="true">
                                                     <ID><%= decl.ID %></ID>
                                                     <Type><%= decl.Type %></Type>
                                                     <ToolTip><%= decl.ToolTip %></ToolTip>
                                                     <Default><%= decl.Default %></Default>
                                                     <Function><%= decl.Function %></Function>
                                                 </Object> %>
                                      <%= From decl In snippetData.Declarations
                                          Where decl.ReplacementType.ToLower = "literal"
                                          Select <Literal Editable="true">
                                                     <ID><%= decl.ID %></ID>
                                                     <ToolTip><%= decl.ToolTip %></ToolTip>
                                                     <Default><%= decl.Default %></Default>
                                                     <Function><%= decl.Function %></Function>
                                                 </Literal> %>
                                  </Declarations>
                                  <Code Language=<%= snippetData.Language %> Kind=<%= snippetKind %>
                                      Delimiter="$"><%= cdata %></Code>
                              </Snippet>
                          </CodeSnippet>
                      </CodeSnippets>
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
End Namespace
