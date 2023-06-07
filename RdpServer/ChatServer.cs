using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RdpServer {
    public class ChatServer :IDisposable {
        private Socket server;
        private Socket client;

        private class NotConnectedException:IOException {
            public override string Message => base.Message + "Connection not established.";
        }

        public bool IsDisposed { get; internal set; }

        public ChatServer(int port = 5555) {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Any, port));
            server.Listen(100);
        }
        public void Accept() {
            client = server.Accept();
        }
        
        public void Send(string msg) {
            if (!Connected()) {
                throw new NotConnectedException();
            }
            client.Send(Encoding.Default.GetBytes(msg));
        }
        
        public string Receive() {
            if (!Connected()) {
                throw new NotConnectedException();
            }
            byte[] data = new byte[1024];
            client.Receive(data);
            return Encoding.Default.GetString(data);
        }

        public bool Connected() {
            if (client == null) { throw new ArgumentNullException("client"); }
            if (client.Poll(50, SelectMode.SelectRead)) {
                client.Close();
                client = null;
                return false;
            }
            return true;
        }

        protected virtual void Dispose(bool disposing) {
            if (!IsDisposed) {
                if (disposing) {
                    client.Close();
                    server.Close();
                }

                client = null;
                server = null;
                IsDisposed = true;
            }
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
