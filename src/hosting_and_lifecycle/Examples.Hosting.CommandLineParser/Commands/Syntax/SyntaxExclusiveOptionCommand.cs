using System.Text;
using CommandLine;

namespace Examples.Hosting.CommandLineParser.Commands.Syntax;

/// <summary>
/// Define commands in set 'web'
/// </summary>
public interface IWebOptions
{
    [Option("document-root", SetName = "web")]
    string DocumentRoot { get; init; }

    [Option("enable-javascript", SetName = "web")]
    bool EnableJavaScript { get; init; }
}

/// <summary>
/// Define commands in set 'ftp'
/// </summary>
public interface IFtpOptions
{
    [Option("ftp-directory", SetName = "ftp")]
    string FtpDirectory { get; init; }

    [Option("anonymous-login", SetName = "ftp")]
    bool AnonymousLogin { get; init; }
}

/// <summary>
/// Defines the options for the "syntax-exclusive" command.
/// </summary>
[Verb("syntax-exclusive", HelpText = "Exclusive Options Syntax Example.")]
public class SyntaxExclusiveOptionCommand : ICommand, IWebOptions, IFtpOptions
{
    // Permitted, Options with the same SetName are allowed:
    // ```
    // --document-root /var/www --enable-javascript
    // --ftp-directory /ftp --anonymous-login
    // ```

    // Not Permitted, Options with different SetName are not allowed together]
    // ```
    // --document-root /var/www --ftp-directory /ftp
    // --enable-javascript --anonymous-login
    // ```

    // Implement commands in set(group) named web
    public string DocumentRoot { get; init; } = default!;
    public bool EnableJavaScript { get; init; } = default!;

    // Implement other commands in set(group) named ftp
    public string FtpDirectory { get; init; } = default!;
    public bool AnonymousLogin { get; init; } = default!;

    // Common option, can be used with any set because it's not included in a set
    [Option('r', HelpText = "URL address, can be used with any set of options.")]
    public string UrlAddress { get; set; } = default!;

    public override string ToString()
    {
        var builder = new StringBuilder("Options Syntax Example:");
        builder.AppendLine();

        builder.AppendLine($"  DocumentRoot: {DocumentRoot}");
        builder.AppendLine($"  EnableJavaScript: {EnableJavaScript}");
        builder.AppendLine($"  FtpDirectory: {FtpDirectory}");
        builder.AppendLine($"  AnonymousLogin: {AnonymousLogin}");
        builder.AppendLine($"  UrlAddress: {UrlAddress}");

        return builder.ToString();
    }
}
