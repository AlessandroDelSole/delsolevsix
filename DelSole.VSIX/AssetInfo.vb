Imports System.Collections.ObjectModel

Public Class AssetInfo
    Public Property Type As String
    Public Property Path As String
End Class

Public Class AssetInfoCollection
    Inherits ObservableCollection(Of AssetInfo)
End Class
