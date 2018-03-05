using GrovePi;
using GrovePi.Sensors;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WindowIotTempMonitor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private TemperaturePolling _tempPolling;
        private int _counter; 

        public UpdatingText Time;
        public UpdatingText Tempurature;
        public UpdatingText Humidity;
        public UpdatingText Counter; 

        public MainPage()
        {
            InitializeComponent();
            
            _tempPolling = new TemperaturePolling();
            _counter = 0; 

            Time = UpdatingText.CreateNewTimeAndTemp();
            Tempurature = UpdatingText.CreateNewTimeAndTemp();
            Humidity = UpdatingText.CreateNewTimeAndTemp();
            Counter = UpdatingText.CreateNewTimeAndTemp();

            UpdateText();
            StartTimer();
        }

        private void StartTimer()
        {
            TimeSpan period = TimeSpan.FromSeconds(60);

            ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {
                UpdateText();
            }, period);
        }
        
        private void UpdateText()
        {
            var entry = _tempPolling.GetTimeAndTemp();
            _counter++;
            
            Time.Text = FormatTime(entry.Time);
            Tempurature.Text = FormatTemperature(entry.Temperature);
            Humidity.Text = FormatHumidity(entry.Humidity);
            Counter.Text = _counter.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateText();
        }

        private string FormatDate(DateTime dateTime)
        {
            return dateTime.ToString("D");
        }

        private string FormatTime(DateTime dateTime)
        {
            return dateTime.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
        }

        private string FormatTemperature(decimal temp)
        {
            return string.Format("Temp: {0}F", temp);
        }

        private string FormatHumidity(decimal humidity)
        {
            return string.Format("Humidity: {0}%", humidity);
        }
    }

    public class UpdatingText : INotifyPropertyChanged
    {
        private string _text;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private UpdatingText()
        {
        }

        public static UpdatingText CreateNewTimeAndTemp()
        {
            return new UpdatingText();
        }

        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                if (value != _text)
                {
                    _text = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
