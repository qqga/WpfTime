using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfTime
{
    public class ViewModel : BaseVm
    {
        System.Windows.Threading.DispatcherTimer _timer;
        string _TimeProperty;
        public string TimeProperty { get => _TimeProperty; set { _TimeProperty = value; OnPropChanged(); } }
        public string _Format;
        public string Format { get => _Format; set { _Format = value; OnPropChanged(); } }
        public int _Period;
        public int Period { get => _Period; set { _Period = value; OnPropChanged(); } }

        public ViewModel()
        {
            InitTimer();
            Start();
        }

        void InitTimer()
        {
            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(Period);
            _timer.Tick += (s, e) => TimeProperty = DateTime.Now.ToString(Format);
        }

        void Start()
        {
            _timer.IsEnabled = true;
        }
    }
}
