using System;
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
using WindowsExplorerish.ViewModels;

namespace WindowsExplorerish.Views
{
    /// <summary>
    /// FileListView.xaml の相互作用ロジック
    /// </summary>
    public partial class FileListView : UserControl
    {
        public FileListView(FileListViewModel viewModel)
        {
            InitializeComponent();

            this.DataContext = viewModel;
        }
    }
}
