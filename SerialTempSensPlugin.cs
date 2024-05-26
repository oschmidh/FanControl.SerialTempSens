﻿using FanControl.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.IO.Ports;

namespace FanControl.SerialTempSensPlugin
{
    public class SerialTempSensPlugin : IPlugin
    {
        private bool _isInitialised = false;
        private SerialPort _serialPort;

        public string Name => "SerialTempSens";

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
            _serialPort.BaudRate = 115200;
            _serialPort.PortName = "COM3";  // TODO detect port somehow
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
            _serialPort.Open();
            _isInitialised = true;
        }

        public void Load(IPluginSensorsContainer _container)
        {
            if (_isInitialised)
            {
                SerialTempSens sensor = new SerialTempSens(_serialPort);
                _container.TempSensors.Add(sensor);
            }
        }
    }
}
