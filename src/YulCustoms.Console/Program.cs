using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YulCustoms.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var endOfProcess    = new QueuedHandler<CustomsDeclaration> (new NullHandler<CustomsDeclaration>("resident"), "end-of-process");

            var customsAgent1 = new QueuedHandler<CustomsDeclaration>(new CustumsAgent(endOfProcess, "joe"), "agent-joe");
            var customsAgent2 = new QueuedHandler<CustomsDeclaration>(new CustumsAgent(endOfProcess, "jack"), "agent-jack");
            var customsAgent3 = new QueuedHandler<CustomsDeclaration>(new CustumsAgent(endOfProcess, "averell"),"agent-averell");

            var customsAgentDispatcher =
                new RoundRobinDispatcher<CustomsDeclaration>(new[] {customsAgent1, customsAgent2, customsAgent3});

            var visitorHandler = new NullHandler<CustomsDeclaration>("visitor");


            var residencyRouter = new QueuedHandler<CustomsDeclaration>(new ResidencyRouter(customsAgentDispatcher, visitorHandler), "residency-router");
            
            residencyRouter.Start();
            customsAgent1.Start();
            customsAgent2.Start();
            customsAgent3.Start();

            var monitor = new Monitor();
            monitor.Add(endOfProcess);
            monitor.Add(customsAgent1);
            monitor.Add(customsAgent2);
            monitor.Add(customsAgent3);
            monitor.Add(residencyRouter);

            monitor.Start();

            var airplane = new Airplane(flight: "DC132", passengersCount: 666,handlesDeclaration: residencyRouter);

            var unload = new Task(airplane.Unload);
            unload.Start();
            unload.Wait();
            System.Console.ReadLine();


        }
    }
}
