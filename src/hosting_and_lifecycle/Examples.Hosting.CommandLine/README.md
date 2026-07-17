# Examples.Hosting.CommandLine

## Overview and Purpose

This project is an application designed to learn how to implement logging with `System.CommandLine` using `Microsoft.Extensions.Hosting`.

## What is System.CommandLine?

Provides functionality commonly needed by command-line apps, such as parsing command-line input and displaying help text.

This is a genuine Microsoft product.

It was offered as a preview version for a long time,　but it appears that significant changes have been made to arrive
at the current version. And finally, it was officially released as version 2.0.0 in November 2025.

Please note that much of the information available online is based on the preview version.

- [System.CommandLine overview](https://learn.microsoft.com/ja-jp/dotnet/standard/commandline/)

> Since .NET Standard 2.0

```bash
dotnet add package System.CommandLine
```

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

### ``quotes`` command

This command basically follows the contents of [Tutorial](https://learn.microsoft.com/ja-jp/dotnet/standard/commandline/get-started-tutorial),
　but defines the arguments and implements the processing independently.

```bash
dotnet run -- quotes --help
```

### ``syntax`` command

This command is for learning the syntax of command line arguments. Basically, I am referring to [Syntax overview](https://learn.microsoft.com/ja-jp/dotnet/standard/commandline/syntax),
 but I have implemented it to check the actual operation.

```bash
dotnet run -- syntax --help
```

## How to implement Commands and subcommands

There are two main ways to implement commands and subcommands.

- derived from "command"
- Use command builder

Both have advantages and disadvantages. A common problem is handling "SetAction" and parsed arguments.

Derived command class approach:

- Encapsulation: Command name, options, and execution logic are in one class, improving readability.
- Constructor pollution: Constructors require service classes or locators, which can clutter parameters.
- Testing: Relying on service locators makes testing difficult and leads to a lot of mock iterations.
- Safe argument access: Options objects allow direct retrieval of type-safe values.
- Flexibility: Inheritance allows you to share logic and properties, but classes can become complex.
  Allowing service locators modifies the services used in commands, which can reduce reusability.
  Inheriting from `RootCommand` defines subcommands as properties of the root, making the hierarchy clearer,
  but also concentrating the definitions and creating a fixed structure.

Builder-based command approach:

- Encapsulation: The "command name", "options", and "execution logic" are combined into one method,
  which improves readability but can make the builder bloated.
- Constructor pollution: The constructor accepts only the arguments needed to define the command.
  No class of service or locator is required.
- Testing: Easier because the definition and logic is inside the builder. You can define mocks there,
  reducing redundant test code.
- Safe argument access: Options objects allow direct retrieval of type-safe values.
- Flexibility: The builder separates definition and execution, so you can change the execution without
  changing the definition, but this separation can compromise readability.

Result = my hybrid approach:

- Separating structure (command definitions) and behavior (handlers) improves readability and extensibility.
- Use a derived command class to encapsulate the "command name" and "options".
  No "execution logic" is written in the command class.
- Service locators are not used.
- The "execution logic" is written in a separate file as a handler class.
- In the setup code, the command structure is defined in `RootCommand`.
  Then, the properties of `RootCommand` are traversed to map each handler to the appropriate command's `SetAction`.

We aim to keep the design simple, minimize the use of reflection, and create code that is familiar and understandable.

## Cascading verbosity options

It's common in Linux systems to change the verbosity of the output by specifying `-V` multiple times.
 `System.CommandLine` allows options to be specified multiple times, and in the case of bool type options,
 the value can be omitted, so I thought it would be possible to easily change the verbosity by specifying `-V`
 multiple times, so I implemented it.

I think that by defining `Option<bool[]>`, you can specify `-V` multiple times. In reality,
 flag processing that does not require a bool value that specifies an array will not work.
 `-V　-V` and `-VV` both result in an error.

```cs
public readonly Option<bool[]> CascadingVerboseOption = new("-V")
{
}
```

Next, I defined `Option<bool>` and set `Arity` to `ZeroOrMore` so that `-V` can be specified multiple times.
Now `-VVV` no longer causes an error.

```cs
public readonly Option<bool> CascadingVerboseOption = new("-V")
{
    Arity = ArgumentArity.ZeroOrMore,
    CustomParser = _ => true
};
```

Be careful when acquiring it. Even if you specify `-V` multiple times, `GetValue()` will only return `true`.
I need to count the number of specified Tokens.

```cs
var value = parseResult.GetValue(CascadingVerboseOption);
var count = parseResult.Tokens.Count(x => x.Value == "-V");
System.Console.WriteLine($"Cascading verbose count: {value}, {count}");
```

However, there is a problem with this method, and if you specify `false` while specifying `-V` multiple times,
such as `-VV -V false`, it will not make sense.
So I thought it would be a good idea to throw an error if I put in an argument, so I tried playing it with Validator.

```cs
CascadingVerboseOption.Validators.Add(result =>
{
    if (result.Tokens.Count != 0)
    {
        result.AddError("The -V option does not accept any arguments. Specify -V multiple times to increase verbosity.");
    }
});
```

Despite all the trouble I've been having, I feel like there isn't much benefit to specifying `-V` multiple times.

Ultimately, I don't know if this is due to the design philosophy of `System.CommandLine`,
or if the `--verbose` option for the `dotnet` command exists because of this implementation.

Rather than having to specify `-VVV` multiple times, it seems natural and requires less effort to specify `--verbose detailed`.
