namespace ChatApp.Infrastructure.Query
{
    public interface IQueryHandler
    {
    }

    public interface IQueryHandler<TQuery, TQResult> : IQueryHandler
        where TQuery : IQuery<TQResult>
    {
        Task<TQResult> HandleAsync();
    }
}
