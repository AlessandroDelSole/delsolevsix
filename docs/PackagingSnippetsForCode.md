#Sharing code snippets for Visual Studio Code

There is a number of different ways to provide custom code snippets for Visual Studio Code, all discussed in the [Extensibility Reference page](https://code.visualstudio.com/docs/customization/userdefinedsnippets). 

The DelSole.VSIX library supports generating a .vsix package that contains (at this time) one .json code snippet. There are some requirements to accomplish this, described through this document. Please pay particular attention to the described requirements. 

#Understanding the VSIX format
The VSIX (.vsix) file format allows packaging multiple Visual Studio extensions into one installer. With .vsix packages, you can easily share VS packages, editor extensions, language services, tool windows, project/item templates and, of course, code snippet files.

A .vsix file is nothing but a .zip archive based on the Open XML conventions. A .vsix package must contain:

- a .vsixmanifest XML file, which contains the package metadata and the list of files that must be packaged.
- a package definition file (.pkgdef) that makes Visual Studio understand what kind of contents the package offers and where they must be installed.
- files that will be installed as Visual Studio extensions (typically .dll and .snippet files).

Microsoft is currently working on extending .vsix packages to support Visual Studio Code. So, the DelSole.VSIX library allows generating a .vsix package that can install one .json code snippet. Hopefully, this limitation will be removed in future versions. 

#Software requirements
In order to generate .vsix packages for Visual Studio Code, the DelSole.VSIX library requires you have installed [Node.js](https://nodejs.org) on your machine. Once you have Node.js, you need to install the **vsce** command line tool. Please refer to the [vsce Publishing Tool](https://code.visualstudio.com/docs/tools/vscecli) page for instructions.

#Placing code snippets in the proper directories
There are some requirements for Visual Studio Code snippet files. Create a folder and then a subfolder called **Snippets**. In this subfolder place the .json code snippet file. In the parent folder, the DelSole.VSIX library will create the *manifest*, which is called **package.json**. You will have something like this:

.

├── snippets                    // VS Code integration

│   └── snippets.json           // The JSON file w/ the snippets

└── package.json                // extension's manifest


Of course, you are only responsible for placing the .json file, the library will generate package.json for you. 
 
#Representing code snippets
Before you create a .vsix package, you need to represent each code snippet file you want to package into the .vsix in a .NET fashion. To accomplish this, you use the `DelSole.VSIX.SnippetInfo` class. 
 An instance of this class represents a single .snippet file. Following is an example:
 
 ```csharp
             //Create a SnippetInfo for each code snippet you want to package
            var Snippet1 = new SnippetInfo();
            Snippet1.SnippetFileName = "javascript.json";
            Snippet1.SnippetPath = "C:\\MySnippets\Snippets";
            Snippet1.SnippetLanguage = "javascript";
            Snippet1.SnippetDescription = "description of a snippet file";
 ```
The class exposes five properties:
`SnippetFileName`, which stores a code snippet's file name without its containing folder name
`SnippetPath`, which stores the name of the directory where the code snippet file is
`SnippetLanguage`, which stores the programming language a code snippet is written for. The value of this property must match exactly one of the [supported anguage IDs](https://code.visualstudio.com/docs/languages/overview)
`SnippetDescription`, which stores the code snippet's description.
 
#Representing a VSIX package
Once you have an instance of the `SnippetInfo` class, you must create an instance of the `DelSole.VSIX.VSIXPackage` class, that represents a single .vsix. Once you have create an instance, you populate the `CodeSnippets` property with the `SnippetInfo` instance created before:
 
  ```csharp
            //Create a new package
            var Vsix = new VSIXPackage();

            //Populate the collection of snippets
            Vsix.CodeSnippets.Add(Snippet1);
 ```
 
`CodeSnippets` is an object of type `SnippetInfoCollection` that, behind the scenes, is an `ObservableCollection` of `SnippetInfo`. 

**Remember**: at this time, only one code snippet per package is allowed. Any additional snippets will be ignored. 

Because the .vsix package is generated behind the scenes by invoking the vsce command line tool, you do not set the .vsix package metadata. 
 
#Building a VSIX package
Once you have populated the `CodeSnippets` property, you are ready to go! Building a .vsix is very simple, in fact you simply invoke the `VSIXPackage.Build` method, passing the name of the folder that contains the .json file and the target IDE:
 
   ```csharp
            //Go build it!
            Vsix.Build("C:\\MySnippets\\Snippets", SnippetTools.IDEType.Code);
 ```

The reason why you do not provide a target file name is that the *vsce* command line tool automatically names the .vsix based on the package.json content.
 
#Other resources
See also:

[Reference source for the VSIXPackage class](http://delsolevsixrefsource.azurewebsites.net/#DelSole.VSIX/Vsix_ObjectModel/VSIXPackage.vb)

[Working with code snippets](https://github.com/AlessandroDelSole/delsolevsix/blob/master/docs/WorkingWithCodeSnippets.md)

[Working with old-style .vsi archives](https://github.com/AlessandroDelSole/delsolevsix/blob/master/docs/WorkingWithOldVsiArchives.md)


