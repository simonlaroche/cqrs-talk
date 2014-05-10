using System.Collections.Concurrent;
using YulCustoms.Messaging;

namespace YulCustoms
{
    public class ProcessManager : IHandle<InterviewTimedOut>, IHandle<TravellerInterviewed>
    {
        private readonly IPublish publish;
        private bool interviewCompleted;

        public ProcessManager(IPublish publish)
        {
            this.publish = publish;
        }

        public void Handle(TravellerArrived message)
        {
            publish.Publish(new InterviewTraveller(){CorrelationId = message.CorrelationId, Declaration = message.Declaration});
            
            //Wake in 2 secs if we haven't heard from customs agent
            publish.Publish(new WakeMeIn(){CorrelationId = message.CorrelationId, TTL = 5,Message = new InterviewTimedOut{CorrelationId = message.CorrelationId, Declaration = message.Declaration}});
        }

        public void Handle(InterviewTimedOut message)
        {
            if (!interviewCompleted)
            {
                publish.Publish(new InterviewTraveller()
                {
                    CorrelationId = message.CorrelationId,
                    Declaration = message.Declaration
                });
                publish.Publish(new WakeMeIn()
                {
                    CorrelationId = message.CorrelationId,
                    TTL = 2,
                    Message =
                        new InterviewTimedOut {CorrelationId = message.CorrelationId, Declaration = message.Declaration}
                });
            }
        }

        public void Handle(TravellerInterviewed message)
        {
            if (!interviewCompleted) 
            {
                interviewCompleted = true;

                publish.Publish(new TravellerCustomsProcessCompleted()
                {
                    CorrelationId = message.CorrelationId,
                    Declaration = message.Declaration
                });
            }
        }
    }
}