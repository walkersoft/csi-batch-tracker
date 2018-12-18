using CSI.BatchTracker.ViewModels;
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

namespace CSI.BatchTracker.Views
{
    /// <summary>
    /// Interaction logic for OperatorManagementWindowView.xaml
    /// </summary>
    public partial class OperatorManagementWindowView : Window
    {
        public OperatorManagementWindowView(BatchOperatorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
