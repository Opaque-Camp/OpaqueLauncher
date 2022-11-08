using System;
namespace OpaqueCamp.Launcher.Application;

using OpaqueCamp.Launcher.Core;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    
    // Double-click
    private void OpenDebugWIndow(object sender, MouseButtonEventArgs e)
    {
        var debugWindow = new DebugWindow();
        debugWindow.Show();
    }
}
