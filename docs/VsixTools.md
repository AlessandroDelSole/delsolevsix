## Extracting a .vsix package ##

You can extract the whole content of a .vsix package invoking the `DelSole.VSIXPackage.ExtractVsix` static method:

  ```csharp
    DelSole.VSIXPackage.ExtractVsix("inputFile.vsix", "C:\targetFolder");
 ```

Notice that this operation will fully extract a .vsix package, including manifest and package definition.

