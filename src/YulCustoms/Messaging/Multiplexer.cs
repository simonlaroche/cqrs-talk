using System.Collections.Generic;
using System.Linq;

namespace YulCustoms.Messaging
{
    public class Multiplexer<T> : IHandle<T> where T : IMessage
    {
        private List<IHandle<T>> messageHandlers = new List<IHandle<T>>();

        public void AddHandler(IHandle<T> handler)
        {
            var newHandlers = this.messageHandlers.ToList();
            newHandlers.Add(handler);
            messageHandlers = newHandlers;
        }

        public void RemoveHandler(IHandle<T> handler)
        {
            var newHandlers = this.messageHandlers.ToList();
            newHandlers.Remove(handler);
            this.messageHandlers = newHandlers;
        }

        public void Handle(T message)
        {
            foreach (var handler in messageHandlers)
            {
                handler.Handle(message);
            }
        }
    }
}