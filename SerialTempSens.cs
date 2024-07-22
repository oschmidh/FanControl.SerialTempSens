using FanControl.Plugins;
using System;
using System.IO.Ports;
using Google.Protobuf;
using System.IO;

namespace FanControl.SerialTempSensPlugin
{
    internal class SerialTempSens : IPluginSensor
    {
        public SerialTempSens(SerialPort serialPort, IPluginLogger logger,uint sensIndex)
        {
            _serialPort = serialPort;
            _sensIndex = sensIndex;
            _logger = logger;
        }

        private readonly uint _sensIndex;

        public float? Value { get; private set; }

        public string Name => $"NTC Sensor {(int)_sensIndex + 1}";

        public string Id => "Sens_" + _sensIndex.ToString();

        private IPluginLogger _logger;

        public bool IsPresent()
        {
            Command cmd = new Command { SensorId = _sensIndex };
            Reply? rply = sendCommand(cmd);
            if (rply == null)
            {
                return false;
            }
            return rply.Error != ErrorCode.SensorOpen && rply.Error != ErrorCode.InvalidSensorId;
        }

        public void Update()
        {
            Command cmd = new Command { SensorId = _sensIndex };
            Reply? rply = sendCommand(cmd);
            if (rply == null || rply.Error != ErrorCode.NoError)
            {
                Value = null;
                return;
            }

            Value = rply.Temperature / 1000.0F;   // Data is in m°C
        }

        private Reply? sendCommand(Command cmd)
        {
            byte[] msg;
            using (var stream = new MemoryStream())
            {
                cmd.WriteDelimitedTo(stream);
                msg = stream.ToArray();
            }

            _serialPort.Write(msg, 0, msg.Length);

            byte[] rply;
            try
            {
                var len = _serialPort.ReadByte();

                rply = new byte[len];
                _serialPort.Read(rply, 0, len);
            }
            catch (TimeoutException) { return null; }

            Reply reply = new Reply();
            try
            {
                reply.MergeFrom(rply);
            }
            catch {
                String logMsg = "failed to parse protobuf reply ";
                foreach (byte b in rply) {
                    logMsg += b.ToString();
                }
                _logger.Log(logMsg);
                return null;
            }
            return reply;
        }

        private SerialPort _serialPort;
    }
}
