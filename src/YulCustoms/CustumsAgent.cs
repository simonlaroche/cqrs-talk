using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace YulCustoms
{
    public class CustumsAgent: IHandle<CustomsDeclaration>
    {
        private readonly IHandle<CustomsDeclaration> next;
        private readonly string name;
        private readonly IList<int> badTravellers = new List<int>(){4, 10,149, 212, 453};

        public CustumsAgent(IHandle<CustomsDeclaration> next, string name)
        {
            this.next = next;
            this.name = name;
        }

        public void Handle(CustomsDeclaration message)
        {
            // Ask silly questions... watch the traveller's reaction
            Thread.Sleep(2000);
            if (badTravellers.Any(x => message.Name.Contains(x.ToString())))
            {
                message.SecretCode = "X-Ray";
                Console.WriteLine("Custom agent {0} marked traveller {1} for strip search",name ,message);
            }
            else
            {
                message.SecretCode = "R13";
                Console.WriteLine("Custom agent {0} marked traveller {1} to go through",name ,message);

            }

            next.Handle(message);
            
        }
    }
}