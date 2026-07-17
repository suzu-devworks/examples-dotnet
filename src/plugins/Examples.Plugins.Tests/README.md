# Examples.Plugins.Tests

## Overview and Purpose

This project uses the xUnit test runner to explore dynamic assembly loading via `AssemblyLoadContext`
and plugin architecture patterns built on it.

## Test Target

- [System.Runtime.Loader.AssemblyLoadContext](https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.loader.assemblyloadcontext)

## Test Index

- [Create a .NET Core application with plugins](./Tutorials/PluginCommandsTests.cs)

## Development

### Copy plugins to output directory

In production, plugins go into the application's execution directory.

In this test project, plugin projects are referenced only to preserve build order.
`ReferenceOutputAssembly="false"` keeps plugin assemblies out of compile-time references.

Plugin files are copied in a custom target that runs after build.
This avoids early wildcard evaluation issues and works reliably in CI.

```xml
  <ItemGroup>
    <ProjectReference Include="..\fixtures\Examples.Plugins.Hello\Examples.Plugins.Hello.csproj"
      ReferenceOutputAssembly="false" />
    <ProjectReference Include="..\fixtures\Examples.Plugins.Json\Examples.Plugins.Json.csproj"
      ReferenceOutputAssembly="false" />
    <ProjectReference Include="..\fixtures\Examples.Plugins.Libuv\Examples.Plugins.Libuv.csproj"
      ReferenceOutputAssembly="false" />
  </ItemGroup>

  <Target Name="CopyFixturePluginOutputs" AfterTargets="Build">
    <ItemGroup>
      <_FixturePlugin Include="Examples.Plugins.Hello">
        <SourcePath>..\fixtures\Examples.Plugins.Hello\bin\$(Configuration)\$(TargetFramework)</SourcePath>
      </_FixturePlugin>
      <_FixturePlugin Include="Examples.Plugins.Json">
        <SourcePath>..\fixtures\Examples.Plugins.Json\bin\$(Configuration)\$(TargetFramework)</SourcePath>
      </_FixturePlugin>
      <_FixturePlugin Include="Examples.Plugins.Libuv">
        <SourcePath>..\fixtures\Examples.Plugins.Libuv\bin\$(Configuration)\$(TargetFramework)</SourcePath>
      </_FixturePlugin>

      <_FixturePluginFile Include="%(_FixturePlugin.SourcePath)\**\*.*">
        <PluginName>%(_FixturePlugin.Identity)</PluginName>
      </_FixturePluginFile>
    </ItemGroup>

    <Copy
      SourceFiles="@(_FixturePluginFile)"
      DestinationFiles="@(_FixturePluginFile->'$(OutDir)plugins\%(PluginName)\%(RecursiveDir)%(Filename)%(Extension)')"
      SkipUnchangedFiles="true" />
  </Target>
```

## References

- [About System.Runtime.Loader.AssemblyLoadContext](https://learn.microsoft.com/ja-jp/dotnet/core/dependency-loading/understanding-assemblyloadcontext)
- [Create a .NET Core application with plugins - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/tutorials/creating-app-with-plugin-support#load-plugins)
