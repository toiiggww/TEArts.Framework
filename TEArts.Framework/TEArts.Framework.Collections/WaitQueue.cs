using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace TEArts.Framework.Collections
{
    public class WaitQueue<T> : ConcurrentQueue<T>
    {
        private CancellationToken mbrCancelToken;
        public event EventHandler<QueueFullEventArgs<T>> OnQueueFull;

        public WaitQueue(CancellationToken cancelToken) : this(-1, cancelToken) { }

        public void Clear()
        {
            while (Count > 0)
            {
                TryDequeue(out T x);
                x = default(T);
            }
            WaitHandle.Set();
            MaxSize = 0;
            //CancellationTokenSource.CreateLinkedTokenSource(CancelToken).Cancel();
        }

        public WaitQueue(int max, CancellationToken cancelToken) // : base(max > 0 ? max : 4)
        {
            MaxSize = max;
            CancelToken = cancelToken;
            WaitHandle = new AutoResetEvent(true);
        }

        public CancellationToken CancelToken
        {
            get { return mbrCancelToken; }
            set
            {
                if (value == mbrCancelToken) { return; }
                mbrCancelToken = value;
                mbrCancelToken.Register(() => WaitHandle.Set());
            }
        }
        public int MaxSize { get; private set; }
        public AutoResetEvent WaitHandle { get; private set; }
        public int MaxCountOfDequeue { get; private set; } = 100;

        public T Dequeue() { return Dequeue(-1); }
        public T Dequeue(int millisecondsTimeout)
        {
            T t = default(T);
            if (CancelToken.IsCancellationRequested)
            {
                return t;
            }
            Wait(millisecondsTimeout);
            if (CancelToken.IsCancellationRequested)
            {
                return t;
            }
            TryDequeue(out t);
            return t;
        }

        public new void Enqueue(T item)
        {
            if (MaxSize > 0 && Count == MaxSize)
            {
                T value = Dequeue();
                OnQueueFull?.Invoke(this, new QueueFullEventArgs<T>() { Dequeued = value, Max = MaxSize });
            }
            base.Enqueue(item);
            if (Count <= 5 || (Count > (MaxCountOfDequeue / 20) && 0 == Count % (MaxCountOfDequeue / 20)))
            {
                WaitHandle.Set();
            }
        }

        public List<T> Dequeue(int count = -1, int millisecondsTimeout = -1)
        {
            return Dequeue(null, count, millisecondsTimeout);
        }

        public List<T> Dequeue(List<T> value, int count, int millisecondsTimeout)
        {
            if (value == null)
            {
                value = new List<T>();
            }
            if (count <= 0)
            {
                count = MaxCountOfDequeue;
            }
            T t = default(T);
            millisecondsTimeout = Wait(millisecondsTimeout);
            int c = 0;
            while (c < count)
            {
                if (CancelToken.IsCancellationRequested)
                {
                    break;
                }
                millisecondsTimeout = Wait(millisecondsTimeout);
                if (CancelToken.IsCancellationRequested)
                {
                    break;
                }
                if (TryDequeue(out t))
                {
                    value.Add(t);
                }
                else
                {
                    break;
                }
                c++;
            }
            return value;
        }

        private int Wait(int millisecondsTimeout)
        {
            DateTime waitStart = DateTime.Now;
            if (Count == 0)
            {
                WaitHandle.WaitOne(millisecondsTimeout);
            }
            DateTime waitEnd = DateTime.Now;
            if (millisecondsTimeout == -1)
            {
                millisecondsTimeout = 0;
            }
            else if (millisecondsTimeout > 0)
            {
                int waited = ((int)(waitEnd - waitStart).TotalMilliseconds);
                if (waited > int.MaxValue || waited < 0)
                {
                    millisecondsTimeout = 0;
                }
                else
                {
                    millisecondsTimeout -= waited;
                }
            }

            return millisecondsTimeout;
        }
        public void Reset()
        {
            while (Count > 0)
            {
                TryDequeue(out T t);
            }
            WaitHandle.Reset();
        }
    }

    public class WaitQueue : WaitQueue<object>
    {
        public WaitQueue(CancellationToken cancelToken) : base(cancelToken)
        {
        }

        public WaitQueue(int max, CancellationToken cancelToken) : base(max, cancelToken)
        {
        }
    }

    public class QueueFullEventArgs<T> : EventArgs
    {
        public int Max { get; set; }
        public T Dequeued { get; set; }
    }
}
