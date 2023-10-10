# Information about **FrizzLib**

## What is FrizzLib?
**FrizzLib** is a small set of library routines written in C#, for the personal use of the author (public name *FrizzSoft*). I am happy that anyone freely use the libraries or code in any way that they like.

---

The most useful namespace is probably FrizzLib.CLI.Pickers.  This has a class called `Pickers` providing the following methods
- `PickFile()`
- `PickFolder()`

These methods provide command-line functionality similar to that found in GUI equivalents such as Microsoft.Win32.OpenFileDialog and System.Windows.Forms.FolderBrowserDialog classes.  They enable you to browse through a directory structure in order to select a file or a folder respectively.

If you are just writing a quick console application and need to browse for a file or a folder, these classes may be helpful.
