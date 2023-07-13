using FanControl.Plugins;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl.UartSensorPlugin
{
    internal class UartSensor : IPluginSensor
    {
        public UartSensor(SerialPort serialPort) { _serialPort = serialPort; }

        private readonly int _sensIndex; // TODO set index

        public float? Value { get; private set; }

        public string Name => $"Uart TemperatureSensor {(int)_sensIndex + 1}";

        public string Id => "Sens_" + _sensIndex.ToString();

        public void Update()
        {
            _serialPort.WriteLine("G");
            try
            {
                string temperature = _serialPort.ReadLine();
                Console.WriteLine(temperature);
                Value = float.Parse(temperature);
            }
            catch (TimeoutException) { }
        }

        private SerialPort _serialPort;
    }
}
