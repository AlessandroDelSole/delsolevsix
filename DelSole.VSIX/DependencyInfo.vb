Imports System.Collections.ObjectModel
Friend Class DependencyInfo
    Public Property Id As String
    Public Property DisplayName As String
    Public Property Version As String
End Class

Friend Class DependencyInfoCollection
    Inherits ObservableCollection(Of DependencyInfo)
End Class
