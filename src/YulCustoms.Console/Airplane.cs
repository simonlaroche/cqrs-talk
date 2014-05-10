using System;

namespace YulCustoms.Console
{
    public class Airplane
    {
        private int passengersCount;
        private readonly IHandle<CustomsDeclaration> handlesDeclaration;
        private Random random = new Random(DateTime.Now.Millisecond);
        private string flight;

        public Airplane(string flight, int passengersCount, IHandle<CustomsDeclaration> handlesDeclaration)
        {
            this.passengersCount = passengersCount;
            this.handlesDeclaration = handlesDeclaration;
            this.flight = flight;
        }

        public void Unload()
        {
            int i = 0;
            while (i < passengersCount)
            {
                var declaration = new CustomsDeclaration();
                var citizenship = random.NextDouble() < 0.7 ? "CDN" : "US";
                declaration.Citenzenship = citizenship;
                declaration.Resident = random.NextDouble() < 0.8;
                declaration.Id = Guid.NewGuid();
                declaration.Flight = this.flight;
                declaration.Name = "name " + i;
                i++;

                handlesDeclaration.Handle(declaration);
            }
        }
    }

}