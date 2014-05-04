using System.Collections.Generic;

namespace YulCustoms.Messaging
{
    public class Dispatcher:IPublish
    {

        private readonly Dictionary<string, Multiplexer<IMessage>> topics = new Dictionary<string, Multiplexer<IMessage>>();
        private readonly QueuedHandler<IMessage> queudHandler;

        public Dispatcher()
        {
            queudHandler = new QueuedHandler<IMessage>(new Handler(topics), "dispatcher");
        }

        public QueuedHandler<IMessage> QueudHandler
        {
            get { return queudHandler; }
        }

        private void Publish<T>(string topic, T message) where T : IMessage
        {
            if (topics.ContainsKey(topic))
            {
                topics[topic].Handle(message);
            }
            var corolationTopic = message.CorrelationId.ToString();
            if (topics.ContainsKey(corolationTopic))
                topics[corolationTopic].Handle(message);
        }

        public void Publish<T>(T message) where T : IMessage
        {
            QueudHandler.Handle(message);
            //Publish(message.GetType().Name, message);
        }

        public void Subscribe<T>(IHandle<T> handler) where T : IMessage
        {
            Subscribe(handler, typeof(T).Name);
        }

        public void Subscribe<T>(IHandle<T> handler, string topic) where T : IMessage
        {
            if (!topics.ContainsKey(topic))
            {
                topics.Add(topic, new Multiplexer<IMessage>());
            }

            var multi = topics[topic];

            multi.AddHandler(new Narrower<IMessage, T>(handler));
        }

        public void Unsubscribe<T>(IHandle<T> handler, string topic) where T : IMessage
        {
            var multi = topics[topic];

            multi.RemoveHandler(new Narrower<IMessage, T>(handler));
        }

        public void Handle(IMessage message)
        {
            Publish(message);
        }

        private class Handler : IHandle<IMessage>
        {
            private Dictionary<string, Multiplexer<IMessage>> topics;

            public Handler(Dictionary<string, Multiplexer<IMessage>> topics)
            {
                this.topics = topics;
            }

            public void Handle(IMessage message)
            {
                string topic = message.GetType().Name;
                if (topics.ContainsKey(topic))
                {
                    topics[topic].Handle(message);
                }
                var corolationTopic = message.CorrelationId.ToString();
                if (topics.ContainsKey(corolationTopic))
                    topics[corolationTopic].Handle(message);
            }
        }

        public void Start()
        {
            QueudHandler.Start();
        }
    }

    public interface IPublish
    {
        void Publish<T>(T message) where T : IMessage;
    }
}