namespace ChatApp.Infrastructure.Command
{
    public interface ICommandResult
    {
        bool Success { get; }
        DateTime Executed { get; }
    }
}
