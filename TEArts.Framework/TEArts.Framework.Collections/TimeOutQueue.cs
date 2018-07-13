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
        public long Millisecond { get; private set; }
        public DateTime Next { get; private set; }
        public TimeOutQueue() : base()
        {
            Timer = new Timer(x =>
            {
                DateTime dt = ((DateTime)(x));
                if (!List.ContainsKey(dt))
                {
                    return;
                }
                List<TimedItem<TEArtsType>> t = List[dt];
                foreach (TimedItem<TEArtsType> tt in t)
                {
                    if (tt.NextTimer < 0)
                    {
                        tt.Callback(tt.Item);
                    }
                    else
                    {
                        tt.NextTimer -= long.MaxValue;
                        Add(tt);
                    }
                }
                t.Clear();
                List.Remove(dt);
                nextTimer();
            }, Next, Timeout.Infinite, Timeout.Infinite);
            Next = DateTime.MaxValue;
        }
        public TimeOutQueue(long mill) : this()
        {
            Millisecond = mill;
        }
        public TimedItem<TEArtsType> Add(TEArtsType item, Action<TEArtsType> onTimeOut)
        {
            return Add(item, Millisecond, onTimeOut);
        }
        public TimedItem<TEArtsType> Add(TEArtsType item, long mill, Action<TEArtsType> onTimeOut)
        {
            TimedItem<TEArtsType> i = new TimedItem<TEArtsType>(Millisecond);
            return Add(i);
        }
        public TimedItem<TEArtsType> Add(TimedItem<TEArtsType> i)
        {
            if (i.TimeOut <= DateTime.Now)
            {
                i.ResetTime();
            }
            long m = 0;
            TimeSpan t = (i.TimeOut - DateTime.Now);
            if (t.TotalMilliseconds > long.MaxValue)
            {
                m = long.MaxValue;
                i.NextTimer = t.TotalMilliseconds - long.MaxValue;
            }
            else
            {
                m = (long)(t.TotalMilliseconds);
            }
            List<TimedItem<TEArtsType>> ts = null;
            lock (List)
            {
                if (List.ContainsKey(i.TimeOut))
                {
                    ts = List[i.TimeOut];
                }
                else
                {
                    ts = new List<TimedItem<TEArtsType>>();
                }
                ts.Add(i);
                if (Next > i.TimeOut)
                {
                    Timer.Change(m, Timeout.Infinite);
                }
            }
            return i;
        }

        public void Remove(TimedItem<TEArtsType> item)
        {
            lock (List)
            {
                if (List.ContainsKey(item.TimeOut))
                {
                    List[item.TimeOut].Remove(item);
                }
                if (List[item.TimeOut].Count == 0)
                {
                    nextTimer();
                }
            }
        }
        private void nextTimer()
        {
            if (List.Keys.Count > 0)
            {
                Next = List.Keys.Min();
                Timer.Change(((long)((Next - DateTime.Now).TotalMilliseconds)), Timeout.Infinite);
            }
            else
            {
                Timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }
        public void Clear()
        {
            List.Clear();
            nextTimer();
        }
        public void Dispose()
        {
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
            Timer.Dispose();
            List.Clear();
        }
    }
    public class TimedItem<TEArtsType>
    {
        public TimedItem(double millisecond)
        {
            if (millisecond < 0) millisecond = 50;
            TimeOut = DateTime.Now.AddMilliseconds(millisecond);
            NextTimer = -1;
        }
        public TimedItem(TEArtsType value, DateTime timeout, Action<TEArtsType> action)
        {
            Item = value;
            Callback = action;
            if (timeout < DateTime.Now)
            {
                timeout = DateTime.Now.AddMilliseconds(50);
            }
            TimeOut = timeout;
            NextTimer = -1;
        }
        internal void ResetTime()
        {
            TimeOut = DateTime.Now.AddMilliseconds(50);
        }
        internal double NextTimer { get; set; }
        public TEArtsType Item { get; private set; }
        public DateTime TimeOut { get; private set; }
        public Action<TEArtsType> Callback { get; private set; }
    }
}
