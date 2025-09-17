namespace Examples.T4.CodeGenerator.DesignTimeTemplates.Walkthrough;

public interface ICatalog
{
    IEnumerable<Artist> Artist => throw new NotImplementedException();

    void Load(string fileName) => throw new NotImplementedException();

}

public partial class Catalog : ICatalog
{
}


public interface IArtist
{
    IEnumerable<Song> Song => throw new NotImplementedException();

    string Name => throw new NotImplementedException();

}

public partial class Artist : IArtist
{
}


public interface ISong
{
    string Text => throw new NotImplementedException();
}

public partial class Song : ISong
{
}
