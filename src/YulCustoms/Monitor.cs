using System;
using System.Collections.Generic;
using System.Threading;

namespace YulCustoms
{
    public class Monitor
    {
        private readonly List<IAmMonitored> subjects=new List<IAmMonitored>();

        public void Add(IAmMonitored subject)
        {
            subjects.Add(subject);
        }

        public void Start()
        {
            new Thread(Run).Start();
        }

        private void Run()
        {
            while (true)
            {
                foreach (var amMonitored in subjects)
                {
                    Console.WriteLine("*** Queue {0}, current count: {1}, total: {2}", amMonitored.Name, amMonitored.Count, amMonitored.Total);
                }
                Thread.Sleep(10000);    
            }
            
        }
    }

    public interface IAmMonitored
    {
        string Name { get; }
        int Count { get; }
        int Total { get; }
    }
}