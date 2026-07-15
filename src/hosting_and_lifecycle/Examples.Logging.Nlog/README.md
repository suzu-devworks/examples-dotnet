# Examples.Logging.Nlog

## Overview and Purpose

This project is an application designed to learn how to implement logging with `NLog` using `Microsoft.Extensions.Hosting`.

## What is NLog?

NLog is a flexible and free logging platform for various .NET platforms, including .NET standard.

A classic logging library dating back to the .NET Framework era, it offers rich functionality and extensibility.

- [NLog](https://nlog-project.org/)

> Since NET Framework 3.5

```bash
dotnet add package NLog.Extensions.Hosting
dotnet add package NLog
```

## Structured logging

The use of structured (object-based) logs appears to be becoming increasingly common.
Many log viewers now support query functionality, improving convenience.

It's possible to control formatting by preceding `@` or `$`:

- `@` will format the object as JSON
- `$` forces `ToString()`

## 'nlog.config' or 'NLog.config'?

The configuration file for NLog is `NLog.config`. Not `nlog.config`.

Windows file systems are case-insensitive, so either name will work,
but case-sensitive file systems such as Linux or macOS may stumble.

However, there is a trap in that when you call `UseNLog()` in `NLog.Extensions.Hosting` or `NLog.Web.AspNetCore`,
the next file is automatically resolved and loaded.

- `nlog.{environmentName}.config`
- `NLog.{environmentName}.config`
- `nlog.config`
- `NLog.config`

You can also load the configuration from `appsettings.json`
by calling `LoadConfigurationFromAppSettings()` on `NLog.Web.AspNetCore`
or `LoadConfigurationFromSection()` on `NLog.Extensions.Logging`.

```cs
var logger = LogManager.Setup()
                       .LoadConfigurationFromAppSettings()
                       .GetCurrentClassLogger();
```

Using `appsettings.json` allows for some overrides using `appsettings.Development.json`,
environment variables, and user secrets, which is not possible with `NLog.config`.

```cs
var logger = LogManager.Setup()
                       .LoadConfigurationFromSection(builder.Configuration)
                       .GetCurrentClassLogger();
```

```json
{
  "NLog": {
    "targets": {
      "file": {
        "type": "File",
        "fileName": "log.txt",
        "layout": "${longdate} ${level:uppercase=true} ${message}"
      }
    },
    "rules": [
      {
        "logger": "*",
        "minLevel": "Info",
        "writeTo": "file"
      }
    ]
  }
}
```

## When do I need to call `LogManager.Shutdown()`

When using generic hosts with `NLog.Extensions.Hosting` or `NLog.Web.AspNetCore`, you do not need to call `LogManager.Shutdown()`.

However, as stated on the official documentation, calling `Shutdown()` in a `finally` block is considered best practice
if you want to catch and log exceptions before the host starts.

This is especially essential when using asynchronous output.

```cs
var builder = Host.CreateApplicationBuilder(args);

var logger = NLog.LogManager.Setup()
    .LoadConfigurationFromSection(builder.Configuration)
    .GetCurrentClassLogger();
try
{
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddHostedService<Worker>();
    
    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
```

## About `RemoveLoggerFactoryFilter`

NLog has a feature called `RemoveLoggerFactoryFilter` that allows you to remove the filter
that prevents logging of messages from the `Microsoft.Extensions.Logging` namespace.

What is this:

```json
  "Logging": {
    "LogLevel": {   // Delete this filter setting.
      "Default": "Information"  
    },
    "NLog": {
      "RemoveLoggerFactoryFilter": true
    }
  },
```

The reason for doing this is that it "because it becomes complicated".

When both filters are enabled, it becomes difficult to determine which filter is working based on the output,
which can lead to problems.

The default value for NLog is `"RemoveLoggerFactoryFilter": true`.
Nlog's recommendation is to "not filter anything in `Microsoft.Extension.Logging`, and handle everything with Nlog".
