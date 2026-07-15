# Examples.CodeGenerator.Roslyn.Demo

## Overview and Purpose

This project includes a sample that uses the code generator from `Examples.CodeGenerator.Roslyn`.

## Build

**Prerequisites**:

- **.NET SDK**: A version of `TargetFramework` that is supported is required.
See [LatestFramework](../Directory.Build.props) property.

There are no particular points to note during the build process.

Just run the commands as you normally would:

```bash
dotnet build
```

## Usage

Since this is a command line sample, let's take a look at the help first:

```bash
dotnet run -- --help
```

Here are the commands used in this sample:

- `hello`: Runs the `HelloWorldGenerator` sample
- `enum`: Runs the `EnumDescriptionGenerator` sample
- `notify`: Runs the `NotifyPropertyChangedGenerator` sample
