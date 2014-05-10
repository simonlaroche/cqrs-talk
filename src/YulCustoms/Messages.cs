using System;

namespace YulCustoms
{

    public class TravellerArrived : TravellerBase
    {
    }

    public class TravellerInterviewed : TravellerBase
    {
    }

    public class TravellerBase : IMessage
    {
        private readonly Guid id = Guid.NewGuid();
        public CustomsDeclaration Declaration { get; set; }

        public Guid Id
        {
            get { return id; }
        }

        public Guid CorrelationId { get; set; }
    }

    public class InterviewTraveller : TravellerBase
    {
    }

    public class TravellerCustomsProcessCompleted : TravellerBase
    {
    }

    public class InterviewTimedOut : TravellerBase
    {
    }

    public class WakeMeIn : IMessage
    {
        public WakeMeIn()
        {
            Id = Guid.NewGuid();
        }

        public Guid CausationId { get; set; }
        public IMessage Message { get; set; }

        /// <summary>
        ///     Time to live in seconds
        /// </summary>
        public int TTL { get; set; }

        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
    }

}