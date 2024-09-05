using BasePoint.Core.Application.UseCases;
using BasePoint.Core.Tests.Application.SampleUseCasesDtos;
using BasePoint.Core.Tests.Domain.Repositories.SampleRepository;
using FluentValidation;

namespace BasePoint.Core.Tests.Application.UseCases.SampleUseCases
{
    public class SampleUseCase : BaseUseCase<SampleChildUseCaseInput, bool>
    {
        private readonly ISampleRepository _sampleRepository;
        private readonly IValidator<SampleChildUseCaseInput> _validator;

        public SampleUseCase(
            IValidator<SampleChildUseCaseInput> validator,
            ISampleRepository sampleRepository) : base()
        {
            _sampleRepository = sampleRepository;
            _validator = validator;
        }

        public override async Task<UseCaseOutput<bool>> InternalExecuteAsync(SampleChildUseCaseInput input)
        {
            _validator.ValidateAndThrow(input);

            var entity = await _sampleRepository.GetById(input.SampleId);

            entity.SetMonthlySalary(input.MonthlySalary);

            var sucess = entity is not null;

            return CreateSuccessOutput(sucess);
        }
    }
}