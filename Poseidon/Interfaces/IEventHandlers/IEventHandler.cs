namespace Poseidon.Interfaces.IEventHandlers
{
    public interface IEventHandler<TEvent>
    {
        Task HandleAsync(TEvent eventMessage);
    }
}
