using NickAc.Backend.Networking.TcpLib;
using NickAc.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ButtonDeck.Misc
{
    public class ServerThread
    {
        readonly Thread baseThread;

        public Thread BaseThread {
            get {
                return baseThread;
            }
        }

        TcpServer tcpServer;

        public TcpServer TcpServer {
            get {
                return tcpServer;
            }
        }

        public ServerThread()
        {
            baseThread = new Thread(RunServer);
        }

        public void Start()
        {
            baseThread?.Start();
        }

        private void RunServer()
        {
            tcpServer = new TcpServer(new DeckServiceProvider(), Constants.PORT_NUMBER);
            Program.SuccessfulServerStart = tcpServer.Start();
            
        }

        public void Stop()
        {
            tcpServer?.Stop();
            baseThread?.Abort();
        }
    }
}
