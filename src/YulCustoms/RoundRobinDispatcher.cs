using System.Collections.Generic;

namespace YulCustoms
{
    public class RoundRobinDispatcher<TMessage>: IHandle<TMessage> where TMessage : IMessage
    {
        private Queue<IHandle<TMessage>> queue;

        public RoundRobinDispatcher(IHandle<TMessage>[] handlers)
        {
            queue = new Queue<IHandle<TMessage>>(handlers.Length);
            foreach (var handler in handlers)
            {
                queue.Enqueue(handler);
            }
        }

        public void Handle(TMessage message)
        {
            var handler = queue.Dequeue();
            handler.Handle(message);
            queue.Enqueue(handler);
        }
    }
}