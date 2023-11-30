# ILSpy

## Project site

- https://github.com/icsharpcode/ILSpy 
- https://www.nuget.org/packages/ilspycmd/


## Install ilspycmd in Remote Container(.NET 5.0)

### Clone Repository

```shell
# On container image is `mcr.microsoft.com/dotnet/sdk:5.0`

git clone https://github.com/icsharpcode/ILSpy.git

cd ILSpy
git submodule update --init --recursive
```

### Modify .csproj file

* TargetFrameworks: Add .net5.0
* Version: Do not duplicate the real thing. (Add -debug ...)

```shell
cd ICSharpCode.Decompiler.Console
code ICSharpCode.Decompiler.Console.csproj
```

```xml:csproj
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <!-- TargetFrameworks>netcoreapp2.1;netcoreapp3.1</TargetFrameworks -->
    <TargetFrameworks>netcoreapp2.1;netcoreapp3.1;net5.0</TargetFrameworks>
    <ServerGarbageCollection>true</ServerGarbageCollection>
    ...
    <ToolCommandName>ilspycmd</ToolCommandName>
    <!--Version>7.1.0.6488-preview1</Version -->
    <Version>7.1.0.6488-debug</Version>
    <AssemblyVersion>7.1.0.0</AssemblyVersion>
    ...
```

### Build = No Error.

```shell
dotnet build
```

### Add local Nuget Source

```shell
dotnet nuget add source %{PATH_TO_REPOS}/ILSpy/ICSharpCode.Decompiler.Console/bin/Debug/ --name ilspy-local

dotnet nuget list source

```

### Add local tool

```shell
# specify version
dotnet tool install ilspycmd --global --version=7.1.0.6488-debug

dotnet tool list --global

```

When you execute --global for the first time, an additional message to the PATH will be displayed, so take appropriate action.

```console
Tools directory '/home/vscode/.dotnet/tools' is not currently on the PATH environment variable.
If you are using bash, you can add it to your profile by running the following command:

cat << \EOF >> ~/.bash_profile
# Add .NET Core SDK tools
export PATH="$PATH:/home/vscode/.dotnet/tools"
EOF

You can add it to the current session by running the following command:

export PATH="$PATH:/home/vscode/.dotnet/tools"
```

### Run

```shell
ilspycmd --help

ilspycmd <assemblyfile>.dll -il -t Examples.Features.CS8.RangeAndIndicesTests | tee xxx.txt

```
