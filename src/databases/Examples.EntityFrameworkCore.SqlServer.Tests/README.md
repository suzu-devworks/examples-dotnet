# Examples.EntityFrameworkCore.SqlServer.Tests

## Create the database

The following steps use migrations to create a database.

Installing the tools:

```shell
dotnet tool restore
```

Set ConnectionStrings:

```shell
dotnet user-secrets set ConnectionStrings:ContosoUniversity "Data Source=sqlserver;Initial Catalog=ContosoUniversity;User ID=sa;Password=$(cat /run/secrets/db_password);Persist Security Info=False;TrustServerCertificate=yes"
```

Or Use environment variable `ConnectionStrings__ContosoUniversity`.

Add a Migration:

```shell
dotnet ef migrations add InitialCreate
```

Apply Migrations:

```shell
dotnet ef database update
```

## Check the database information

You can easily check this by using `sqlcmd` in the container.

```shell
./opt/mssql-tools18/bin/sqlcmd -U sa -C
```

> [!TIP]
> This is what happens, so "-C" is required.
>
> ```console
> Sqlcmd: Error: Microsoft ODBC Driver 18 for SQL Server : SSL Provider: [error:0A000086:SSL routines::certificate verify failed:self-signed certificate].
> Sqlcmd: Error: Microsoft ODBC Driver 18 for SQL Server : Client unable to establish connection. For solutions related to encryption errors, see https://go.microsoft.com/fwlink/?linkid=2226722.
> ```

Check the database list:

```SQL
SELECT name, database_id, create_date
FROM sys.databases;
GO
```

It's probably in the `master` database, so move it.

```SQL
USE Examples;
GO
```

Get a list of tables:

```SQL
SELECT schema_name(schema_id), name FROM sys.tables;
GO
```
