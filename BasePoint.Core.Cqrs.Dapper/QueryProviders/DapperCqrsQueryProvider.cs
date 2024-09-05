using BasePoint.Core.Application.Cqrs.QueryProviders;
using System.Data;

namespace BasePoint.Core.Cqrs.Dapper.QueryProviders
{
    public abstract class DapperCqrsQueryProvider<ResultOutput> : ICqrsQueryProvider<ResultOutput>
    {
        protected IDbConnection _connection;

        public DapperCqrsQueryProvider(IDbConnection connection)
        {
            _connection = connection;
        }

        public abstract Task<ResultOutput> GetById(Guid id);
    }
}