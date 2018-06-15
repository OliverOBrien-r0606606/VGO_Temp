using Cells;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ViewModel;

namespace View
{
    /// <summary>
    /// Interaction logic for Application.xaml
    /// </summary>
    public partial class ApplicationStart : Window
    {
        private ApplicationViewModel app = new ApplicationViewModel();
        public ApplicationStart()
        {
            InitializeComponent();
            DataContext = app;
        }
    }
    
}
