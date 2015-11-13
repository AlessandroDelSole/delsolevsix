#Introducing the library

The DelSole.VSIX library exposes the same-named namespace, which offers classes that enable generating a compiled .vsix package. 
At this time, the library only allows generating .vsix package that only contain IntelliSense code snippets for Visual Studio (.snippet, .cssnippet, .vbsnippet, .*snippet)

 #Understanding the VSIX format
 The VSIX (.vsix) file format is nothing but a .zip archive based on the Open XML conventions. A .vsix package must contain a .vsixmanifest XML file, which contains the package metadata and the list of files that must be packaged.
 Also, a .vsix must containg a package definition file (.pkgdef) that makes Visual Studio understand what kind of contents the package offers and where they must be installed. The library automatically creates a .vsixmanifest and
 .pkgdef file for you.
 
 #Representing code snippets
 Before you create a .vsix package, you need to represent each code snippet file you want to package into the .vsix in a .NET fashion. To accomplish this, you use the DelSole.VSIX.SnippetInfo class. 
 An instance of this class represents a single .snippet file. Following is an example:
 
 ```csharp
             //Create a SnippetInfo for each code snippet you want to package
            var Snippet1 = new SnippetInfo();
            Snippet1.SnippetFileName = "UnprotectDocument.snippet";
            Snippet1.SnippetPath = "C:\\MySnippets";
            Snippet1.SnippetLanguage = SnippetInfo.GetSnippetLanguage(Snippet1.SnippetPathName);
            Snippet1.SnippetDescription = SnippetInfo.GetSnippetDescription(Snippet1.SnippetFileName);
 ```
