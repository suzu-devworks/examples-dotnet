using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Examples.Serialization.Yaml.Tests;

public class BasicSerializationTests
{
    private readonly ITestOutputHelper _output;

    public BasicSerializationTests(ITestOutputHelper output)
    {
        // ```
        // dotnet test --logger "console;verbosity=detailed"
        // ```
        _output = output;
    }


    [Fact]
    public void WhenCallingsSerializeAndDeserialize_ReturnsAsExpected()
    {
        const string EXPECTED = """
            databaseConnectionString: Server=.;Database=myDatabase;
            uploadFolder: upload/
            approvedFileTypes:
            - .png
            - .jpeg
            - .jpg

            """;

        var config = new Configuration
        {
            DatabaseConnectionString = "Server=.;Database=myDatabase;",
            UploadFolder = "upload/",
            ApprovedFileTypes = new[] { ".png", ".jpeg", ".jpg" }
        };

        // Serialize.
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var yaml = serializer.Serialize(config);

        _output.WriteLine($"YAML{Environment.NewLine}{yaml}");
        Assert.Equal(EXPECTED, yaml, ignoreLineEndingDifferences: true);

        // Deserialize.
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var actual = deserializer.Deserialize<Configuration>(yaml);

        object.ReferenceEquals(actual, config).IsFalse();
        actual.DatabaseConnectionString.Is(config.DatabaseConnectionString);
        actual.UploadFolder.Is(config.UploadFolder);
        actual.ApprovedFileTypes.Is(config.ApprovedFileTypes);

        return;
    }


    [Theory]
    [MemberData(nameof(DataOfMultiline))]
    public void WhenCallingsSerialize_WithMultiline_ReturnsAsExpected(
        Configuration? config,
        string expected)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .Build();

        var yaml = serializer.Serialize(config);

        _output.WriteLine($"YAML{Environment.NewLine}{yaml}");
        Assert.Equal(expected, yaml, ignoreLineEndingDifferences: true);

        return;
    }

    public static IEnumerable<object[]> DataOfMultiline()
    {
        {
            var config = new Configuration
            {
                UploadFolder = "abc\n" +
                                "def\n" +
                                "\n" +
                                "g\n",
            };

            var yaml = """
                uploadFolder: >
                  abc

                  def


                  g

                """;

            yield return new object[] { config, yaml };
        }

        {
            var config = new Configuration
            {
                UploadFolder = "abc\n" +
                                "def\n" +
                                "\n" +
                                "g\n",
            };

            var yaml = """
                uploadFolder: >
                  abc

                  def


                  g

                """;

            yield return new object[] { config, yaml };
        }
    }

    public class Configuration
    {
        public string? DatabaseConnectionString { get; set; }
        public string? UploadFolder { get; set; }
        public IEnumerable<string>? ApprovedFileTypes { get; set; }
    }

}
