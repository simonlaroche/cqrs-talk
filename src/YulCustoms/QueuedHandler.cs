namespace YulCustoms
{
    using System.Collections.Concurrent;
    using System.Threading;

    public class QueuedHandler<T>: IHandle<T>
        where T : IMessage
    {
        private readonly IHandle<T> next;

        private readonly ConcurrentQueue<T> queue;

        public QueuedHandler(IHandle<T> next)
        {
            this.next = next;
            queue = new ConcurrentQueue<T>();
        }

        public void Handle(T message)
        {
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