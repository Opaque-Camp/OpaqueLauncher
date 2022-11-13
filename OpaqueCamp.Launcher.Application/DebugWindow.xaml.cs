using System.Windows;
using OpaqueCamp.Launcher.Core;
using OpaqueCamp.Launcher.Core.Memory;
using OpaqueCamp.Launcher.Infrastructure.Memory;

namespace OpaqueCamp.Launcher.Application
{
    /// <summary>
    /// Логика взаимодействия для DebugWindow.xaml
    /// </summary>
    public partial class DebugWindow : Window
    {
        public DebugWindow()
        {
            InitializeComponent();
        }

        private void debugJavaButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var javaFinder = new JavaFinder(new EnvironmentService(), new FileSystem());

                debugJavaLabel.Content = $"Java path: {javaFinder.GetJavawExePath()}";
            }
            catch (JavaNotFoundException er)
            {
                debugJavaLabel.Content = $"Java path: {er.Message}";
            }
        }

        private void debugWMI_Click(object sender, RoutedEventArgs e)
        {
            var jvmMP = new JvmMemorySettings(new JvmMemorySettingsStorage(), new WindowsSystemMemoryDetector());
            
            debugWMILabel.Content = $"Max memory: {jvmMP.MaximumMemoryAllocation.RecommendedMegabytes}";
        }

        private void debugCrashHandler_Click(object sender, RoutedEventArgs e)
        {
            var crashLogs = new MinecraftCrashLogs("This is debug", "Sample text");
            var mcw = new MinecraftCrashWindow(crashLogs);
            mcw.Show();
        }
    }
}