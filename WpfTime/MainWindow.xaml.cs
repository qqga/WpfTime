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
using System.Windows.Threading;
using WpfTime.Properties;

namespace WpfTime
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            SetSettings();
            _viewModel = new ViewModel() { };
            this.DataContext = _viewModel;

            _viewModel.Bind(vm => vm.Format, Settings.Default, s => s.Format);
            _viewModel.Bind(vm => vm.Period, Settings.Default, s => s.TimerPeriod);
        }

        void SetSettings()
        {
            SetLabelFont(Settings.Default.Font);
            SetBorderThicknessFromSettings();
        }

        void SetLabelFont(System.Drawing.Font font)
        {
            _label.FontFamily = new FontFamily(font.Name);
            _label.FontSize = font.Size * 96.0 / 72.0;
            _label.FontWeight = font.Style == System.Drawing.FontStyle.Bold ? FontWeights.Bold : FontWeights.Regular;
            _label.FontStyle = font.Italic ? FontStyles.Italic : FontStyles.Normal;

            if(font.Strikeout) _textBlock.TextDecorations = TextDecorations.OverLine;
            if(font.Underline) _textBlock.TextDecorations = TextDecorations.Underline;
            if(!font.Underline && !font.Strikeout) _textBlock.TextDecorations = null;
        }
        private void SetBorderThicknessFromSettings()
        {
            this.BorderThickness = new Thickness(Settings.Default.BorderThickness);
            this.BorderBrush = _label.Foreground;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Settings.Default.Save();
        }

        #region Drag
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
        #endregion

        #region MenuCommands

        private void MenuItemFormat_Click(object sender, RoutedEventArgs e)
        {
            WindowFormat wf = new WindowFormat();
            wf.Format = wf.Format;
            if(wf.ShowDialog() == true)
            {
                _viewModel.Format = wf.Format;
            }
        }

        private void MenuItemBorder_Click(object sender, RoutedEventArgs e)
        {
            WindowBorder windowBorder = new WindowBorder();            
            windowBorder.Thickness = Settings.Default.BorderThickness;

            if(windowBorder.ShowDialog() == true)
            {
                Settings.Default.BorderThickness = windowBorder.Thickness;                
                SetBorderThicknessFromSettings();
            }
        }

        private void MenuItemFont_Click(object sender, RoutedEventArgs e)
        {
            var fontDialog = new System.Windows.Forms.FontDialog();
            fontDialog.Font = Settings.Default.Font;
            if(fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var font = fontDialog.Font;
                SetLabelFont(font);
                Settings.Default.Font = font;
            }
        }

        private void MenuItemColor_Click(object sender, RoutedEventArgs e)
        {
            var colorDialog = new System.Windows.Forms.ColorDialog();
            if(colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _label.Foreground = ColorToBrush(colorDialog.Color);
            }
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e) => Settings.Default.Save();
        private void MenuItemClose_Click(object sender, RoutedEventArgs e) => this.Close();
        #endregion

        Brush ColorToBrush(System.Drawing.Color color)
        {
            var converter = Resources.Values.OfType<ColorConverter>().FirstOrDefault();
            SolidColorBrush solidColorBrush = (SolidColorBrush)converter.Convert(color, typeof(SolidColorBrush), null, null);
            return solidColorBrush;
        }
    }
}
