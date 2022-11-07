namespace OpaqueLauncher;

public sealed record Version(int Major, int Minor, int Patch)
{
    /// <summary>
    /// String with only major and minor parts, without the patch part.
    /// </summary>
    /// <example>
    /// <c>"1.19"</c>
    /// </example>
    public string MajorAndMinorString => $"{Major}.{Minor}";

    /// <summary>
    /// Returns the full version string representation, including major, minor and patch parts.
    /// </summary>
    /// <example>
    /// <c>"1.19.2"</c>
    /// </example>
    public override string? ToString() => $"{MajorAndMinorString}.{Patch}";
}
