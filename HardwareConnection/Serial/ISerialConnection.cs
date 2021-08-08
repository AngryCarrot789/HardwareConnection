using HardwareConnection.Connections;

namespace HardwareConnection.Serial {
    public interface ISerialConnection : IConnection {
        void Open();
        void Close();
    }
}