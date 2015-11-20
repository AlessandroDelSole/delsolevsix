# VSIX tools
The `DelSole.VSIX.VSIXPackage` class exposes other methods to interact with a .vsix package, such as SignVsix (to add a digital signature) and ExtractVsix (to extract the content of a package)

## Signing a .vsix package ##

VSIX packages can be digitally signed with a X.509 certificate, through a password protected .pfx file. The DelSole.VSIX package allows signing an existing .vsix package by invoking the `DelSole.VSIXPackage.SignVsix` static method:

```csharp
    DelSole.VSIXPackage.SignVsix("inputPackage.vsix", "certFile.pfx", "password");
```

Where:
**inputPackage.vsix** is the file name for the .vsix package to be signed
**certFile.pfx** is the digital signature
**password** is an object of type SecureString that contains the password for the .pfx file

## Extracting a .vsix package ##

You can extract the whole content of a .vsix package or only the code snippet it contains invoking the `DelSole.VSIXPackage.ExtractVsix` static method:

  ```csharp
    DelSole.VSIXPackage.ExtractVsix("inputFile.vsix", "C:\targetFolder", true);
 ```

If the third argument is *true*, the operation will only extract code snippets (.snippet) contained in the .vsix package. If *false*, this operation will fully extract a .vsix package, including manifest and package definition.

