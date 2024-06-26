﻿using FanControl.Plugins;
using System;
using System.IO.Ports;
using Google.Protobuf;
using System.IO;

namespace FanControl.SerialTempSensPlugin
{
    internal class SerialTempSens : IPluginSensor
    {
        public SerialTempSens(SerialPort serialPort) { _serialPort = serialPort; }

        private readonly uint _sensIndex = 0; // TODO set index

        public float? Value { get; private set; }

        public string Name => $"NTC Sensor {(int)_sensIndex + 1}";

        public string Id => "Sens_" + _sensIndex.ToString();

        public void Update()
        {
            Command cmd = new Command { SensorId = _sensIndex };

            byte[] msg;
            using (var stream = new MemoryStream())
            {
                cmd.WriteDelimitedTo(stream);
                msg = stream.ToArray();
            }

            _serialPort.Write(msg, 0, msg.Length);

            try
            {
                var len = _serialPort.ReadByte();

                byte[] rply = new byte[len];
                _serialPort.Read(rply, 0, len);

                Reply reply = new Reply();
                reply.MergeFrom(rply);

                Value = reply.Temperature / 1000.0F;   // Data is in m°C
            }
            catch (TimeoutException) { }
        }

        private SerialPort _serialPort;
    }
}
