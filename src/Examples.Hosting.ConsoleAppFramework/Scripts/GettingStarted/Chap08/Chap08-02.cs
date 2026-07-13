#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap08-02.cs
// [Exit Code](https://github.com/Cysharp/ConsoleAppFramework#exit-code)
//
// If the method returns int or Task<int>, ConsoleAppFramework will set the return value to the exit code.

using ConsoleAppFramework;

// return StatusCode
await ConsoleApp.RunAsync(args, async Task<int> (string url, CancellationToken cancellationToken) =>
{
    using var client = new HttpClient();
    var response = await client.GetAsync(url, cancellationToken);
    return (int)response.StatusCode;
});

// > --url https://www.google.com/
// > echo $?
// 200
