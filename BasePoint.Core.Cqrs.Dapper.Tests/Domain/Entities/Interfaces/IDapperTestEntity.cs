using BasePoint.Core.Domain.Entities;
using BasePoint.Core.Domain.Entities.Interfaces;

namespace BasePoint.Core.Cqrs.Dapper.Tests.Domain.Entities.Interfaces
{
    public interface IDapperTestEntity : IBaseEntity
    {
        string Code { get; set; }
        string Name { get; set; }

        EntityList<DapperChildEntityTest> Childs { get; set; }
    }
}