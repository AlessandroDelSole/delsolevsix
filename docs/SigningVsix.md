# Signing a .vsix package #

The DelSole.VSIX package allows signing an existing .vsix package with a digital signature based on the X.509 certificate file format (.pfx). This must be password protected.

In order to sign a .vsix package, you invoke the DelSole.VSIXPackage.SignVsix static method:

    DelSole.VSIXPackage.SignVsix("inputPackage.vsix", "certFile.pfx", "password");

Where:
inputPackage.vsix is the file name for the .vsix package to be signed
certFile.pfx is the digital signature
password is an object of type SecureString that contains the password for the .pfx file
