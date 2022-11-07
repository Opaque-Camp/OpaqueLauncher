﻿using System;
namespace OpaqueCamp.Launcher.Application;

using OpaqueLauncher.Core;
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

    private void debugJavaButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var javaFinder = new JavaFinder();
            debugJavaButton.Content = javaFinder.GetJavaVersion();
        }
        catch (JavaNotFoundException er)
        {
            debugJavaButton.Content = er.Message;
            
        }

        
    }
}