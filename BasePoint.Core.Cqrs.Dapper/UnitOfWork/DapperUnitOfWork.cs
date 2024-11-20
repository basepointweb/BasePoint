using BasePoint.Core.UnitOfWork;
using System.Data;
using System.Transactions;

namespace BasePoint.Core.Cqrs.Dapper.UnitOfWork
{
    public class DapperUnitOfWork : BaseUnitOfWork
    {
        private readonly IDbConnection _connection;
        private TransactionScope _transactionScope;

        public DapperUnitOfWork(IDbConnection connection) : base()
        {
            _connection = connection;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public override async Task<bool> BeforeSaveAsync()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();

            if (_transactionScope is null)
                _transactionScope = new TransactionScope();

            return await base.BeforeSaveAsync();
        }

        public override async Task<bool> AfterSave(bool sucess)
        {
            await base.AfterSave(sucess);

            if (sucess)
                _transactionScope.Complete();
            else
            {
                _transactionScope.Dispose();
                _transactionScope = null;
            }

            return sucess;
        }

        public override async Task AfterRollBackAsync()
        {
            if (_transactionScope is null)
            {
                _transactionScope.Dispose();
                _transactionScope = null;
            }

            await base.AfterRollBackAsync();
        }
    }
}