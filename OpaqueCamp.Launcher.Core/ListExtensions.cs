namespace OpaqueCamp.Launcher.Core;

public static class ArgumentListExtensions
{
    public static void AddMany<T>(this List<T> list, params T[] args)
    {
        list.AddRange(args);
    }
}
