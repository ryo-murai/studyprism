﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication.ViewModels;

namespace WpfApplication.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class Shell : Window
    {
        public Shell(ShellViewModel viewModel)
        {
            this.DataContext = viewModel;

            InitializeComponent();
        }

        private void RibbonApplicationMenu_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
    }
}
