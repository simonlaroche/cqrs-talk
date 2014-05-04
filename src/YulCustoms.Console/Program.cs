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
            var residentHandler = new NullHandler<CustomsDeclaration>("resident");
            var visitorHandler = new NullHandler<CustomsDeclaration>("visitor");

            var resindencyRouter = new QueuedHandler<CustomsDeclaration>(new ResidencyRouter(residentHandler, visitorHandler));
            resindencyRouter.Start();

            var airplane = new Airplane("DC132", 666, resindencyRouter);

            var unload = new Task(() => airplane.Unload());
            unload.Start();
            unload.Wait();
            System.Console.ReadLine();
        }
    }
}
