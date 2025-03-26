using BasePoint.Core.Cqrs.Dapper.EntityCommands;
using BasePoint.Core.Cqrs.Dapper.Tests.Domain.Entities;
using BasePoint.Core.Cqrs.Dapper.Tests.TableDefinitions;
using System.Data;

namespace BasePoint.Core.Cqrs.Dapper.Tests.Domain.Cqrs.Commands
{
    public class CreateDapperTestEntity2Command : DapperCommand<DapperTestEntity2>
    {
        public CreateDapperTestEntity2Command(IDbConnection connection, DapperTestEntity2 affectedEntity) : base(connection, affectedEntity)
        {
            AddTypeMapping(nameof(DapperTestEntity2), DapperTestEntity2TableDefinition.TableDefinition);
        }
    }
}
