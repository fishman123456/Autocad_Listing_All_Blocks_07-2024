using MyApplication;

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

namespace Autocad_Listing_All_Blocks_07_2024
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : Window
    {
        DumpAttributes dumpAttributes = new DumpAttributes();
        public UserControl1()
        {
            InitializeComponent();
        }

        private void textSeach_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void buttonSeach_Click(object sender, RoutedEventArgs e)
        {
           WinCloseTwo.massSeach = textSeach.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            dumpAttributes.ListAttributes();
        }

        private void buttonClouseWin_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            WinCloseTwo.countWin = 0;
        }
    }
}
