using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Examples.Serialization.Yaml.Tests.BasicUsages;

public class SerializationTests
{
    private static ITestOutputHelper? Output => TestContext.Current.TestOutputHelper;

    [Fact]
    public void When_DeserializeAfterSerializing_Then_RestoresToOriginal()
    {
        var original = new Configuration
        {
            DatabaseConnectionString = "Server=.;Database=myDatabase;",
            UploadFolder = "upload/",
            ApprovedFileTypes = new[] { ".png", ".jpeg", ".jpg" }
        };

        const string expected = """
            databaseConnectionString: Server=.;Database=myDatabase;
            uploadFolder: upload/
            approvedFileTypes:
            - .png
            - .jpeg
            - .jpg

            """;

        // Serialize.
        var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

        var yaml = serializer.Serialize(original);

        Output?.WriteLine($"YAML:{Environment.NewLine}{yaml}");
        Assert.Equal(expected, yaml, ignoreLineEndingDifferences: true);

        // Deserialize.
        var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

        var actual = deserializer.Deserialize<Configuration>(yaml);

        Assert.NotNull(actual);
        Assert.NotSame(original, actual);
        Assert.Equal(original.DatabaseConnectionString, actual.DatabaseConnectionString);
        Assert.Equal(original.UploadFolder, actual.UploadFolder);
        Assert.Equal(original.ApprovedFileTypes, actual.ApprovedFileTypes);
    }

    [Fact]
    public void When_Serialize_WithMultilineData_Then_ReturnsAsExpected()
    {
        var original = new Configuration
        {
            UploadFolder = "abc\n" +
                               "def\n" +
                               "\n" +
                               "g\n",
        };

        var expected = """
                uploadFolder: >
                  abc

                  def


                  g

                """;

        var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                .Build();

        var yaml = serializer.Serialize(original);

        Output?.WriteLine($"YAML:{Environment.NewLine}{yaml}");
        Assert.Equal(expected, yaml, ignoreLineEndingDifferences: true);
    }

    private class Configuration
    {
        public string? DatabaseConnectionString { get; set; }
        public string? UploadFolder { get; set; }
        public IEnumerable<string>? ApprovedFileTypes { get; set; }
    }
}
