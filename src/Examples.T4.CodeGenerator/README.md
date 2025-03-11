# Examples.T4.CodeGenerator

- [Code Generation and T4 Text Templates - Microsoft Learn](https://learn.microsoft.com/ja-jp/visualstudio/modeling/code-generation-and-t4-text-templates?view=vs-2022)

## Overview

T4 text template is a mixture of text blocks and control logic that can generate a text file. 

Use T4 templates to generate text files such as web pages, resource files, and program source code in any language.

There are two kinds of T4 text templates: run time and design time

### Run time T4 text templates

Run time templates are also known as 'preprocessed' templates. You run the templates in your application to produce text strings, as part of its output.

### Design time T4 text templates

Design time templates define part of the source code and other resources of your application. Typically, you use several templates that read the data in a single input file or database, and generate some of your .cs, .vb, or other source files. 


## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.T4.CodeGenerator
dotnet new console -o src/Examples.T4.CodeGenerator
dotnet sln add src/Examples.T4.CodeGenerator
cd src/Examples.T4.CodeGenerator
dotnet add package Mono.TextTemplating
cd ../../

# Tools config
dotnet new tool-manifest
dotnet tool install dotnet-t4

# Update outdated package
dotnet list package --outdated
```

