Imports System.Collections.ObjectModel
Public Class DependencyInfo
    Public Property Id As String
    Public Property DisplayName As String
    Public Property Version As String
End Class

Public Class DependencyInfoCollection
    Inherits ObservableCollection(Of DependencyInfo)
End Class
