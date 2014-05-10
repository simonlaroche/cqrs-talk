using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YulCustoms.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //Bootstrap process
            var residentHandler = new NullHandler("Resident Handler");
            var visitorHandler = new NullHandler("Resident Handler");

            var residencyRouter =
                new QueuedHandler<CustomsDeclaration>(new ResidencyRouter(residentHandler, visitorHandler));

            residencyRouter.Start();
            
            var airplane = new Airplane("DC-132", 500, residencyRouter);
            airplane.Unload();


        }
    }

    internal class NullHandler:IHandle<CustomsDeclaration>
    {
        private readonly string name;

        public NullHandler(string name)
        {
            this.name = name;
        }

        public void Handle(CustomsDeclaration message)
        {
            System.Console.WriteLine("{0} passenger {1} on flight {2}", name, message.Name, message.Flight);
            Thread.Sleep(1000);
        }
    }
}
