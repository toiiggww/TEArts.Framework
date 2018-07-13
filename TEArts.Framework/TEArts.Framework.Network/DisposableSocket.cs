using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TEArts.Framework.Network
{
    public partial class DisposableSocket : Socket
    {
        public DisposableSocket(SocketInformation socketInformation) : base(socketInformation)
        {
        }

        public DisposableSocket(SocketType socketType, ProtocolType protocolType) : base(socketType, protocolType)
        {
        }

        public DisposableSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : base(addressFamily, socketType, protocolType)
        {
        }

        public bool IsDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            (this as Socket).Reset();
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}
