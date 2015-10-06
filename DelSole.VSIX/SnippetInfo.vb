Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports <xmlns="http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet">

''' <summary>
''' Represents full-fidelity information for a code snippet
''' </summary>
Public Class SnippetInfo
    Implements IDataErrorInfo

    ''' <summary>
    ''' A code snippet's file name
    ''' </summary>
    ''' <returns>String</returns>
    Public Property SnippetFileName As String
    ''' <summary>
    ''' The programming language the snippet targets
    ''' </summary>
    ''' <returns>String</returns>
    Public Property SnippetLanguage As String
    ''' <summary>
    ''' The snippet's path on disk
    ''' </summary>
    ''' <returns>String</returns>
    Public Property SnippetPath As String
    ''' <summary>
    ''' The snippet's description
    ''' </summary>
    ''' <returns>String</returns>
    Public Property SnippetDescription As String

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
                Return String.Format("{0} data is invalid.", TypeName(Me))
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
            Case Is = "SnippetFileName"
                If Me.SnippetFileName = "" Or String.IsNullOrEmpty(Me.SnippetFileName) Then
                    Me.AddError("SnippetFileName", "Value cannot be null")
                Else
                    Me.RemoveError("SnippetFileName")
                End If
            Case Is = "SnippetLanguage"
                If Me.SnippetLanguage = "" Or String.IsNullOrEmpty(Me.SnippetLanguage) Then
                    Me.AddError("SnippetLanguage", "Value cannot be null")
                Else
                    Me.RemoveError("SnippetLanguage")
                End If
            Case Is = "SnippetPath"
                If Me.SnippetPath = "" Or String.IsNullOrEmpty(Me.SnippetPath) Then
                    Me.AddError("SnippetPath", "Value cannot be null")
                Else
                    Me.RemoveError("SnippetPath")
                End If
        End Select
    End Sub

    ''' <summary>
    ''' Create an instance of the class, supplying null values
    ''' for every property
    ''' </summary>
    Public Sub New()
        Me.SnippetLanguage = Nothing
        Me.SnippetFileName = Nothing
        Me.SnippetPath = Nothing
        Me.SnippetDescription = Nothing
    End Sub

    ''' <summary>
    ''' Detect the target programming language
    ''' for the specified snippet file
    ''' </summary>
    ''' <param name="snippetFile"></param>
    ''' <returns></returns>
    Public Shared Function GetSnippetLanguage(snippetFile As String) As String
        Dim doc As XDocument = XDocument.Load(snippetFile)

        Dim query = From line In doc...<Code>
                    Select line.@Language

        Return query.FirstOrDefault.ToUpper

    End Function

    ''' <summary>
    ''' Detect the description for the specified snippet file
    ''' </summary>
    ''' <param name="snippetFile"></param>
    ''' <returns></returns>
    Public Shared Function GetSnippetDescription(snippetFile As String) As String
        Dim doc As XDocument = XDocument.Load(snippetFile)

        Dim query = From line In doc...<Description>
                    Select line.Value

        Return query.FirstOrDefault

    End Function
End Class

''' <summary>
''' A collection of <seealso cref="SnippetInfo"></seealso> objects/>
''' </summary>
Public Class SnippetInfoCollection
    Inherits ObservableCollection(Of SnippetInfo)
End Class

