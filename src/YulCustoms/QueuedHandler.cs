using System.Collections.Concurrent;
using System.Threading;

namespace YulCustoms
{
    public class QueuedHandler<T> : IHandle<T>, IAmMonitored
        where T : IMessage
    {
        private readonly string name;
        private readonly IHandle<T> next;

        private readonly ConcurrentQueue<T> queue;
        private int total;

        public QueuedHandler(IHandle<T> next, string name)
        {
            this.next = next;
            this.name = name;
            queue = new ConcurrentQueue<T>();
        }

        public string Name
        {
            get { return name; }
        }

        public int Count
        {
            get { return queue.Count; }
        }

        public int Total
        {
            get { return total; }
        }

        public void Handle(T message)
        {
            Interlocked.Increment(ref total);
            queue.Enqueue(message);
        }

        public void Start()
        {
            new Thread(Run).Start();
        }

        private void Run()
        {
            while (true)
            {
                T message;
                queue.TryDequeue(out message);
                if (message != null)
                {
                    next.Handle(message);
                }
                Thread.Sleep(1);
            }
        }
    }
}