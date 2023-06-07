using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RdpClient {
    public class ChatClient :IDisposable {
        private Socket client;
        
        public bool IsDisposed { get; internal set; }

        public ChatClient(int connectPort = 5555) {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(new IPEndPoint(IPAddress.Parse(Program.Ip), connectPort));
        }
        public void Disconnect() {
            client.Disconnect(false);
        }

        public void Send(string msg) {
            client.Send(Encoding.Default.GetBytes(msg));
        }
        
        public string Receive() {
            byte[] data = new byte[1024];
            client.Receive(data);
            return Encoding.Default.GetString(data);
        }

        protected virtual void Dispose(bool disposing) {
            if (!IsDisposed) {
                if (disposing) {
                    client.Close();
                }

                client = null;
                IsDisposed = true;
            }
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
