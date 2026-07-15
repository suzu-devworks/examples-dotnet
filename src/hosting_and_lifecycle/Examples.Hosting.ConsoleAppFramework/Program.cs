using ConsoleAppFramework;
using Examples.Hosting.ConsoleAppFramework.Services.Hello;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddScoped<IHelloService, HelloService>();

var app = builder.ToConsoleAppBuilder();

await app.RunAsync(args);
