using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelSole.VSIX;

namespace SampleApp_CS
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create a SnippetInfo for each code snippet you want to package
            var Snippet1 = new SnippetInfo();
            Snippet1.SnippetFileName = "UnprotectDocument.snippet";
            Snippet1.SnippetPath = "C:\\MySnippets";
            Snippet1.SnippetLanguage = SnippetInfo.GetSnippetLanguage(Snippet1.SnippetPathName);
            Snippet1.SnippetDescription = SnippetInfo.GetSnippetDescription(Snippet1.SnippetFileName);

            //Create a new package
            var Vsix = new VSIXPackage();

            //Populate the collection of snippets
            Vsix.CodeSnippets.Add(Snippet1);

            //Set package metadata information
            Vsix.Tags = "Word";
            Vsix.PackageAuthor = "Alessandro Del Sole";
            Vsix.PackageDescription = "A test VSIX package with snippets";
            Vsix.License = "C:\\temp\\MIT_License.txt";
            Vsix.MoreInfoURL = "https://github.com/alessandrodelsole/delsolevsix";
            //Assign other properties here...

            //Go build it!
            Vsix.Build("C:\temp\Sample.vsix");
        }
    }
}
