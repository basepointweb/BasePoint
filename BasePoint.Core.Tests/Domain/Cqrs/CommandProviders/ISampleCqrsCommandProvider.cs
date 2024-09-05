using BasePoint.Core.Domain.Cqrs.CommandProviders;
using BasePoint.Core.Tests.Domain.Entities;

namespace BasePoint.Core.Tests.Domain.Cqrs.CommandProviders
{
    public interface ISampleCqrsCommandProvider : ICqrsCommandProvider<SampleEntity>
    {
        Task<SampleEntity> GetBySampleName(string sampleName);
    }
}