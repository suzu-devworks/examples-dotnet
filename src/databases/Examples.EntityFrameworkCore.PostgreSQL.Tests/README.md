# Examples.EntityFrameworkCore.PostgreSQL.Tests

## Create the database

The following steps use migrations to create a database.

Installing the tools:

```shell
dotnet tool restore
```

Set ConnectionStrings:

```shell
dotnet user-secrets set ConnectionStrings:ContosoUniversity "Host=postgres;Database=postgres;Username=postgres;Password=$(cat /run/secrets/db_password)"
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

## Development

### `Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone', only UTC is supported.`

As stated, in order to register data as a timestamp with time zone, the `DateTime`'s `DateTimeKind` must be `UTC`.

If you think about it carefully, there is no DateTime type in .NET that stores TimeZone. It seems that the mapping is insufficient.

The optimal solution for PostgreSQL seems to be to use a library called NodaTime, but if you only need to convert to UTC,
there are probably other ways to solve the problem. One such solution is `ValueConverter`.

```cs
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>()
            .Property(x => x.EnrollmentDate)
            .HasConversion<UniversalDateTimeConvertor>();
    }
```

### If the date type is still messing up

I haven't started researching it yet.

- [Date/Time Mapping with NodaTime | Npgsql Documentation](https://www.npgsql.org/efcore/mapping/nodatime.html?tabs=ef9-with-connection-string)

### I usually stick to snake_case when naming stuff in PostgreSQLs

Change the naming convention for EF Core's auto-mapping.

```shell
dotnet add package EFCore.NamingConventions
```

Enable a naming convention in your model's OnConfiguring method:

```cs
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        .UseNpgsql(...)
        .UseSnakeCaseNamingConvention();
```

If you change this, the migration table(`__EFMigrationsHistory`) will also change, so you will need to recreate the migration.
