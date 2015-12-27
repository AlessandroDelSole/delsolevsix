Imports System.Collections.ObjectModel
Imports System.ComponentModel

Namespace SnippetTools
    Public Class SnippetLibrary
        Implements System.ComponentModel.INotifyPropertyChanged

        Private _folders As ObservableCollection(Of SnippetFolder)

        Public Sub New()
            Me.Folders = New ObservableCollection(Of SnippetFolder)
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        ''' <summary>
        ''' Return the collection of folders in the library
        ''' </summary>
        ''' <returns></returns>
        Public Property Folders As ObservableCollection(Of SnippetFolder)
            Get
                Return _folders
            End Get
            Set(value As ObservableCollection(Of SnippetFolder))
                _folders = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Folders)))
            End Set
        End Property

        ''' <summary>
        ''' Return the full list of code snippets inside the library. 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SnippetFiles As IEnumerable(Of String)
            Get

                If Not Me.Folders.Any Then
                    Throw New InvalidOperationException("The folder collection is empty.")
                End If

                Dim fileNames As New List(Of String)
                For Each folder In Me.Folders
                    Dim files = IO.Directory.EnumerateFiles(folder.FolderName)
                    For Each fileName In files
                        fileNames.Add(fileName)
                    Next
                Next
                Return fileNames.AsEnumerable
            End Get
        End Property

        ''' <summary>
        ''' Save a library of folders containing code snippets
        ''' </summary>
        ''' <param name="pathName"></param>
        Public Sub SaveLibrary(pathName As String)
            Dim doc = <?xml version="1.0" encoding="utf-8"?>
                      <Folders>
                          <%= From fold In Me.Folders
                              Select <Folder FolderName=<%= fold.FolderName %>/> %>
                      </Folders>

            doc.Save(pathName)
        End Sub

        ''' <summary>
        ''' Load a snippet library list from disk. Make sure you load a list that was previously saved with the SaveLibrary method.
        ''' </summary>
        ''' <param name="pathName"></param>
        Public Sub LoadLibrary(pathName As String)
            Dim doc = XDocument.Load(pathName)
            Dim query = From fold In doc...<Folder>
                        Select fold.@FolderName

            For Each element In query
                Dim folder As New SnippetFolder
                folder.FolderName = element
                Me.Folders.Add(folder)
            Next
        End Sub
    End Class
End Namespace
