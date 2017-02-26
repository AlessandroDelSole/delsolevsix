Imports Newtonsoft.Json
Public Class CatalogJson
    <JsonProperty("manifestVersion")>
    Public Property ManifestVersion As String
    <JsonProperty("info")>
    Public Property Info As String
    <JsonProperty("packages")>
    Public Property Packages As List(Of CatalogJsonFile)
End Class

Public Class CatalogJsonFile
    <JsonProperty("id")>
    Public Property Id As String
    <JsonProperty("version")>
    Public Property Version As String
    <JsonProperty("type")>
    Public Property Type As String
    <JsonProperty("extension")>
    Public Property Extension As Boolean
    <JsonProperty("extensionDir")>
    Public Property ExtensionDir As String
    <JsonProperty("installSize")>
    Public Property InstallSize As Long
    <JsonProperty("dependencies")>
    Public Property Dependencies As String
    <JsonProperty("localizedResources")>
    Public Property LocalizedResources As List(Of CatalogLocalizedResource)
    <JsonProperty("payloads")>
    Public Property Payloads As List(Of CatalogPayload)
    <JsonProperty("vsixId")>
    Public Property VsixId As String

    Public Sub New()
        Me.LocalizedResources = New List(Of CatalogLocalizedResource)
        Me.Payloads = New List(Of CatalogPayload)
    End Sub
End Class

Public Class CatalogLocalizedResource
    <JsonProperty("language")>
    Public Property Language As String
    <JsonProperty("title")>
    Public Property Title As String
    <JsonProperty("description")>
    Public Property Description As String
End Class

Public Class CatalogPayload
    <JsonProperty("fileName")>
    Public Property FileName As String
    <JsonProperty("size")>
    Public Property Size As Long
End Class