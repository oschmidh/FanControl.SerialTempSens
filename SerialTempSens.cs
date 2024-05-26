using FanControl.Plugins;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl.SerialTempSensPlugin
{
    internal class SerialTempSens : IPluginSensor
    {
        public SerialTempSens(SerialPort serialPort) { _serialPort = serialPort; }

        private readonly int _sensIndex; // TODO set index

        public float? Value { get; private set; }

        public string Name => $"SerialTempSens Sensor {(int)_sensIndex + 1}";

        public string Id => "Sens_" + _sensIndex.ToString();

        public void Update()
        {
            try
            {
                // TODO implement
            }
            catch (TimeoutException) { }
        }

        private SerialPort _serialPort;
    }
}
