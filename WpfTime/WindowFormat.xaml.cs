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

namespace WpfTime
{
    /// <summary>
    /// Логика взаимодействия для WindowFormat.xaml
    /// </summary>
    public partial class WindowFormat : Window
    {
        public string Format
        {
            get { return TextBoxFormat.Text; }
            set { TextBoxFormat.Text = value; }
        }
        public WindowFormat()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
