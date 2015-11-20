# Generating code snippets (.snippet)

The `DelSole.VSIX.SnippetTools.SnippetService.SaveSnippet` method creates a .snippet file compliant with the [Code Snippet Schema Reference](https://msdn.microsoft.com/en-us/library/ms171418.aspx).
Following is an example:

  ```vb
        SnippetService.SaveSnippet(fileName, selectedSnippet.Kind, currentLang, selectedSnippet.Title,
                               selectedSnippet.Description, selectedSnippet.HelpUrl,
                               selectedSnippet.Author, selectedSnippet.Shortcut, editControl1.Text,
                               Imports, References, Declarations, keywords)
 ```
 The second argument is of type `SnippetTools.CodeSnippetTypes`, an enumeration that represents the kind of code snippet. The Imports and References arguments are of type `SnippetTools.Imports` and `SnippetTools.References`, two collections that
 respectively represent required Imports directives and assembly references. These are for VB code snippets only. 
 The Declaration argument is of type `SnippetTools.Declarations`, a collection that represents the list of replacements/suggestions in the code editor.
 
 **Note** At this time, there is a bug with this method. So it will probably fail. There's already an open issue on GitHub.
