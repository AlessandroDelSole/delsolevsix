# Working with code snippets
The DelSole.VSIX library makes it easy to generate reusable code snippets for both Visual Studio 2015 and Visual Studio Code. These two IDEs use different code snippet formats: Visual Studio relies on XML code snippets, whereas Code relies on JSON code snippets. This document describes how to create code snippets for both environments. 

# Code snippets for Visual Studio 2015 (.snippet)

Microsoft Visual Studio (2005 and higher) supports IntelliSense code snippets. These are small pieces of reusable code that you can insert in your code while typing.
To insert a snippet, right-click in the code editor and then select **Insert Snippet**. A popup allows you to pickup from a built-in set of code snippets, which can be categorized and organized within folders.

You can write and share your own code snippets, and plug them into the Visual Studio's editor. To accomplish this, you first create a .snippet file based on the [Code Snippet Schema Reference](https://msdn.microsoft.com/en-us/library/ms171418.aspx).
Once you have created one or more .snippet files, you can package your code snippets into a .vsix archive, which is the file format for Visual Studio extensions and that provides an automated way of installing, among the others, reusable code snippets.

The DelSole.VSIX library provides APIs that make it easy to both create .snippet files and create .vsix installers. 

*There are many free code snippet editors that you can use, but in case you need to build your own or you need a simple way to manipulate snippets, this library is for you. Do not forget to checkout the [Code Snippet Studio](https://github.com/AlessandroDelSole/CodeSnippetStudio) project, which uses the DelSole.VSIX library*    

# Code snippets for Visual Studio Code

Visual Studio Code is a free, advanced code editor with syntax highlighting, IntelliSense, live Roslyn analysis, Git integration and much more. To insert a snippet, in Code click View, Command Palette, then type Snippets and select the desired programming language from the list or press CTRL+Space. 

You can write and share your own code snippets, and plug them into the Code's editor. To accomplish this, you first create a .json file based on the [proper JSON format](https://code.visualstudio.com/docs/customization/userdefinedsnippets). Once you have created one or more .json files, you can package your code snippets into a .vsix archive, which provides an automated way of installing, among the others, reusable code snippets. Actually, this is not the only way to share code snippets for Visual Studio Code because the way Code manages code snippets is very different from Visual Studio. More about these differences is discussed in the [Working with .vsix packages](https://github.com/AlessandroDelSole/delsolevsix/blob/master/docs/WorkingWithVsixPackages.md) document. 

The DelSole.VSIX library provides APIs that make it easy to create .json files. At this time, you can package only one snippet into a .vsix archive. 

## The `DelSole.VSIX.SnippetTools` namespace

A code snippet is essentially made of the following elements:

- snippet properties (author, title, keywords, shortcut, help url)
- Imports directives (VB only, a list of namespaces required for the snippet to work)
- Assembly references (VB only, a list of assembly references required for the snippet to work)
- The code (supported languages are Visual Basic, C#, SQL, XML, C++, Html, JavaScript) 
- Declarations. A declaration lets IntelliSense to highlight the specified word/identifier in the code editor and to suggest the proper replacement with a tooltip.

The DelSole.VSIX library expose the `DelSole.VSIX.SnippetTools` namespaces, which offers the following types:

- `CodeSnippet` class, which represents a code snippet with its properties and allows saving a code snippet based on the proper XML schema by invoking the SaveSnippet method
- `CodeSnippetKinds` enumeration, which represents the kind of code snippet (method body, method declaration, type declaration, file, any)
- `Import` class and `Imports` collection, which represent one or more Imports directives. 
- `Reference` class and `References` collection, which represent one or more assembly references. 
- `Declaration` class and `Declarations` collection, which represent one or more declarations.

## The `CodeSnippet` class

The `DelSole.VSIX.SnippetTools.CodeSnippet` class represents a .snippet file compliant with the [Code Snippet Schema Reference](https://msdn.microsoft.com/en-us/library/ms171418.aspx) and, with the exception of namespaces and references, a .json snippet file. The following code demonstrates how to create a code snippet:

  ```csharp
            CodeSnippet myCodeSnippet = new CodeSnippet();
            myCodeSnippet.Author = "Alessandro Del Sole";
            myCodeSnippet.Title = "Print message";
            myCodeSnippet.Language = "CSharp";
            myCodeSnippet.Kind = CodeSnippetKinds.MethodBody;
            myCodeSnippet.Description = "Print a message to the Console window";
            myCodeSnippet.Code = @"Console.Writeline(""Hello world!"");";

            Declaration decl = new Declaration();
            decl.ID = "Hello";
            decl.Default = "Hello world!";
            decl.Editable = true;
            decl.ToolTip = "Replace with your string...";

            myCodeSnippet.Declarations.Add(decl);
```
### Saving a code snippet

The `CodeSnippet` class exposes the static `SaveSnippet` method, which is very simple to use. It requires the target file name, the `CodeSnippet` instance, and a value from the `IDEType` enumeration as arguments. The latter is used to determine if the target file format must be a .snippet (Visual Studio 2015) or .json (Visual Studio Code) file.

The following code demonstrates how to save a Visual Studio code snippet:

```csharp
CodeSnippet.SaveSnippet("C:\\Temp\\PrintMessage.snippet", myCodeSnippet, IDEType.VisualStudio);
```

The following code demonstrates how to save a code snippet for Visual Studio Code:

```csharp
CodeSnippet.SaveSnippet("C:\\Temp\\PrintMessage.json", myCodeSnippet, IDEType.Code);
```

### Error validation

The `CodeSnippet` class implements the `IDataErrorInfo` interface which helps for data validation. For instance, you can check the `HasErrors` property before saving the snippet as follows:

  ```csharp
            if (myCodeSnippet.HasErrors==true)
            {
                // Validation errors exist
                throw new InvalidOperationException();
            }
            else
            {
                CodeSnippet.SaveSnippet("C:\\Temp\\PrintMessage.snippet", myCodeSnippet, IDEType.VisualStudio);
            }
```

## Visual Basic only: namespaces and assembly references
 
The code snippet schema reference states that Visual Basic code snippets can optionally include a list of namespaces and assembly references that are required for the snippet to work. This is **not** supported in Visual Studio Code. Here's an example:
 
   ```vb
            Dim ns As New Import
            ns.ImportDirective = "System.Diagnostics"
            myCodeSnippet.Namespaces.Add(ns)

            Dim asm As New Reference
            asm.Assembly = "DelSole.Vsix.dll"
            asm.Url = "https://github.com/AlessandroDelSole/delsolevsix" 'optional
            myCodeSnippet.References.Add(asm)
``` 

### Loading a code snippet

The `CodeSnippet` class exposes the static `LoadSnippet` method, which loads the content of a .snippet file and returns an instance of the `CodeSnippet` class. It only requires the file name as an argument:

```csharp
CodeSnippet mySnippet = CodeSnippet.LoadSnippet("C:\\Temp\\PrintMessage.snippet");
```

At this time, there is no support for loading an existing .json snippet.

# Reference source
Browse the code for the CodeSnippet class [online](http://delsolevsixrefsource.azurewebsites.net/#DelSole.VSIX/Snippet_ObjectModel/CodeSnippet.vb).
