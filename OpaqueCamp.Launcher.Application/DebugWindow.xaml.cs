using OpaqueCamp.Launcher.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
                var javaFinder = new JavaFinder();
                
                debugJavaLabel.Content = $"Java path: {javaFinder.GetJavawExePath()}";
            }
            catch (JavaNotFoundException er)
            {
                debugJavaLabel.Content = $"Java path: {er.Message}";
            }
        }

        private void debugWMI_Click(object sender, RoutedEventArgs e)
        {
            var jvmMP = new JVMMemoryProvider();
            jvmMP.AutoMaxMemory();
            

            debugWMILabel.Content = $"Max memory: {jvmMP.MaxMemoryAllocation}";
        }
    }
}
