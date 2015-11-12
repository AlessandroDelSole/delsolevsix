## Extracting a .vsix package ##

You can extract the whole content of a .vsix package invoking the DelSole.VSIXPackage.ExtractVsix static method:

    DelSole.VSIXPackage.ExtractVsix("inputFile.vsix", "C:\targetFolder")

## Converting a .vsi package to .vsix ##

Visual Studio 2005, 2008, and 2010 support the .vsi "Visual Studio Installer" package format. The DelSole.VSIXPackage.Vsi2Vsix static method allows converting an old .vsi package to a .vsix package. The only condition is that **the original .vsi package must contain only code snippets**, otherwise conversion will fail.

The first two arguments for the method are the input .vsi file and the target .vsix package name. Other arguments must be taken from an instance of the VSIXPackage class.