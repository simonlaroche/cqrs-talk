using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YulCustoms.Messaging;

namespace YulCustoms.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var dispatcher = new Dispatcher();
            var alarmClock = new AlarmClock(dispatcher);
            dispatcher.Subscribe(alarmClock);

            var endOfProcess = new QueuedHandler<TravellerCustomsProcessCompleted>(new Archivist(), "end-of-process");
            dispatcher.Subscribe(endOfProcess);
            
            var customsAgent1 = new QueuedHandler<InterviewTraveller>(new CustumsAgent(dispatcher, "joe"), "agent-joe");
            var customsAgent2 = new QueuedHandler<InterviewTraveller>(new CustumsAgent(dispatcher, "jack"), "agent-jack");
            var customsAgent3 = new QueuedHandler<InterviewTraveller>(new ExceptionHandler<InterviewTraveller>(new FuCustumAgent("averell")), "agent-averell");

            var customsAgentDispatcher =
                new RoundRobinDispatcher<InterviewTraveller>(new[] { customsAgent1, customsAgent2, customsAgent3 });
            dispatcher.Subscribe(customsAgentDispatcher);


            var processManageHouse = new ProcessManagerHouse(dispatcher);
            dispatcher.Subscribe<TravellerArrived>(processManageHouse);            
            
            dispatcher.Start();
            customsAgent1.Start();
            customsAgent2.Start();
            customsAgent3.Start();
            endOfProcess.Start();
            alarmClock.Start();

            var monitor = new Monitor();
            monitor.Add(endOfProcess);
            monitor.Add(customsAgent1);
            monitor.Add(customsAgent2);
            monitor.Add(customsAgent3);
            monitor.Start();

            var airplane = new Airplane(flight: "DC132", passengersCount: 20, publisher: dispatcher);

            var unload = new Task(airplane.Unload);
            unload.Start();
            unload.Wait();
            System.Console.ReadLine();


        }
    }
}
