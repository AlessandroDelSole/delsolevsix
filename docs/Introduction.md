
# Introducing the library

The DelSole.VSIX library exposes three namespaces:

`DelSole.VSIX`, which offers classes that enable generating a compiled .vsix package for both Visual Basic and C#. 
At this time, the library only allows generating .vsix package that only contain IntelliSense code snippets for Visual Studio (.snippet, .cssnippet, .vbsnippet, .snippet). The library now also supports generating .vsix packages that contain code snippets for Visual Studio Code. At this time, only one snippet per package is allowed but this is going to be enhanced in the future.

`DelSole.VSIX.SnippetTools`, which offers classes that enable generating reusable IntelliSense code snippets for Visual Studio (.snippet files) and Visual Studio Code (.json files).

`DelSole.VSIX.VsiTools`, which offers classes that help interacting with the old-fashioned .vsi (Visual Studio Community Content Installer) packages, such as for converting .vsi into .vsix or extracting .vsi contents.

The library is available for Visual Studio 2015 to both Visual Basic and C# and can be [downloaded from NuGet](https://www.nuget.org/packages/DelSole.VSIX)
