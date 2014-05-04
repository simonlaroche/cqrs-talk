using System;

namespace YulCustoms
{
    public class Archivist : IHandle<TravellerCustomsProcessCompleted>
    {
     
        public void Handle(TravellerCustomsProcessCompleted message)
        {
            Console.WriteLine("Archiving declaration {0} in giant datamart for future analysis", message.Declaration.Name);
        }
    }
}