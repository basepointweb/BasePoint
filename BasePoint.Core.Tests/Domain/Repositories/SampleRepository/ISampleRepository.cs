using BasePoint.Core.Domain.Repositories.Interfaces;
using BasePoint.Core.Tests.Domain.Entities;

namespace BasePoint.Core.Tests.Domain.Repositories.SampleRepository
{
    public interface ISampleRepository : IRepository<SampleEntity>
    {
        Task<SampleEntity> GetBySampleName(string sampleName);
    }
}