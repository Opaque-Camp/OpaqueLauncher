namespace OpaqueCamp.Launcher.Core;

public sealed class JavaNotFoundException : UserFriendlyException
{
    public JavaNotFoundException() : base(
        "Не удалось найти установленную Java на вашем компьютере. Установите Java, либо, если вы уверены, что она уже установлена, попробуйте переустановить её.",
        "javaw.exe was not found because JAVA_HOME is not set or set incorrectly and javaw.exe is missing in PATH.")
    {
    }
}