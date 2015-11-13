 # Signing a .vsix package

VSIX packages can be digitally signed with a X.509 certificate, through a password protected .pfx file. The DelSole.VSIX package allows signing an existing .vsix package by invoking the DelSole.VSIXPackage.SignVsix static method:

```csharp
    DelSole.VSIXPackage.SignVsix("inputPackage.vsix", "certFile.pfx", "password");
```

Where:
**inputPackage.vsix** is the file name for the .vsix package to be signed
**certFile.pfx** is the digital signature
**password** is an object of type SecureString that contains the password for the .pfx file
