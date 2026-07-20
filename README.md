# examples-dotnet

![Dynamic XML Badge](https://img.shields.io/badge/dynamic/xml?url=https%3A%2F%2Fraw.githubusercontent.com%2Fsuzu-devworks%2Fexamples-dotnet%2Frefs%2Fheads%2Fmain%2Fsrc%2FDirectory.Build.props&query=%2F%2FLatestFramework&logo=dotnet&label=Framework&color=%23512bd4)
[![CI (build and test)](https://github.com/suzu-devworks/examples-dotnet/actions/workflows/dotnet-ci.yml/badge.svg)](https://github.com/suzu-devworks/examples-dotnet/actions/workflows/dotnet-ci.yml)
[![CodeQL](https://github.com/suzu-devworks/examples-dotnet/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/suzu-devworks/examples-dotnet/actions/workflows/github-code-scanning/codeql)

## What is this repository?

This repository explains various types of application development using .NET through small-scale samples that focus on
specific features.

It is part of the `examples` collection — a set of repositories that document personal exploration of
technologies, APIs, and programming techniques.
Each example is meant as a reference that may help others investigating similar topics.

## What topics are covered?

This includes a wide variety of projects.

### Fundamentals and Core Concepts

This includes items that do not fall into specific categories,
as well as custom implementations based on the concept of extending core functionality.

### Architecture and Design Patterns

This includes examples of design patterns and domain-driven design concepts using .NET and C#.

### Compiler and Roslyn

This includes examples of a record of small-scale, focused experiments exploring language features,
compiler extensibility, runtime technologies, and related tools.

### Concurrency and Parallel Programming

This includes examples of concurrent and parallel programming using .NET, including multithreading,
async/await, and task-based programming.

### Cryptography and Security

This includes practical examples of encryption and security features in .NET using
`System.Security.Cryptography` and the BouncyCastle library.

### Data Access and Databases

This includes examples of data access and database interaction using .NET, including Entity Framework Core,
ADO.NET, and Dapper.

### Graphics

This includes examples of graphics programming using .NET.

### Hosting and Lifecycle Management

This includes examples using .NET that demonstrate host configuration, dependency injection,
configuration management, related runtime patterns.

### Logging

This includes examples of logging and monitoring using .NET based on `Microsoft.Extensions.Logging`.

### Plugins and Extensions

This includes examples of developing plugins and extensions that use .NET to read assemblies directly.

### Serialization and Deserialization

This includes examples of serialization and deserialization using .NET, including JSON, XML, and binary formats.

### Web Development

This includes examples of web development using .NET, including ASP.NET Core, Blazor, and related technologies.

## What should I prepare before development?

This repository provides multiple development container environments. Please select the libraries,
services, and databases you wish to use.

- [C# (.NET)](./.devcontainer/): It is used in standard .NET Core development
- [C# (.NET with Graphics)](./.devcontainer/graphics/): Adding graphics-related native libraries (planned);
  currently, only Japanese fonts are included
- [C# (.NET with SqlServer)](./.devcontainer/mssql/): Use this when using a SQL Server database
- [C# (.NET with PostgreSQL)](./.devcontainer/pgsql/): Use this when using a PostgreSQL database
- [C# (.NET with Web)](./.devcontainer/web/): The NGINX reverse proxy is started. Configurations
  for SSL certificates and ASP.NET are also already set up.

As the repository houses a large number of projects, opening it directly may make working difficult;
therefore, we have prepared VSCode workspaces organized by category. Please select the appropriate workspace for your tasks.
