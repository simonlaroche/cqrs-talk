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
            var endOfProcess    = new QueuedHandler<CustomsDeclaration> (new NullHandler<CustomsDeclaration>("resident"));

            var customsAgent1 = new QueuedHandler<CustomsDeclaration>(new CustumsAgent(endOfProcess, "joe"));
            var customsAgent2 = new QueuedHandler<CustomsDeclaration>(new CustumsAgent(endOfProcess, "jack"));
            var customsAgent3 = new QueuedHandler<CustomsDeclaration>(new CustumsAgent(endOfProcess, "averell"));

            var customsAgentDispatcher =
                new RoundRobinDispatcher<CustomsDeclaration>(new[] {customsAgent1, customsAgent2, customsAgent3});

            var visitorHandler = new NullHandler<CustomsDeclaration>("visitor");


            var residencyRouter = new QueuedHandler<CustomsDeclaration>(new ResidencyRouter(customsAgentDispatcher, visitorHandler));
            
            residencyRouter.Start();
            customsAgent1.Start();
            customsAgent2.Start();
            customsAgent3.Start();

            var airplane = new Airplane("DC132", 666, residencyRouter);

            var unload = new Task(() => airplane.Unload());
            unload.Start();
            unload.Wait();
            System.Console.ReadLine();


        }
    }
}
