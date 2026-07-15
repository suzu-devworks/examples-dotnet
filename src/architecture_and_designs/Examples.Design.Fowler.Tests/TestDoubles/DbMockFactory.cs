using System.Data;
using System.Text.RegularExpressions;
using NSubstitute;

namespace Examples.TestDoubles;

public static partial class DbMockFactory
{
    /// <summary>
    /// Generates a set of database-related mocks based on the specified data list.
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    /// <param name="data">List of data to read into IDataReader.</param>
    /// <returns>Container for the generated set of mocks</returns>
    public static DbMocks CreateDbMocks<T>(IEnumerable<T> data)
        where T : class
    {
        var verifiers = new List<Action>();

        var dataList = data.ToList();
        var mockReader = Substitute.For<IDataReader>();

        // A counter indicating the current row.
        var currentRow = -1;

        // Set up the behavior of IDataReader.Read()
        mockReader.Read().Returns(_ =>
        {
            currentRow++;
            return currentRow < dataList.Count;
        });

        // IDataReader[string]
        mockReader[Arg.Any<string>()].Returns(call => GetValue(dataList, currentRow, call.Arg<string>()));

        mockReader.Close();
        mockReader.Dispose();

        verifiers.Add(() =>
        {
            mockReader.Received(dataList.Count == 1 ? 1 : dataList.Count + 1);
            _ = mockReader.Received()[Arg.Any<string>()];
            mockReader.Received().Close();
            mockReader.Received().Dispose();
        });

        var mockCommand = Substitute.For<IDbCommand>();
        mockCommand.ExecuteReader().Returns(mockReader);
        mockCommand.CommandText = Arg.Any<string>();

        var mockParameter = Substitute.For<IDbDataParameter>();
        mockCommand.CreateParameter().Returns(mockParameter);
        mockParameter.ParameterName = Arg.Any<string>();
        mockParameter.Value = Arg.Any<object>();

        var mockParameters = Substitute.For<IDataParameterCollection>();
        mockParameters.Add(Arg.Any<IDbDataParameter>()).Returns(0);

        mockCommand.Parameters.Returns(mockParameters);
        mockCommand.Dispose();

        verifiers.Add(() =>
        {
            mockCommand.Received().ExecuteReader();
            mockCommand.Received().CommandText = Arg.Any<string>();
            mockCommand.Received().CreateParameter();
            _ = mockParameters.Received().Add(Arg.Any<IDbDataParameter>());
            mockCommand.Received().Dispose();
        });

        var mockConnection = Substitute.For<IDbConnection>();
        mockConnection.CreateCommand().Returns(mockCommand);

        verifiers.Add(() =>
        {
            mockConnection.Received().CreateCommand();
        });

        return new DbMocks
        {
            Connection = mockConnection,
            Verifiers = verifiers,
        };
    }

    private static object? GetValue<T>(List<T> dataList, int rowIndex, string colName)
    {
        if (rowIndex < 0 || rowIndex >= dataList.Count)
        {
            return default;
        }

        var row = dataList[rowIndex]!;

        var properties = row.GetType().GetProperties();
        var property = properties.FirstOrDefault(x => ToSnakeCase(x.Name) == colName);
        if (property is null)
        {
            return default;
        }

        var value = property.GetValue(row);
        return value;
    }

    [GeneratedRegex("[a-z][A-Z]")]
    private static partial Regex CamelCaseRegex();
    private static string ToSnakeCase(string name)
        => CamelCaseRegex()
            .Replace(name, s => $"{s.Groups[0].Value[0]}_{s.Groups[0].Value[1]}")
            .ToLower();

}
