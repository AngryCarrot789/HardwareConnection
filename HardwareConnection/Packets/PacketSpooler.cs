using System;
using System.Collections.Generic;
using System.Threading;
using HardwareConnection.Connections;
using HardwareConnection.Packets.Packets;
using HardwareConnection.Serial;

namespace HardwareConnection.Packets {
    /// <summary>
    /// A threaded spooler for packets
    /// </summary>
    public class PacketSpooler {
        private static int SpoolersCount;

        private readonly Thread WriteThread;
        private readonly Stack<Packet> Queue;
        private readonly IConnection _connection;
        private bool CanThreadRun;

        public IConnection Connection => _connection;

        public PacketSpooler(IConnection connection) {
            this._connection = connection;
            this.Queue = new Stack<Packet>();
            this.CanThreadRun = true;
            this.WriteThread = new Thread(this.WriteMain);
            this.WriteThread.Name = $"REghZy Packet Spooler {SpoolersCount++}";
            this.WriteThread.Start();
        }

        [MTAThread]
        private void WriteMain() {
            while (CanThreadRun) {
                WriteNextPacket();
                Thread.Sleep(1);
            }
        }

        public void QueuePacket(Packet packet) {
            lock (this.Queue) {
                this.Queue.Push(packet);
            }
        }

        [STAThread]
        public void WriteNextPacket() {
            if (CanWrite()) {
                lock (this.Queue) {
                    this._connection.SendPacket(this.Queue.Pop());
                }
            }
        }

        public bool CanWrite() {
            return this.Queue.Count != 0;
        }

        public void StopThread() {
            this.CanThreadRun = false;
        }
    }
}