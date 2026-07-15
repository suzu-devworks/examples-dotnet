# Examples.FluentMigrator.PostgreSQL

## Overview

This project contains an example FluentMigrator setup targeting PostgreSQL. It demonstrates how to create and run database
migrations using FluentMigrator with the Npgsql provider in a .NET class library.

### Build

To build the solution from the repository root:

```bash
dotnet build
```

Note: Running migrations requires a configured connection string and a migration runner host. Refer to the project code
for example runner usage and configuration.

## Commands

The project includes a small console runner (ConsoleAppFramework) that exposes migration commands. You can pass a
connection string via the global option `-c|--connection` or configure `ConnectionStrings:Default` in `appsettings.json`,
environment variables, or user secrets.

Available commands:

- `migrate` : Run migrations to the latest version. Options: `-r` (with RLS check, default true) and `-v <version>` to
 run a specific migration version.
- `rollback <version>` : Roll back to the specified migration version.
- `info` : Show the latest applied migration version.
- `check-rls` : Run Row-Level Security (RLS) checks.

Examples (run from repository root):

```bash
dotnet run --project src/Examples.FluentMigrator.PostgreSQL -- migrate -c "Host=localhost;Database=mydb;Username=user;Password=pass"
dotnet run --project src/Examples.FluentMigrator.PostgreSQL -- migrate -v 20251229002 -c "..."
dotnet run --project src/Examples.FluentMigrator.PostgreSQL -- rollback 20251229001 -c "..."
dotnet run --project src/Examples.FluentMigrator.PostgreSQL -- info -c "..."
dotnet run --project src/Examples.FluentMigrator.PostgreSQL -- check-rls -c "..."
```

See the `Program.cs` and `Commands` folder for details on options and behavior.
