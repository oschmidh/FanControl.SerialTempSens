using FanControl.Plugins;
using Microsoft.Win32;
using System.Collections.Generic;
using System;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.Versioning;

namespace FanControl.SerialTempSensPlugin
{
    public class SerialTempSensPlugin : IPlugin
    {
        private bool _isInitialised = false;
        private SerialPort _serialPort = new SerialPort();

        private const string usbVid = "2FE3";
        private const string usbPid = "0100";

        private const uint numSensors = 3;

        public string Name => "SerialTempSens";

        public void Initialize()
        {
            var ports = GetComPort(usbVid, usbPid);
            if (ports.Count < 1)
            {
                // TODO log error
                return;
            } else if (ports.Count > 1) 
            {
                // TODO log warning, multiple devices found, first port chosen?
            }

            _serialPort.BaudRate = 115200;
            _serialPort.PortName = ports[0];
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
            _serialPort.Open();
            _isInitialised = true;
        }

        public void Load(IPluginSensorsContainer _container)
        {
            if (!_isInitialised)
            {
                return;
            }

            for (uint i = 0; i < numSensors; ++i)
            {
                SerialTempSens sensor = new SerialTempSens(_serialPort, i);
                if (sensor.IsPresent())
                {
                    _container.TempSensors.Add(sensor);
                }
            }
        }

        public void Close()
        {
            if (_isInitialised)
            {
                _serialPort.Close();
                _isInitialised = false;
            }
        }

        // see https://stackoverflow.com/a/34918472
        private List<string> GetComPort(String VID, String PID)
        {
            String pattern = String.Format("^VID_{0}.PID_{1}", VID, PID);
            Regex _rx = new Regex(pattern, RegexOptions.IgnoreCase);
            List<string> comports = new List<string>();

            RegistryKey rk1 = Registry.LocalMachine;
            RegistryKey rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");

            foreach (String s3 in rk2.GetSubKeyNames())
            {
                RegistryKey rk3 = rk2.OpenSubKey(s3);
                foreach (String s in rk3.GetSubKeyNames())
                {
                    if (_rx.Match(s).Success)
                    {
                        RegistryKey rk4 = rk3.OpenSubKey(s);
                        foreach (String s2 in rk4.GetSubKeyNames())
                        {
                            RegistryKey rk5 = rk4.OpenSubKey(s2);
                            string location = (string)rk5.GetValue("LocationInformation");
                            RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                            string portName = (string)rk6.GetValue("PortName");
                            if (!String.IsNullOrEmpty(portName) && SerialPort.GetPortNames().Contains(portName))
                                comports.Add((string)rk6.GetValue("PortName"));
                        }
                    }
                }
            }
            return comports;
        }
    }
}
