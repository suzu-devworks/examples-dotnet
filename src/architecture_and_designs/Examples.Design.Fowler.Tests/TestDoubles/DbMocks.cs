using System.Data;

namespace Examples.TestDoubles;

public class DbMocks
{
    public required IDbConnection Connection { get; init; }

    public required IEnumerable<Action> Verifiers { get; init; }

    public void VerifyAll()
    {
        foreach (var verifier in Verifiers)
        {
            verifier();
        }
    }

}
