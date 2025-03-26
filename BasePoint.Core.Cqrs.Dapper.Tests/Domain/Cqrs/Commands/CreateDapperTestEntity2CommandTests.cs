using BasePoint.Core.Cqrs.Dapper.Tests.Domain.Entities;
using BasePoint.Core.Domain.Enumerators;
using FluentAssertions;
using Moq;
using System.Data;
using Xunit;

namespace BasePoint.Core.Cqrs.Dapper.Tests.Domain.Cqrs.Commands
{
    public class CreateDapperTestEntity2CommandTests
    {
        private readonly CreateDapperTestEntity2Command _command;
        private readonly Mock<IDbConnection> _connection;
        private readonly Mock<DapperTestEntity2> _entity2;

        public CreateDapperTestEntity2CommandTests()
        {
            _connection = new Mock<IDbConnection>();
            _entity2 = new Mock<DapperTestEntity2>();

            _command = new CreateDapperTestEntity2Command(_connection.Object, _entity2.Object);
        }


        [Fact]
        public void CreateAnInsertCommandFromEntityProperties_WhenHasAEntityInAProperty_ReturnsCreateCommandDefinition()
        {
            var entityId = Guid.NewGuid();
            const string expectedChildDeleteSql = "Delete From\nEntityTestTable2\nWhere\nChildEntity_Id = @ChildEntity_Id;";

            var entityInsertedProperties = new Dictionary<string, object>()
            {
                { nameof(DapperTestEntity2.ChildEntity), new DapperTestEntity() { Code = "000001", Name = "Test" } }
            };

            _entity2.Setup(x => x.GetPropertiesToPersist()).Returns(entityInsertedProperties);

            _entity2.Setup(x => x.State).Returns(EntityState.New);
            _entity2.Setup(x => x.GetPropertiesToPersist()).Returns(entityInsertedProperties);


            _entity2.Object.ChildEntity = new DapperTestEntity();

            var commandDefinition = _command.CreateAnInsertCommandFromEntityProperties(
                _entity2.Object);

            commandDefinition.CommandText.Should().BeEquivalentTo(expectedChildDeleteSql);
        }
    }
}
