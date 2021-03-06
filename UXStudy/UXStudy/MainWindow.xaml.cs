﻿using System;
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

namespace UXStudy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainApplication app;
        private Window instruction_window;

        public MainWindow()
        {
            InitializeComponent();
            app = new MainApplication();
            instruction_window = new InstructionWindow();
    
            instruction_window.DataContext = app.Instructions;
            DataContext = app;
        }
    }
}
