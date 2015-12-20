
#Understanding the VSIX format
The VSIX (.vsix) file format allows packaging multiple Visual Studio extensions into one installer. With .vsix packages, you can easily share VS packages, editor extensions, language services, tool windows, project/item templates and, of course, code snippet files.

A .vsix file is nothing but a .zip archive based on the Open XML conventions. A .vsix package must contain:

- a .vsixmanifest XML file, which contains the package metadata and the list of files that must be packaged.
- a package definition file (.pkgdef) that makes Visual Studio understand what kind of contents the package offers and where they must be installed.
- files that will be installed as Visual Studio extensions (typically .dll and .snippet files).

The library helps generating .vsix packages for sharing and installing code snippets; it automatically creates a .vsixmanifest and .pkgdef file for you, and packages all the supplied code snippets into the final .vsix.
 
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
 
#Opening an existing .vsix package
You can open an existing .vsix package and get an instance of the `VSIXPackage` class invoking the `OpenVsix` method and passing the .vsix file name as an argument. The operation succeeds if the .vsix package **only** contains code snippets. Following is an example:

   ```csharp
VSIXPackage vsix = VSIXPackage.OpenVsix("C:\\temp\\MyPackage.vsix");
 ```

#Additional Tools
The `DelSole.VSIX.VSIXPackage` class exposes other methods to interact with a .vsix package, such as `SignVsix` (to add a digital signature) and `ExtractVsix` (to extract the content of a package)

##Signing a .vsix package ##

VSIX packages can be digitally signed with a X.509 certificate, through a password protected .pfx file. The `DelSole.VSIXPackage` class allows signing an existing .vsix package by invoking the `SignVsix` static method:

```csharp
    DelSole.VSIXPackage.SignVsix("inputPackage.vsix", "certFile.pfx", "password");
```

Where:
`inputPackage.vsix` is the file name for the .vsix package to be signed
`certFile.pfx` is the digital signature

The method has two overloads: one that accepts an object of type `SecureString` as the password, and one that accepts a regular string as the password.

##Extracting a .vsix package ##

You can extract the whole content of a .vsix package or only the code snippet it contains invoking the `DelSole.VSIXPackage.ExtractVsix` static method:

  ```csharp
    DelSole.VSIXPackage.ExtractVsix("inputFile.vsix", "C:\targetFolder", true);
 ```

If the third argument is `true`, the operation will only extract code snippets (.snippet) contained in the .vsix package. If `false`, this operation will fully extract a .vsix package, including manifest and package definition.


#Other resources
See also:

[Reference source for the VSIXPackage class](http://delsolevsixrefsource.azurewebsites.net/#DelSole.VSIX/Vsix_ObjectModel/VSIXPackage.vb)

[Working with code snippet files](https://github.com/AlessandroDelSole/delsolevsix/blob/master/docs/WorkingWithCodeSnippets.md)

[Working with old-style .vsi archives](https://github.com/AlessandroDelSole/delsolevsix/blob/master/docs/WorkingWithOldVsiArchives.md)

