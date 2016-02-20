#Roslyn Code Analyis

The DelSole.VSIX library has integrated Roslyn code analysis for different scenarios.

##Detecting issues in the snippet's code

The `CodeSnippet` class can detect issues in the snippet's code using the Roslyn APIs (VB and C# snippets only). To run code analysis, you invoke the `AnalyzeCode` method. The result is assigned to the `Diagnostics` property, which is an `ObservableCollection<Microsoft.CodeAnalysis.Diagnostic>` and that can be easily data-bound. You use it like this:

  ```csharp
            myCodeSnippet.AnalyzeCode();
            foreach (var diag in myCodeSnippet.Diagnostics)
            {
              Console.Writeline(diag.Id);
            }
```

##Live analysis when using the library

When coding using DelSole.VSIX, Visual Studio 2015's editor will automatically detect some code issues. Coming soon.
