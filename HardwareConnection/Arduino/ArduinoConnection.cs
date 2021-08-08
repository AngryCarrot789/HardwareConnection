using HardwareConnection.Packets;
using HardwareConnection.Serial;

namespace HardwareConnection.Arduino {
    public class ArduinoConnection : SerialConnection {
        public ArduinoConnection(string port) : base(new SerialTransceiver(port)) { }
        public ArduinoConnection(SerialTransceiver transceiver) : base(transceiver) { }
    }
}