using System.Net.Sockets;

namespace TEArts.Framework.Extends
{
    public static class SocketExtends
    {
        private static readonly LingerOption Linger = new LingerOption(true, 0);
        public static LingerOption ResetLinger(this Socket socket) { return Linger; }
        public static void Reset(this Socket socket)
        {
            if (socket != null)
            {
                try { socket.LingerState = Linger; } catch { }
            }
        }
    }
}
