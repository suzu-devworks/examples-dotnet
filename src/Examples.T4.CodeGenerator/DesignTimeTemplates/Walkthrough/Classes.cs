namespace Examples.T4.CodeGenerator.DesignTimeTemplates.Walkthrough;

public interface ICatalog
{
    public IEnumerable<Artist> Artist => throw new NotImplementedException();

    public void Load(string fileName) => throw new NotImplementedException();

}

public partial class Catalog : ICatalog
{
}


public interface IArtist
{
    public IEnumerable<Song> Song => throw new NotImplementedException();

    public string Name => throw new NotImplementedException();

}

public partial class Artist : IArtist
{
}


public interface ISong
{
    public string Text => throw new NotImplementedException();
}

public partial class Song : ISong
{
}
