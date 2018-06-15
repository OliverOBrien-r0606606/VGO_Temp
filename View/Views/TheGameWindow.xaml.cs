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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel;
using Model.Reversi;
using System.Globalization;
using System.Diagnostics;

namespace View.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow : UserControl
    {
        public BoardViewModel boardViewModel;
        public GameWindow()
        {
            InitializeComponent();
        }
    }

    public class OwnerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Owner = (Player)value;
            if (Owner == Player.WHITE)
            {
                Debug.WriteLine("W");
                return new SolidColorBrush(Prefs.PlayerWhite);
            }
            else if (Owner == Player.BLACK)
            {
                Debug.WriteLine("B");
                return new SolidColorBrush(Prefs.PlayerBlack);
            }
            else
            {
                return "Transparent";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CurrentPlayerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var CurrentPlayer = (Player)value;

            if (CurrentPlayer == Player.BLACK) return "Black";
            return "White";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HexStringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value!=null)
                return new SolidColorBrush((Color)value);
            return "Transparent";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
