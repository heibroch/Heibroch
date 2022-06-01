namespace Heibroch.Infrastructure.Interfaces.MessageBus
{
    public interface IInternalMessage
    {
        public bool LogPublish { get; set; }
    }
}
