using System.IO.Ports;
using HardwareConnection.Serial;

namespace HardwareConnection.Arduino {
    public class ArduinoConnection : SerialConnection {
        public ArduinoConnection(string port, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, Handshake handshake = Handshake.None) : base(port, baudRate, parity, dataBits, stopBits, handshake) {

        }
    }
}