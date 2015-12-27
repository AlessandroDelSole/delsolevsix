Imports System.Collections.ObjectModel
Imports System.ComponentModel
Namespace SnippetTools
    Public Class SnippetFolder
        Implements INotifyPropertyChanged

        Private _folderName As String
        Private _subFolders As ObservableCollection(Of SnippetFolder)
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        ''' <summary>
        ''' The full directory name
        ''' </summary>
        ''' <returns></returns>
        Public Property FolderName As String
            Get
                Return _folderName
            End Get
            Set(value As String)
                _folderName = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(FolderName)))
            End Set
        End Property

        ''' <summary>
        ''' Return the list of snippet file names in the current folder
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SnippetFiles As IEnumerable(Of CodeSnippet)
            Get
                Dim files = IO.Directory.EnumerateFiles(FolderName, "*.*snippet")
                If files.Any Then
                    Dim snips As New List(Of CodeSnippet)
                    For Each snip In files
                        Dim newSnip = CodeSnippet.LoadSnippet(snip)
                        snips.Add(newSnip)
                    Next
                    Return snips.AsEnumerable
                Else
                    Return Nothing
                End If
            End Get
        End Property
    End Class
End Namespace
