namespace ChatApp.Infrastructure.Command
{
    public interface ICommand
    {
    }

    public interface ICommand<TResult> : ICommand where TResult : ICommandResult
    {
    }
}
