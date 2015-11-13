#Introducing the library

The DelSole.VSIX library exposes the same-named namespace, which offers classes that enable generating a compiled .vsix package for both Visual Basic and C#. 
At this time, the library only allows generating .vsix package that only contain IntelliSense code snippets for Visual Studio (.snippet, .cssnippet, .vbsnippet, .*snippet)

#Understanding the VSIX format
The VSIX (.vsix) file format is nothing but a .zip archive based on the Open XML conventions. A .vsix package must contain a .vsixmanifest XML file, which contains the package metadata and the list of files that must be packaged.
Also, a .vsix must containg a package definition file (.pkgdef) that makes Visual Studio understand what kind of contents the package offers and where they must be installed. The library automatically creates a .vsixmanifest and .pkgdef file for you.
 
#Representing code snippets
Before you create a .vsix package, you need to represent each code snippet file you want to package into the .vsix in a .NET fashion. To accomplish this, you use the `DelSole.VSIX.SnippetInfo` class. 
 An instance of this class represents a single .snippet file. Following is an example:
 
 ```csharp
             //Create a SnippetInfo for each code snippet you want to package
            var Snippet1 = new SnippetInfo();
            Snippet1.SnippetFileName = "UnprotectDocument.snippet";
            Snippet1.SnippetPath = "C:\\MySnippets";
            Snippet1.SnippetLanguage = SnippetInfo.GetSnippetLanguage(Snippet1.SnippetPathName);
            Snippet1.SnippetDescription = SnippetInfo.GetSnippetDescription(Snippet1.SnippetFileName);
 ```
The class exposes five properties:
`SnippetFileName`, which stores a code snippet's file name without its containing folder name
`SnippetPath`, which stores the name of the directory where the code snippet file is
`SnippetLanguage`, which stores the programming language a code snippet is written for. You do not retrieve the language manually. Instead, you invoke the static `SnippetInfo.GetSnippetLanguage` method, passing the full pathname of the code snippet file. The full pathname for a code snippet file is exposed by the `SnippetPathName` read-only property.
`SnippetDescription`, which stores the code snippet's description and that you retrieve by invoking the static `SnippetInfo.GetSnippetDescription` method, passing the full pathname of the code snippet file  
 
#Representing a VSIX package
Once you have any instances of the `SnippetInfo` class, you must create an instance of the `DelSole.VSIX.VSIXPackage` class, that represents a single .vsix. Once you have create an instance, you populate the `CodeSnippets` property with all the SnippetInfo instances created before:
 
  ```csharp
            //Create a new package
            var Vsix = new VSIXPackage();

            //Populate the collection of snippets
            Vsix.CodeSnippets.Add(Snippet1);
 ```
 
`CodeSnippets` is an object of type `SnippetInfoCollection` that, behind the scenes, is an ObservableCollection of SnippetInfo. The next step is setting the .vsix package metadata. Metadata contain information such as the package author, 
the product name, the package description, the version number, the license agreement, preview image and icon, an URL that folks can use to discover more information. Following is an example:
 
  ```csharp
            //Set package metadata information
            Vsix.Tags = "Word";
            Vsix.PackageAuthor = "Alessandro Del Sole";
            Vsix.PackageDescription = "A test VSIX package with snippets";
            Vsix.License = "C:\\temp\\MIT_License.txt";
            Vsix.MoreInfoURL = "https://github.com/alessandrodelsole/delsolevsix";
            //Assign other properties here...
 ```
 
#Building a VSIX package
Once you have populated the CodeSnippets property and once you have assigned the package metadata, you are ready to go! Building a .vsix is very simple, in fact you simply invoke the `VSIXPackage.Build` method, passing the target file name:
 
   ```csharp
            //Go build it!
            Vsix.Build("C:\\temp\\Sample.vsix");
 ```
 
#Other Tools
The DelSole.VSIX library also allows to digitally sign a .vsix package and to work with .vsi archives:

[Signing a .vsix package](https://github.com/alessandrodelsole/delsolevsix/docs/signingvsix.md)

[Extracting a .vsix package and Working with .vsi archives](https://github.com/alessandrodelsole/delsolevsix/docs/othertools.md)
