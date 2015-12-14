Imports System.ComponentModel
Namespace SnippetTools
    Public Class SnippetFolder
        Implements INotifyPropertyChanged

        Private _folderName As String
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Public Property FolderName As String
            Get
                Return _folderName
            End Get
            Set(value As String)
                _folderName = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(FolderName)))
            End Set
        End Property
    End Class

End Namespace
