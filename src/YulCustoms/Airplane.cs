using System;
using System.Security.Cryptography;
using YulCustoms.Messaging;

namespace YulCustoms
{
    public class Airplane
    {
        private int passengersCount;
        private readonly Dispatcher publisher;
        private Random random = new Random(DateTime.Now.Millisecond);
        private string flight;

        public Airplane(string flight, int passengersCount, Dispatcher publisher)
        {
            this.passengersCount = passengersCount;
            this.publisher = publisher;
            this.flight = flight;
        }

        public void Unload()
        {
            int i= 0;
            while (i < passengersCount)
            {
                var declaration = new CustomsDeclaration();
                var citizenship = random.NextDouble() < 0.7 ? "CDN" : "US";
                declaration.Citenzenship = citizenship;
                declaration.Resident = random.NextDouble() < 0.9;
                declaration.Id = Guid.NewGuid();
                declaration.Flight = this.flight;
                declaration.Name = "name " + i;
                i++;

                var travellerArrived = new TravellerArrived {Declaration = declaration};

                publisher.Publish(travellerArrived);
            }
        }
    }
}