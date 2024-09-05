using BasePoint.Core.Domain.Repositories;
using BasePoint.Core.Tests.Domain.Cqrs.CommandProviders;
using BasePoint.Core.Tests.Domain.Entities;

namespace BasePoint.Core.Tests.Domain.Repositories.SampleRepository
{
    public class SampleRepository : Repository<SampleEntity>, ISampleRepository
    {
        private readonly ISampleCqrsCommandProvider _commandProvider;

        public SampleRepository(ISampleCqrsCommandProvider commandProvider) : base(commandProvider)
        {
            _commandProvider = commandProvider;
        }

        public async Task<SampleEntity> GetBySampleName(string sampleName)
        {
            return HandleAfterGetFromCommandProvider(await _commandProvider.GetBySampleName(sampleName));
        }
    }
}