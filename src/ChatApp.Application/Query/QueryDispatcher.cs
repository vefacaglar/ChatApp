﻿using Autofac;

namespace ChatApp.Infrastructure.Query
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IComponentContext _componentContext;

        public QueryDispatcher(
            IComponentContext componentContext
            )
        {
            _componentContext = componentContext;
        }

        public Task<TModel> ExecuteAsync<TModel>(IQuery<TModel> query)
        {
            var queryHandlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TModel));

            var handler = _componentContext.Resolve(queryHandlerType);

            return (Task<TModel>)queryHandlerType
                .GetMethod("HandleAsync").Invoke(handler, new object[] { query });
        }
    }
}
