# Examples.EntityFrameworkCore.Tests

## Index

### Examples.EntityFrameworkCore Tests

- [Metadata/](./EntityFrameworkCore.Tests/Metadata/)

### ContosoUniversity Tests

- [Tutorials/](./EntityFrameworkCore.SQLite.Tests/ContosoUniversity/Tutorials/)
- [Repositories/](./EntityFrameworkCore.SQLite.Tests/ContosoUniversity/Repositories/)

## Create the database

The following steps use migrations to create a database.

Installing the tools:

```shell
dotnet tool restore
```

Add a Migration:

```shell
dotnet ef migrations add InitialCreate
```

Apply Migrations:

```shell
dotnet ef database update
```

## Microsoft.EntityFrameworkCore.InMemory

I tried using `Microsoft.EntityFrameworkCore.InMemory` partway through, but the lack of transactions was a fatal problem,
making it impossible to write test code.

## Microsoft.EntityFrameworkCore.SQLite

### Types of connection strings

A standard connection string, persisted in the specified file.

`Data Source=Application.db;Cache=Shared`

SQLite uses the in-memory format below.

`Data Source=:memory:`

However, this format doesn't work properly with EntityFramework tests because Open and Close are performed automatically.

Therefore, we'll use a shareable in-memory format.

`Data Source=Sharable:Mode=Memory;Cache=Shared`

- [Connection strings - Microsoft.Data.Sqlite | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/standard/data/sqlite/connection-strings)
