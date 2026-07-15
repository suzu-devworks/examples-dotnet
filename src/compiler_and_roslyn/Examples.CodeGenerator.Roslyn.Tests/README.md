# Examples.CodeGenerator.Roslyn.Tests

## Overview and Purpose

This project contains tests for the code generator implemented in the `Examples.CodeGenerator.Roslyn` project.

## Test Target

- [Examples.CodeGenerator.Roslyn](../Examples.CodeGenerator.Roslyn/)

## Development Notes

### Check the generated file

Just put the following in your `*.csproj` file.

```xml
<EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
```

The file will be output to `/obj/Debug/{FrameworkVersion}/generated/`

### Generator Debugging

When using a typical generator, project references are done as follows:

```xml
<ProjectReference Include="..\MyGenerator\MyGenerator.csproj"
                  OutputItemType="Analyzer"
                  ReferenceOutputAssembly="false" />
```

It may be possible to debug with this in Visual Studio (unconfirmed), but it is not possible with VS Code.
To debug in VS Code, you need to browse your project and run generation using `GeneratorDriver` as usual.

```xml
<ProjectReference Include="..\MyGenerator\MyGenerator.csproj" />
```

```cs
var generator = new MyGenerator();
var driver = CSharpGeneratorDriver.Create(generator);
driver.RunGenerators(compilation, CancellationToken);
```

You can now set breakpoints and debug within the generator.

However, that is fine, but if you do that, you will need the following three projects.

- Generator project
- Generator test project (debug)
- Projects that use Generator (generate code)

This configuration is a bit more difficult to manage due to the increased number of projects.

If you want to create one project for code generation and debugging, write two ProjectReferences.

```xml
<ItemGroup>
  <ProjectReference Include="..\Examples.CodeGenerator.Roslyn\Examples.CodeGenerator.Roslyn.csproj" />
</ItemGroup>

<ItemGroup>
  <ProjectReference Include="..\Examples.CodeGenerator.Roslyn\Examples.CodeGenerator.Roslyn.csproj"
                    OutputItemType="Analyzer"
                    ReferenceOutputAssembly="false" />
</ItemGroup>
```

This method (double referencing) is fine if there are no issues,
but it raises some questions because it references the same project.

In fact, I received a review from Copilot.

Actually, the following code will also work.

```xml
<ItemGroup>
  <ProjectReference Include="..\Examples.CodeGenerator.Roslyn\Examples.CodeGenerator.Roslyn.csproj"
                    OutputItemType="Analyzer"
                    ReferenceOutputAssembly="true" />
</ItemGroup>
```

Ultimately, it seems that even for projects registered with `OutputItemType="Analyzer"`,
setting `ReferenceOutputAssembly="true"` allows test code to read the type and create an instance.

**Because the setup became hard to follow, I moved it into a separate sample project.**
