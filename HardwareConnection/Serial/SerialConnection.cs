using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using HardwareConnection.Packets;

namespace HardwareConnection.Serial {
    public class SerialConnection : IConnection {
        private Thread ReceiverThread;
        private volatile bool _canReceive;
        private volatile bool CanThreadRun;

        public SerialPort Port { get; }

        private StringWriter PacketWriteBuffer;

        public bool CanReceive {
            get => _canReceive;
            set => _canReceive = value;
        }

        public delegate void LineReceivedEventArgs(SerialConnection connection, string line);
        public event LineReceivedEventArgs OnLineReceived;

        public SerialConnection(string port, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, Handshake handshake = Handshake.None) {
            this.Port = new SerialPort(port, baudRate, parity, dataBits, stopBits);
            if (handshake != Handshake.None) {
                this.Port.Handshake = handshake;
            }

            this.CanThreadRun = true;
            this.ReceiverThread = new Thread(this.ReceiveMain) {
                Name = "REghZy Serial Reader"
            };

            this.PacketWriteBuffer = new StringWriter(new StringBuilder(256));
            this.ReceiverThread.Start();
        }

        public void Open(bool startReceiving = true) {
            this.Port.Open();
            this.CanReceive = startReceiving;
        }

        public void Close(bool stopReceiving = true) {
            this.CanReceive = stopReceiving;
            this.Port.Close();
        }

        public void Write(char c) {
            byte[] bytes = this.Port.Encoding.GetBytes(c.ToString());
            this.Port.Write(bytes, 0, bytes.Length);
        }

        public void Write(String text) {
            byte[] bytes = this.Port.Encoding.GetBytes(text);
            this.Port.Write(bytes, 0, bytes.Length);
        }

        public void WriteLine(char c) {
            Write(new StringBuilder().Append(c).Append('\n').ToString());
        }

        public void WriteLine(String c) {
            Write(new StringBuilder().Append(c).Append('\n').ToString());
        }

        public void SendPacket(Packet packet) {
            Packet.WritePacket(PacketWriteBuffer, packet);
            PacketWriteBuffer.Flush();
            WriteLine(PacketWriteBuffer.ToString());
            PacketWriteBuffer.GetStringBuilder().Clear();
        }

        private void ReceiveMain() {
            StringBuilder buffer = new StringBuilder(128);
            SerialPort port = this.Port;
            while (CanThreadRun) {
                if (CanReceive && port.IsOpen) {
                    while (port.BytesToRead > 0) {
                        char read = (char) port.ReadChar();
                        if (read == '\r')
                            continue;

                        if (read == '\n') {
                            if (OnLineReceived != null) {
                                OnLineReceived(this, buffer.ToString());
                            }

                            buffer.Clear();
                            continue;
                        }

                        buffer.Append(read);
                    }

                    Thread.Sleep(1);
                }

                Thread.Sleep(5);
            }
        }
    }
}