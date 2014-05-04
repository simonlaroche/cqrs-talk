using System;

namespace YulCustoms
{
    public class InterviewTimedOut : TravellerBase
    {

    }

    public class WakeMeIn : IMessage
    {
        public WakeMeIn()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid CausationId { get; set; }
        public Guid CorrelationId { get; set; }
        public IMessage Message { get; set; }

        /// <summary>
        /// Time to live in seconds
        /// </summary>
        public int TTL { get; set; }
    }

    public class TravellerArrived: TravellerBase
    {
    }

    public class TravellerInterviewed:TravellerBase
    {
    }

    public class TravellerBase : IMessage
    {
        private Guid id= Guid.NewGuid();

        public Guid Id
        {
            get { return id; }
        }

        public Guid CorrelationId { get; set; }
        public CustomsDeclaration Declaration { get; set; }
    }

    public class InterviewTraveller : TravellerBase
    {
    }

    public class RouteTraveller : TravellerBase
    {

    }

    public class TravellerCustomsProcessCompleted : TravellerBase
    {
    }
}