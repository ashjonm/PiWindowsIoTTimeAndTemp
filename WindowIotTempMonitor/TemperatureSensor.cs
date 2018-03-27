using GrovePi;
using GrovePi.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowIotTempMonitor
{
    public class TemperatureSensor
    {
        private IDHTTemperatureAndHumiditySensor _sensor;

        public TemperatureSensor()
        {
            _sensor = DeviceFactory.Build.DHTTemperatureAndHumiditySensor(Pin.DigitalPin8, DHTModel.Dht11);
        }
        
        public TempEntry GetTimeAndTemp()
        {
            _sensor.Measure();

            var entry = new TempEntry
            {
                Time = DateTime.Now,
                Temperature = (Decimal)_sensor.TemperatureInFahrenheit,
                Humidity = (Decimal)_sensor.Humidity
            };

            return entry;
        }
    }

    public class TempEntry
    {
        public DateTime Time { get; set; }
        public Decimal Temperature { get; set; }
        public Decimal Humidity { get; set; }
    }
}
