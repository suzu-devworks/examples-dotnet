# Examples.Logging.Serilog

## Overview and Purpose

This project is an application designed to learn how to implement logging using `Serilog` using `Microsoft.Extensions.Hosting`.

## What is Serilog?

Serilog is a library for outputting diagnostic logs, but unlike other logging libraries,
it is built with powerful structured event data in mind.

Most settings can also be done in code,
but you can also load settings from a configuration file such as `appsettings.json` using `Serilog.Settings.Configuration`.

Since the packages are separated by function, they seem to be used in combination.
Therefore, the number of packages to be installed will be larger than others.

- [Serilog](https://serilog.net/)

> Since NET Framework 4.5

```bash
# minimum
dotnet add package Serilog
dotnet add package Serilog.Extensions.Hosting
dotnet add package Serilog.Sinks.Console

# Required to load settings from appsettings.json
dotnet add package Serilog.Settings.Configuration

# Convenient sinks and enrichers
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.Sinks.Async
dotnet add package Serilog.Enrichers.Environment
dotnet add package Serilog.Enrichers.Thread
```

## The way you use words is special

### Sinks

It is "Sink" in English. This word refers to the "sink" in the kitchen,
but in Serilog, it means "the log output destination".

Serilog uses additional sinks to write log events to storage in various formats.
A sink is a component that sends log events to various output destinations such as files, consoles, and databases.

- [Provided Sinks](https://github.com/serilog/serilog/wiki/Provided-Sinks)

### Enrichers

Enricher is a function that automatically adds useful information (context) to logs.
You can add information such as machine name and thread ID to properties.

- [Available enricher packages](https://github.com/serilog/serilog/wiki/Enrichment#available-enricher-packages)

### Filters

Enrichers and the properties they attach are generally more useful with sinks that use structured storage,
where the property values can be viewed and filtered.

## Structured logging

The use of structured (object-based) logs appears to be becoming increasingly common.
Many log viewers now support query functionality, improving convenience.

If Serilog does not recognize the type and no operator is specified, the object will be rendered using `ToString()`.

- In Serilog, `@` represents the destructuring assignment operator.
  In other words, it outputs in JSON object format.
- `$` represents the stringification operator, which outputs the result of `ToString()`.

By making full use of `Destructure`, it seems possible to represent the passed object in various forms.

## Two ways to write startup code

### Basic

```cs
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

```

- By initializing Log.Logger before the startup code, all logs can be recorded
- Since the configuration file hasn't been loaded yet, the logging settings must be configured in the code

### Startup code using `Serilog.Extensions.Hosting`

```cs
builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());
```

- Settings can be written in `appsettings.json`. `Serilog.Settings.Configuration` is required
- Exceptions that occur before `AddSerilog()` are not logged. (`Microsoft.Extensions.Logging` behaves the same way.)

### And this is probably the correct answer

**Write both.**

It seems you only need to change the last part of the beginning of the code to `CreateBootstrapLogger()`.

```cs
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger(); // <-- Change this line!
```

- Use the basic log until you call `AddSerilog()`
- After calling `AddSerilog()`, use the settings in `appsettings.json` to overwrite the settings in code.

Basically, I want to do everything in `appsettings.json`.

## Sinks a little more

### `Serilog.Sinks.Async`

This is an asynchronous wrapper for other Serilog sinks. Using this sink allows you to delegate processing
to a background thread, reducing the I/O overhead of logging calls.

## Enrichment more

### `Serilog.Enrichers.Sensitive`

This is a Serilog enricher that can mask sensitive data from a LogEvent message template and its properties.

```cs
var logger = new LoggerConfiguration()
    .Enrich.WithSensitiveDataMasking()
    .WriteTo.Console()
    .CreateLogger();
```

Hmm, I think I'll use it.

## References

- [Configuration Basics - Github Wiki](https://github.com/serilog/serilog/wiki/Configuration-Basics)
- [Serilog.Settings.Configuration](<https://github.com/serilog/serilog-settings-configuration>)
