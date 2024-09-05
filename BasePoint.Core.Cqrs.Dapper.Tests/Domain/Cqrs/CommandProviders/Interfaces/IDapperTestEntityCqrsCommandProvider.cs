using BasePoint.Core.Cqrs.Dapper.Tests.Domain.Entities;
using BasePoint.Core.Domain.Cqrs.CommandProviders;

namespace BasePoint.Core.Cqrs.Dapper.Tests.Domain.Cqrs.CommandProviders.Interfaces
{
    public interface IDapperTestEntityCqrsCommandProvider : ICqrsCommandProvider<DapperTestEntity>
    {
        DapperTestEntity GetByCode(string sampleName);
    }
}