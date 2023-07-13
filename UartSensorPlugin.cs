using FanControl.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.IO.Ports;

namespace FanControl.UartSensorPlugin
{
    public class UartSensorPlugin : IPlugin
    {
        private bool _isInitialised = false;
        private SerialPort _serialPort;

        public string Name => "UartSensor";

        public void Close()
        {
            if (_isInitialised)
            {
                _serialPort.Close();
                _isInitialised = false;
            }
        }

        public void Initialize()
        {
            _serialPort = new SerialPort();
            _serialPort.BaudRate = 9600;
            _serialPort.PortName = "COM3";  // TODO detect port somehow
            _serialPort.Open();
            _isInitialised = true;

        }

        public void Load(IPluginSensorsContainer _container)
        {
            if (_isInitialised)
            {
                UartSensor sensor = new UartSensor(_serialPort);
                _container.TempSensors.Add(sensor);
            }
        }
    }
}
