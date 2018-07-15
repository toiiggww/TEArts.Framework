using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using TEArts.Framework.Collections;
using System.Threading;

namespace TEArts.Framework.Network
{
    partial class DisposableSocket
    {
        private CancellationTokenSource TokenSource;
        private CancellationToken mbrCancelToken;

        public WaitQueue<SocketAsyncEventArgs> SendOperatorQueue { get; private set; }
        public WaitQueue<SocketAsyncEventArgs> ReceiveOperatorQueue { get; private set; }
        public int SendOperatorCount { get; private set; }
        public int ReceiveOperatorCount { get; private set; }
        public CancellationToken Token
        {
            get { return mbrCancelToken; }
            set
            {
                if (value == mbrCancelToken) { return; }
                TokenSource.Cancel();
                mbrCancelToken = value;
                mbrCancelToken.Register(() => ChangeToken());
                TokenSource = CancellationTokenSource.CreateLinkedTokenSource(mbrCancelToken);
            }
        }

        private void ChangeToken()
        {
            throw new NotImplementedException();
        }

        private void InitOperators(WaitQueue<SocketAsyncEventArgs> queue, int queueCount, Action<SocketAsyncEventArgs> success, Action<SocketAsyncEventArgs> fail, Action<SocketAsyncEventArgs> fina)
        {
            if (TokenSource == null)
            {
                TokenSource = new CancellationTokenSource();
            }
            if (queue == null)
            {
                queue = new WaitQueue<SocketAsyncEventArgs>(queueCount, Token);
            }
            Parallel.For(0, queueCount, x => queue.Enqueue(InitEventArgs(success, fail, fina)));
        }
        private SocketAsyncEventArgs InitEventArgs(Action<SocketAsyncEventArgs> success, Action<SocketAsyncEventArgs> fail, Action<SocketAsyncEventArgs> fina)
        {
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.Completed += (o, x) =>
            {
                if (e.SocketError == SocketError.Success)
                {
                    success?.Invoke(x);
                }
                else
                {
                    fail?.Invoke(x);
                }
                fina?.Invoke(x);
            };
            return e;
        }

    }
}
