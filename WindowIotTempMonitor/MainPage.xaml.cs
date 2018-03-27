using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WindowIotTempMonitor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private TemperatureSensor _sensor;
        private Timer _timeTimer;
        private Timer _tempTimer;

        public UpdatingText Time;
        public UpdatingText Tempurature;
        public UpdatingText Humidity;

        public MainPage()
        {
            InitializeComponent();
            _sensor = new TemperatureSensor();

            Time = UpdatingText.CreateNewTimeAndTemp();
            Tempurature = UpdatingText.CreateNewTimeAndTemp();
            Humidity = UpdatingText.CreateNewTimeAndTemp();

            UpdateTempData();
            _timeTimer = new Timer(new TimerCallback((obj) => RefreshTime()), null, 0, 1000);
            _tempTimer = new Timer(new TimerCallback((obj) => RefreshTemp()), null, 0, 30500);
        }

        private async void RefreshTemp()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, UpdateTempData);
        }

        private void UpdateTempData()
        {
            var entry = _sensor.GetTimeAndTemp();
                        
            Time.Text = FormatTime(entry.Time);
            Tempurature.Text = FormatTemperature(entry.Temperature);
            Humidity.Text = FormatHumidity(entry.Humidity);
        }

        private async void RefreshTime()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, UpdateTimeData);
        }

        private void UpdateTimeData()
        {
            Time.Text = FormatTime(DateTime.Now);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateTempData();
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
