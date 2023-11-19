namespace ChatApp.Application.Command
{
    public interface ICommandResult
    {
        bool Success { get; }
        DateTime Executed { get; }
    }
}
