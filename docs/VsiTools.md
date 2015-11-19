# Vsi Tools
The `DelSole.VSIX.VsiTools.VsiService` class exposes methods that help interacting with old-fashioned .vsi archives.

## Converting a .vsi package to .vsix ##

Visual Studio 2005, 2008, and 2010 support the .vsi "Visual Studio Content Installer" package format. The `DelSole.VSIX.VsiTools.VsiService.Vsi2Vsix` static method allows converting an old .vsi package to a .vsix package. The only condition is that **the original .vsi package must contain only code snippets**, otherwise conversion will fail.

The first two arguments for the method are the input .vsi file and the target .vsix package name. Other arguments must be taken from an instance of the VSIXPackage class. Following is an example:

  ```csharp
    //Convert an old .vsi file into a .vsix package
    VSIXPackage.Vsi2Vsix("C:\\temp\\VBWPFSnippets.vsi", "C:\\temp\\VBWPFSnippets.vsix",
            "VB WPF Snippets", "Alessandro Del Sole", "VB Snippets for WPF", "A common set of WPF Snippets for VB",
            null, null, "https://github.com/alessandrodelsole/delsolevsix");
 ```

## Extracting a .vsi package's content ##

The `DelSole.VSIX.VsiTools.VsiService.ExtractVsi` static allows extracting the content of a .vsi package. You can decide wether to extract only the code snippets or the whole archive content. Following is an example:

  ```csharp
    //Extract the full content of a .vsi archive
    VsiService.ExtractVsi("C:\\temp\\VBWPFSnippets.vsi", "C:\\temp", false);
 ```
 
 If the third argument is *false*, the whole content is extracted. If *true*, only code snippets are extracted.
