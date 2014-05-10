using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using YulCustoms.Messaging;

namespace YulCustoms
{
    public class FuCustumAgent: IHandle<InterviewTraveller>
    {
        private readonly string name;

        public FuCustumAgent(string name)
        {
            this.name = name;
        }

        public void Handle(InterviewTraveller message)
        {
            Thread.Sleep(1000);
            throw new SleepingOnTheJobException(name);
        }
    }
    
    public class SleepingOnTheJobException : Exception
    {
        public SleepingOnTheJobException(string name):base(string.Format("Agent {0} is sleeping on the job.", name))
        {
            
        }
    }

    public class CustumsAgent : IHandle<InterviewTraveller>
    {
        private readonly IPublish publisher;
        private readonly string name;
        private readonly IList<int> badTravellersDb = new List<int>(){4, 10,149, 212, 453};

        public CustumsAgent(IPublish publisher, string name)
        {
            this.publisher = publisher;
            this.name = name;
        }

        public void Handle(InterviewTraveller message)
        {
            // Ask silly questions... watch the traveller's reaction
            Thread.Sleep(1000);
            if (badTravellersDb.Any(x => message.Declaration.Name.Contains(x.ToString())))
            {
                message.Declaration.SecretCode = "X-Ray";
                Console.WriteLine("Custom agent {0} marked traveller {1} for strip search",name ,message);
            }
            else
            {
                message.Declaration.SecretCode = "R13";
                Console.WriteLine("Custom agent {0} marked traveller {1} to go through",name ,message);
            }

            publisher.Publish(new TravellerInterviewed{CorrelationId = message.CorrelationId, Declaration = message.Declaration});
            
        }
}
}