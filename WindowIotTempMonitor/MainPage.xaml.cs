using GrovePi;
using GrovePi.Sensors;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WindowIotTempMonitor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IDHTTemperatureAndHumiditySensor _sensor;
        private int _counter; 

        public UpdatingText Time;
        public UpdatingText Tempurature;
        public UpdatingText Humidity;
        public UpdatingText Counter; 

        public MainPage()
        {
            InitializeComponent();
            
            _sensor = DeviceFactory.Build.DHTTemperatureAndHumiditySensor(Pin.DigitalPin8, DHTModel.Dht11);
            _counter = 0; 

            Time = UpdatingText.CreateNewTimeAndTemp();
            Tempurature = UpdatingText.CreateNewTimeAndTemp();
            Humidity = UpdatingText.CreateNewTimeAndTemp();
            Counter = UpdatingText.CreateNewTimeAndTemp();

            UpdateText();
        }

        private void UpdateText()
        {
            _sensor.Measure();
            _counter++;
            
            Time.Text = GetTime();
            Tempurature.Text = GetTemperatureReading();
            Humidity.Text = GetHumidityReading();
            Counter.Text = _counter.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateText();
        }

        private string GetDate()
        {
            return DateTime.Today.ToString("D");
        }

        private string GetTime()
        {
            return DateTime.Now.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
        }

        private string GetTemperatureReading()
        {
            return string.Format("Temp: {0}F", _sensor.TemperatureInFahrenheit);
        }

        private string GetHumidityReading()
        {
            return string.Format("Humidity: {0}%", _sensor.Humidity);
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
