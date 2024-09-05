using BasePoint.Core.Domain.Entities;

namespace BasePoint.Core.Cqrs.Dapper.Tests.Domain.Entities
{
    public class DapperTestEntity2 : BaseEntity
    {
        public DapperTestEntity ChildEntity { get; set; }
    }
}