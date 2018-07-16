using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WpfTime.Properties;

namespace WpfTime
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer _timer;



        public string TimeProperty
        {
            get { return (string)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register(nameof(TimeProperty), typeof(string), typeof(MainWindow), new PropertyMetadata("def"));

        //public string Format { get; set; } = Properties.Settings.Default.Format;
        public MainWindow()
        {
            InitializeComponent();

            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.IsEnabled = true;
            _timer.Tick += (s, e) =>
            {
                TimeProperty = DateTime.Now.ToString(Settings.Default.Format);
            };
            //    delegate
            //{
            //    TimeProperty = DateTime.Now.ToString(Format);
            //};

            this.DataContext = this;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void MenuItemColor_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog cd = new System.Windows.Forms.ColorDialog();
            var result = cd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var converter = Resources.Values.OfType<ColorConverter>().FirstOrDefault();

                _label.Foreground = (SolidColorBrush)converter.Convert(cd.Color, typeof(SolidColorBrush), null, null);
            }
        }

        private void MenuItemFont_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FontDialog fontDialog = new System.Windows.Forms.FontDialog();
            if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Debug.WriteLine(fd.Font);

                _label.FontFamily = new FontFamily(fontDialog.Font.Name);
                _label.FontSize = fontDialog.Font.Size * 96.0 / 72.0;
                _label.FontWeight = fontDialog.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                _label.FontStyle = fontDialog.Font.Italic ? FontStyles.Italic : FontStyles.Normal;

                TextDecorationCollection tdc = new TextDecorationCollection();
                if (fontDialog.Font.Underline) tdc.Add(TextDecorations.Underline);
                if (fontDialog.Font.Strikeout) tdc.Add(TextDecorations.Strikethrough);
                //_label.TextDecorations = tdc;
            }
        }

        private void MenuItemBorder_Click(object sender, RoutedEventArgs e)
        {
            this.BorderThickness =
            MessageBox.Show("Показывать рамку?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes ?
            new Thickness(2) : new Thickness();
        }

        private void MenuItemFormat_Click(object sender, RoutedEventArgs e)
        {
            WindowFormat wf = new WindowFormat();
            wf.Format = Settings.Default.Format;
            if (wf.ShowDialog() == true)
            {
                Settings.Default.Format = wf.Format;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Properties.Settings.Default.Save();
        }

        private void MenuItemClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
