using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using YulCustoms.Messaging;

namespace YulCustoms
{
    public class ProcessManagerHouse: IHandle<TravellerArrived>, IHandle<TravellerCustomsProcessCompleted>
    {
        private readonly Dispatcher dispatcher;
        private Dictionary<Guid, ProcessManager> runningProcesses = new Dictionary<Guid, ProcessManager>();

        public ProcessManagerHouse(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public void Handle(TravellerCustomsProcessCompleted message)
        {
            var runningProcess = runningProcesses[message.CorrelationId];
            dispatcher.Subscribe<InterviewTimedOut>(runningProcess, message.CorrelationId.ToString());
            dispatcher.Subscribe<TravellerInterviewed>(runningProcess, message.CorrelationId.ToString());
            dispatcher.Unsubscribe<TravellerCustomsProcessCompleted>(this, message.CorrelationId.ToString());
            runningProcesses.Remove(message.CorrelationId);
        }

        public void Handle(TravellerArrived message)
        {
            message.CorrelationId = message.Id;
            var pm = new ProcessManager(dispatcher);
            runningProcesses.Add(message.CorrelationId, pm);
            dispatcher.Subscribe<InterviewTimedOut>(pm, message.CorrelationId.ToString());
            dispatcher.Subscribe<TravellerInterviewed>(pm, message.CorrelationId.ToString());
            dispatcher.Subscribe<TravellerCustomsProcessCompleted>(this, message.CorrelationId.ToString());
            pm.Handle(message);
        }
    }

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