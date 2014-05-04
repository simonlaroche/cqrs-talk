namespace YulCustoms
{
    using System;

    public interface IHandle<in T> where T: IMessage
    {
        void Handle(T message);
    }

    public interface IMessage
    {
        Guid Id { get; }
        Guid CorrelationId { get; }
    }
}