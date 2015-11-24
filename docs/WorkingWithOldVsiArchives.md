# Working with old-style .vsi archives

In Visual Studio 2005, 2008, and 2010, you could package and share code snippets and other items via .vsi installers, based on the Visual Studio Community Content Installer engine. Since Visual Studio 2012 on, the .vsi format has been deprecated and developers have been encouraged to definitely use the .vsix file format, which enables installing Visual Studio extensions. Among the possible extensions you can install, code snippet files (.snippet) are definitely supported. But because you might have existing code snippets packaged into .vsi archives from the past, the DelSole.VSIX library makes it easier for you to migrate those archives to the .vsix format. 

## The `DelSole.VSIX.VsiTools` namespace and the `DelSole.VSIX.VsiTools.VsiService` class
The `DelSole.VSIX.VsiTools` namespace exposes the `VsiService` class, which offers methods that help interacting with old-fashioned .vsi archives. More specifically, the class exposes the static `Vsi2Vsix` and `ExtractVsi` methods as detailed below.

## Converting a .vsi package to .vsix: The `Vsi2Vsix` method ##

The `DelSole.VSIX.VsiTools.VsiService.Vsi2Vsix` static method allows converting an old .vsi package to a .vsix package. 
The only condition is that **the original .vsi package must contain only code snippets**, otherwise conversion will fail.

The first two arguments for the method are the input .vsi file and the target .vsix package name. Other arguments **must** be taken from an instance of the `VSIXPackage` class. Following is an example:

  ```csharp
    //Convert an old .vsi file into a .vsix package
    VsiService.Vsi2Vsix("C:\\temp\\VBWPFSnippets.vsi", "C:\\temp\\VBWPFSnippets.vsix",
            "VB WPF Snippets", "Alessandro Del Sole", "VB Snippets for WPF", "A common set of WPF Snippets for VB",
            null, null, "https://github.com/alessandrodelsole/delsolevsix");
 ```

## Extracting a .vsi package's content: the `ExtractVsi` method ##

The `DelSole.VSIX.VsiTools.VsiService.ExtractVsi` static allows extracting the content of a .vsi package. You can decide wether to extract only the code snippets or the whole archive content. Following is an example:

  ```csharp
    //Extract the full content of a .vsi archive
    VsiService.ExtractVsi("C:\\temp\\VBWPFSnippets.vsi", "C:\\temp", false);
 ```
 
 If the third argument is *false*, the whole content is extracted. If *true*, only code snippets are extracted.
