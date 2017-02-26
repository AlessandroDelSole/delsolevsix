Imports Newtonsoft.Json

Public Class ManifestJson
    <JsonProperty("id")>
    Public Property Id As String
    <JsonProperty("version")>
    Public Property Version As String
    <JsonProperty("type")>
    Public Property Type As String
    <JsonProperty("vsixId")>
    Public Property VsixId As String
    <JsonProperty("extensionDir")>
    Public Property ExtensionDir As String
    <JsonProperty("installSize")>
    Public Property InstallSize As Long
    <JsonProperty("dependencies")>
    Public Property Dependencies As String
    <JsonProperty("files")>
    Public Property Files As List(Of ManifestJsonFile)

    Public Sub New()
        Me.Files = New List(Of ManifestJsonFile)
    End Sub
End Class


Public Class ManifestJsonFile
    Public Property FileName As String
    Public Property Sha256 As String
End Class

