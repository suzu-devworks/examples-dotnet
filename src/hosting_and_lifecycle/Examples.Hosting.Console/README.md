# Examples.Hosting.Console

## Overview and Purpose

This project is an application designed to learn how to implement logging using `Microsoft.Extensions.Hosting`.

## What is Microsoft.Extensions.Hosting?

Provides a convenient way to manage the lifetime of applications and their dependencies.
It allows you to create long-running services, such as background tasks or web applications,
and provides features like dependency injection, configuration, and logging.

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

This is a sample console application, so let's start by looking at the help.
However, there is no dedicated help command.

```bash
dotnet run
```

```console
No valid startup option provided. Use --host, --host-app, --host-app-hosted, or --without.

Example: dotnet run -- --host-app
```

### ``--host`` option

This option uses the `HostBuilderStartup` class, which is a typical implementation of a console application using `Microsoft.Extensions.Hosting`.

```bash
dotnet run -- --host
```

### ``--host-app`` option

This option uses the `HostAppStartup` class, which is a typical implementation of a console application
using `Microsoft.Extensions.Hosting` and `IHostedService`.

```bash
dotnet run -- --host-app
```

### ``--host-app-hosted`` option

This option uses the `HostAppHostedStartup` class, which is a typical implementation of a console application
using `Microsoft.Extensions.Hosting` and `IHostedService`, and also demonstrates the use of hosted services.

```bash
dotnet run -- --host-app-hosted
```

### ``--without`` option

This option uses the `WithoutHostStartup` class, which is a typical implementation of a console application
without using `Microsoft.Extensions.Hosting`.

```bash
dotnet run -- --without
```
