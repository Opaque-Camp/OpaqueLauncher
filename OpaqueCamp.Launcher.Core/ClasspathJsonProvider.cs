namespace OpaqueCamp.Launcher.Core;

public sealed class ClasspathJsonProvider : IClasspathJsonProvider
{
    private readonly PathProvider _pathProvider;

    public ClasspathJsonProvider(PathProvider pathProvider)
    {
        _pathProvider = pathProvider;
    }

    public string GetClasspathJson()
    {
        return File.ReadAllText(_pathProvider.ClasspathJsonPath);
    }
}
