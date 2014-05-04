using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace YulCustoms.Messaging
{
    public class AlarmClock : IHandle<WakeMeIn>
    {
        private readonly IPublish publisher;
        private readonly SortedList list = new SortedList();
        private readonly object locker = new object();
        private Stopwatch clock;

        public AlarmClock(IPublish publisher)
        {
            this.publisher = publisher;
        }

        public void Handle(WakeMeIn message)
        {
            lock (locker)
            {
                long timeOut = clock.ElapsedTicks + message.TTL * 1000000;
                list.Add(timeOut, new Tuple<long, WakeMeIn>(timeOut, message));
            }
        }

        public void Start()
        {
            clock = new Stopwatch();
            clock.Start();
            new Thread(Run).Start();
        }

        private void Run()
        {
            while (true)
            {
                lock (locker)
                {
                    while (true)
                    {
                        if (list.Count == 0)
                            break;

                        object o = list.GetByIndex(0);
                        var item = (Tuple<long, WakeMeIn>)o;
                        if (item.Item1 < clock.ElapsedTicks)
                        {
                            list.Remove(item.Item1);
                            publisher.Publish(item.Item2.Message);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                Thread.Sleep(1); //Give chance to add time outs
            }
        }
    }
}