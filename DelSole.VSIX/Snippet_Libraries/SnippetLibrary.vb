Imports System.Collections.ObjectModel

Namespace SnippetTools
    Public Class SnippetLibrary
        Inherits ObservableCollection(Of SnippetFolder)

        ''' <summary>
        ''' Return the full list of code snippets inside the library. 
        ''' </summary>
        ''' <returns></returns>
        Public Function ReturnSnippets() As IEnumerable(Of String)
            If Me.Items Is Nothing Then
                Throw New InvalidOperationException("The folder collection is empty.")
            End If

            Dim fileNames As New List(Of String)
            For Each folder In Me
                Dim files = IO.Directory.EnumerateFiles(folder.FolderName)
                For Each fileName In files
                    fileNames.Add(IO.Path.Combine(folder.FolderName, fileName))
                Next
            Next
            Return fileNames.AsEnumerable
        End Function

        ''' <summary>
        ''' Return the full list of code snippets inside the specified folder. 
        ''' </summary>
        ''' <param name="folderName"></param>
        ''' <returns></returns>
        Public Function ReturnSnippets(folderName As String) As IEnumerable(Of String)
            If Me.Items Is Nothing Then
                Throw New InvalidOperationException("The folder collection is empty.")
            End If

            Dim query = (From fold In Me
                         Where fold.FolderName.ToLower = folderName.ToLower
                         Select fold).FirstOrDefault

            If query IsNot Nothing Then
                Dim fileNames As New List(Of String)
                Dim files = IO.Directory.EnumerateFiles(query.FolderName)
                For Each fileName In files
                    fileNames.Add(IO.Path.Combine(query.FolderName, fileName))
                Next
                Return fileNames.AsEnumerable
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Return the full list of code snippets inside the specified folder. 
        ''' </summary>
        ''' <param name="folderInfo"></param>
        ''' <returns></returns>
        Public Function ReturnSnippets(folderInfo As SnippetFolder) As IEnumerable(Of String)
            If Me.Items Is Nothing Then
                Throw New InvalidOperationException("The folder collection is empty.")
            End If

            Dim query = (From fold In Me
                         Where fold.FolderName.ToLower = folderInfo.FolderName.ToLower
                         Select fold).FirstOrDefault

            If query IsNot Nothing Then
                Dim fileNames As New List(Of String)
                Dim files = IO.Directory.EnumerateFiles(query.FolderName)
                For Each fileName In files
                    fileNames.Add(IO.Path.Combine(query.FolderName, fileName))
                Next
                Return fileNames.AsEnumerable
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Save a library of folders containing code snippets
        ''' </summary>
        ''' <param name="pathName"></param>
        Public Sub SaveLibrary(pathName As String)
            Dim doc = <?xml version="1.0" encoding="utf-8"?>
                      <Folders>
                          <%= From fold In Me
                              Select <Folder FolderName=<%= fold.FolderName %>/> %>
                      </Folders>

            doc.Save(pathName)
        End Sub

        ''' <summary>
        ''' Load a snippet library list from disk. Make sure you load a list that was previously saved with the <seealso cref="SaveLibrary(String)"/> method.
        ''' </summary>
        ''' <param name="pathName"></param>
        Public Sub LoadLibrary(pathName As String)
            Dim doc = XDocument.Load(pathName)
            Dim query = From fold In doc...<Folder>
                        Select fold.@FolderName

            For Each element In query
                Dim folder As New SnippetFolder
                folder.FolderName = element
                Me.Add(folder)
            Next
        End Sub
    End Class
End Namespace
