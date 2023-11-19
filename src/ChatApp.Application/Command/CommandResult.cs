namespace ChatApp.Application.Command
{
    public class CommandResult : ICommandResult
    {
        public bool Success { get; set; }

        public DateTime Executed { get; set; }

        public CommandResult()
        {
            Executed = DateTime.UtcNow;
        }

        public CommandResult(bool success)
        {
            Success = success;
            Executed = DateTime.UtcNow;
        }

        public static TCommandResul CreateFailResult<TCommandResul>() where TCommandResul : CommandResult, new()
        {
            return new TCommandResul();
        }
    }
}
