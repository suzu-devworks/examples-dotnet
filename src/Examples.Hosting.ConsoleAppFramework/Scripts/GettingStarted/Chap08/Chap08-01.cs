#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap08-01.cs
// [Exit Code](https://github.com/Cysharp/ConsoleAppFramework#exit-code)
//
// If the method returns int or Task<int>, ConsoleAppFramework will set the return value to the exit code.

using ConsoleAppFramework;

// return Random ExitCode...
ConsoleApp.Run(args, int () => Random.Shared.Next());

// Linux and macOS: echo $?
// Windows: echo %ERRORLEVEL%

// spell-checker: words ERRORLEVEL
