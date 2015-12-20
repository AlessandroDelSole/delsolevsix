
# Introducing the library

The DelSole.VSIX library allows creating and sharing reusable IntelliSense code snippets for both Visual Studio 2015 and Visual Studio Code. The DelSole.VSIX library exposes three namespaces:

`DelSole.VSIX`, which offers classes that enable generating a compiled .vsix package that makes it easy to share either code snippets for Visual Basic and C# or code snippets for Visual Studio Code. At this time, the library allows generating .vsix package that only contain IntelliSense code snippets, so no other kinds of extensions are supported. 

`DelSole.VSIX.SnippetTools`, which offers classes that enable generating reusable IntelliSense code snippets for Visual Studio (.snippet files) and Visual Studio Code (.json files).

`DelSole.VSIX.VsiTools`, which offers classes that help interacting with the old-fashioned .vsi (Visual Studio Community Content Installer) packages, such as for converting .vsi into .vsix or extracting .vsi contents.

The library is available for Visual Studio 2015 to both Visual Basic and C# and can be [downloaded from NuGet](https://www.nuget.org/packages/DelSole.VSIX)

## Reading the documentation
If you want to create, package and share code snippets for Visual Studio 2015, please read the following:

[Working with Code Snippets](https://github.com/AlessandroDelSole/delsolevsix/blob/master/docs/WorkingWithCodeSnippets.md)
[Packaging Snippets for Visual Studio](https://github.com/AlessandroDelSole/delsolevsix/blob/master/docs/PackagingSnippetsForVisualStudio.md)

If you want to create, package and share code snippets for Visual Studio Code, please read the following:
[Working with Code Snippets](https://github.com/AlessandroDelSole/delsolevsix/blob/master/docs/WorkingWithCodeSnippets.md)
[Packaging Snippets for Code](https://github.com/AlessandroDelSole/delsolevsix/blob/master/docs/PackagingSnippetsForCode.md)
