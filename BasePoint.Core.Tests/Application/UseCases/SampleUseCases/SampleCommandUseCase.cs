using BasePoint.Core.Application.UseCases;
using BasePoint.Core.Extensions;
using BasePoint.Core.Tests.Application.SampleUseCasesDtos;
using BasePoint.Core.Tests.Common;
using BasePoint.Core.Tests.Domain.Entities;
using BasePoint.Core.Tests.Domain.Repositories.SampleRepository;
using BasePoint.Core.UnitOfWork.Interfaces;

namespace BasePoint.Core.Tests.Application.UseCases.SampleUseCases
{
    public class SampleCommandUseCase : CommandUseCase<SampleChildUseCaseInput, SampleChildUseCaseOutput>
    {
        private readonly ISampleRepository _sampleRepository;
        protected override string SaveChangesErrorMessage => "SampleChildCommandUseCase Error message";

        public SampleCommandUseCase(
            ISampleRepository sampleRepository,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _sampleRepository = sampleRepository;
        }

        public override async Task<UseCaseOutput<SampleChildUseCaseOutput>> InternalExecuteAsync(SampleChildUseCaseInput input)
        {
            await ThrowsInvalidInputIfEntityExistsAsync(
                _sampleRepository.GetBySampleName,
                input.SampleName, CommonTestContants.EntityWithNameAlreadyExists.Format(input.SampleName));

            ThrowsResourceNotFoundIfEntityDoesNotExists(
                _sampleRepository.GetById,
                input.SampleLookUpId, CommonTestContants.EntityWithIdDoesNotExists.Format(input.SampleLookUpId), out var lookupEntity);

            var entity = new SampleEntity()
            {
                SampleName = input.SampleName + lookupEntity.SampleName
            };

            _sampleRepository.Persist(entity, UnitOfWork);

            await SaveChangesAsync();

            return CreateSuccessOutput(new SampleChildUseCaseOutput()
            {
                SampleId = entity.Id,
                SampleName = entity.SampleName,
            });
        }
    }
}