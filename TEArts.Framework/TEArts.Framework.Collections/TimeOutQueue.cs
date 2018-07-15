using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TEArts.Framework.Collections
{
    public class TimeOutQueue<TEArtsType> : IDisposable
    {
        private SortedList<DateTime, List<TimedItem<TEArtsType>>> List = new SortedList<DateTime, List<TimedItem<TEArtsType>>>();
        private Timer Timer;
        //public EventHandler<TEArtsType> OnTimeOut;
        public long Millisecond { get; private set; } = 100;
        public DateTime Next { get; private set; }
        public TimeOutQueue() : base()
        {
            Timer = new Timer(x =>
            {
                DateTime dt = ((DateTime)(x));
                if (List.TryGetValue(dt, out List<TimedItem<TEArtsType>> t))
                {
                    foreach (TimedItem<TEArtsType> tt in t)
                    {
                        if (tt.NextTimer <= 0)
                        {
                            tt.Callback?.Invoke(tt.Item);
                        }
                        else
                        {
                            tt.NextTimer -= long.MaxValue;
                            Add(tt);
                        }
                    }
                    t.Clear();
                    List.Remove(dt);
                }
                NextTimer();
            }, Next, Timeout.Infinite, Timeout.Infinite);
        }
        public TimeOutQueue(long mill) : this()
        {
            Millisecond = mill;
        }
        public TimedItem<TEArtsType> Add(TEArtsType item, Action<TEArtsType> onTimeOut)
        {
            return Add(new TimedItem<TEArtsType>(item, DateTime.Now.AddMilliseconds(Millisecond), onTimeOut));
        }
        public TimedItem<TEArtsType> Add(TEArtsType item, long mill, Action<TEArtsType> onTimeOut)
        {
            return Add(new TimedItem<TEArtsType>(item, DateTime.Now.AddMilliseconds(mill), onTimeOut));
        }
        public TimedItem<TEArtsType> Add(TimedItem<TEArtsType> i)
        {
            if (i.TimeOut <= DateTime.Now)
            {
                i.Callback?.Invoke(i.Item);
            }
            else
            {
                lock (List)
                {
                    if (!List.TryGetValue(i.TimeOut, out List<TimedItem<TEArtsType>> ts))
                    {
                        ts = new List<TimedItem<TEArtsType>>();
                        List.Add(i.TimeOut, ts);
                    }
                    ts.Add(i);
                    NextTimer();
                }
            }
            return i;
        }

        public void Remove(TimedItem<TEArtsType> item)
        {
            if (item == null)
            {
                return;
            }
            lock (List)
            {
                if (List.ContainsKey(item.TimeOut))
                {
                    List[item.TimeOut].Remove(item);
                }
                if (List[item.TimeOut].Count == 0)
                {
                    List.Remove(item.TimeOut);
                    NextTimer();
                }
            }
        }
        public void Clear(bool invoke = false)
        {
            List.Clear();
            if (invoke) { NextTimer(); }
        }
        private void NextTimer()
        {
            if (List.Keys.Count > 0)
            {
                DateTime min = List.Keys.Min();
                if (min <= Next || Next <= DateTime.Now)
                {
                    Next = min;
                    long l = Next <= DateTime.Now ? 0 : ((Next - DateTime.Now).TotalMilliseconds > long.MaxValue ? long.MaxValue : ((long)((Next - DateTime.Now).TotalMilliseconds)));
                    Timer.Change(l, Timeout.Infinite);
                }
            }
            else
            {
                Timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }
        public void Dispose()
        {
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
            Timer.Dispose();
            List.Clear();
        }
    }

    public class TimeOutQueue : TimeOutQueue<object>
    {

    }

    public class TimedItem<TEArtsType>
    {
        public TimedItem(TEArtsType value, DateTime timeout, Action<TEArtsType> action)
        {
            Item = value;
            Callback = action;
            if (timeout < DateTime.Now)
            {
                timeout = DateTime.Now.AddMilliseconds(50);
            }
            TimeOut = timeout;
            NextTimer = (timeout - DateTime.Now).TotalMilliseconds;
        }
        internal double NextTimer { get; set; }
        public TEArtsType Item { get; private set; }
        public DateTime TimeOut { get; private set; }
        public Action<TEArtsType> Callback { get; private set; }
    }
}
