## Extracting a .vsix package ##

You can extract the whole content of a .vsix package invoking the DelSole.VSIXPackage.ExtractVsix static method:

  ```csharp
    DelSole.VSIXPackage.ExtractVsix("inputFile.vsix", "C:\targetFolder");
 ```

## Converting a .vsi package to .vsix ##

Visual Studio 2005, 2008, and 2010 support the .vsi "Visual Studio Installer" package format. The DelSole.VSIXPackage.Vsi2Vsix static method allows converting an old .vsi package to a .vsix package. The only condition is that **the original .vsi package must contain only code snippets**, otherwise conversion will fail.

The first two arguments for the method are the input .vsi file and the target .vsix package name. Other arguments must be taken from an instance of the VSIXPackage class. Following is an example:

  ```csharp
    //Convert an old .vsi file into a .vsix package
    VSIXPackage.Vsi2Vsix("C:\\temp\\VBWPFSnippets.vsi", "C:\\temp\\VBWPFSnippets.vsix",
            "VB WPF Snippets", "Alessandro Del Sole", "VB Snippets for WPF", "A common set of WPF Snippets for VB",
            null, null, "https://github.com/alessandrodelsole/delsolevsix");
 ```
