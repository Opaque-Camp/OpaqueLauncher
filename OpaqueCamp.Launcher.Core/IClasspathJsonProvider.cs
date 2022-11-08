namespace OpaqueCamp.Launcher.Core;

public interface IClasspathJsonProvider
{
    /// <summary>
    /// Returns the contents of <c>game\versions\Opaque Vanilla 1.XX.X\Opaque Vanilla 1.XX.X.json</c>.
    /// </summary>
    /// <exception cref="IOException">Thrown when <c>game\versions\Opaque Vanilla 1.XX.X\Opaque Vanilla 1.XX.X.json</c> does not exist or is inaccessible.</exception>
    string GetClasspathJson();
}
