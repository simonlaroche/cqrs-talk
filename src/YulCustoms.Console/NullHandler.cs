using System.Threading;

namespace YulCustoms.Console
{
    public class NullHandler<TMessage> : IHandle<TMessage> where TMessage : IMessage
    {
        private readonly string name;

        public NullHandler(string name)
        {
            this.name = name;
            
        }

        public void Handle(TMessage message)
        {
            System.Console.WriteLine("Handler {0} handled {1}", name, message);
            Thread.Sleep(1000);
        }
    }
}