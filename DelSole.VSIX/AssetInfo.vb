Imports System.Collections.ObjectModel

Friend Class AssetInfo
    Public Property Type As String
    Public Property Path As String
End Class

Friend Class AssetInfoCollection
    Inherits ObservableCollection(Of AssetInfo)
End Class
