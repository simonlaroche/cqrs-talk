using System;
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
}