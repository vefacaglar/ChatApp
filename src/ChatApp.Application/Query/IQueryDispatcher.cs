namespace ChatApp.Infrastructure.Query
{
    public interface IQueryDispatcher
    {
        Task<TModel> ExecuteAsync<TModel>(IQuery<TModel> query);
    }
}
