﻿using BasePoint.Core.Cqrs.Dapper.EntityCommands;
using BasePoint.Core.Cqrs.Dapper.Extensions;
using BasePoint.Core.Cqrs.Dapper.Tests.Domain.Entities;
using BasePoint.Core.Cqrs.Dapper.Tests.TableDefinitions;
using Dapper;
using System.Data;

namespace BasePoint.Core.Cqrs.Dapper.Tests.Domain.Cqrs.CommandProviders.Commands
{
    public class DeleteDapperTestEntityCommand : DapperCommand<DapperTestEntity>
    {
        public DeleteDapperTestEntityCommand(
            IDbConnection connection,
            DapperTestEntity affectedEntity)
            : base(connection, affectedEntity)
        {
            AddTypeMapping(nameof(DapperTestEntity), DapperTestEntityTableDefinition.TableDefinition);
        }

        public override IList<CommandDefinition> CreateCommandDefinitions(DapperTestEntity entity)
        {
            var commandDefinitions = new List<CommandDefinition>();

            var deleteQuery = "Delete From EntityTestTable Where Id = @Id";

            var parameters = new DynamicParameters();

            parameters.AddNullable(@"Id", entity);

            var deleteCommandDefinition = new CommandDefinition(deleteQuery, parameters);

            commandDefinitions.Add(deleteCommandDefinition);

            return commandDefinitions;
        }
    }
}