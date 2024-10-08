﻿using BasePoint.Core.Shared;
using BasePoint.Core.Tests.Application.Dtos.Builders;
using BasePoint.Core.Tests.Application.SampleUseCasesDtos;
using BasePoint.Core.Tests.Application.UseCases.SampleUseCases;
using BasePoint.Core.Tests.Common;
using BasePoint.Core.Tests.Domain.Repositories.SampleRepository;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace BasePoint.Core.Tests.Application.UseCases
{
    public class BaseUseCaseTests
    {
        private readonly SampleUseCase _useCase;
        private readonly Mock<ISampleRepository> _sampleRepository;
        private readonly Mock<IValidator<SampleChildUseCaseInput>> _validator;

        public BaseUseCaseTests()
        {
            _sampleRepository = new Mock<ISampleRepository>();
            _validator = new Mock<IValidator<SampleChildUseCaseInput>>();
            _useCase = new SampleUseCase(
                 _validator.Object,
                _sampleRepository.Object);
        }

        [Fact]
        public async Task Execute_Always_ReturnsSuccess()
        {
            // Arrange
            var input = new SampleChildUseCaseInputBuilder()
                .WithSampleName("Sample Name Test")
                .Build();

            var sampleEntity = new SampleEntityBuilder().Build();

            _sampleRepository.Setup(s => s.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(sampleEntity);

            // Act
            var output = await _useCase.ExecuteAsync(input);

            // Assert
            output.HasErros.Should().BeFalse();
            _sampleRepository.Verify(x => x.GetById(input.SampleId), Times.Once);
        }

        [Fact]
        public async Task Execute_SampleRepositoryThrowsException_ReturnError()
        {
            var input = new SampleChildUseCaseInputBuilder()
                .Build();

            _sampleRepository.Setup(s => s.GetById(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Error test"));

            var output = await _useCase.ExecuteAsync(input);

            output.HasErros.Should().BeTrue();
            output.Errors.Should().ContainEquivalentOf(new ErrorMessage("000;Error test"));
        }

        [Fact]
        public async Task Execute_ValidatorThowsException_ReturnError()
        {
            var input = new SampleChildUseCaseInputBuilder()
                .Build();

            _validator.Setup(x => x.Validate(It.IsAny<ValidationContext<SampleChildUseCaseInput>>()))
                .Throws(new FluentValidation.ValidationException("000;Validation Error"));

            var output = await _useCase.ExecuteAsync(input);

            output.HasErros.Should().BeTrue();
            output.Errors.Should().ContainEquivalentOf(new ErrorMessage("000;Validation Error"));
        }

        [Fact]
        public async Task Execute_ValidatorThowsExceptionAndHasErrors_ReturnError()
        {
            var input = new SampleChildUseCaseInputBuilder()
                .Build();

            _validator.Setup(x => x.Validate(It.IsAny<ValidationContext<SampleChildUseCaseInput>>()))
                .Throws(new FluentValidation.ValidationException([new ValidationFailure("SampleProperty", "SampleProperty is invalid.")]));

            var output = await _useCase.ExecuteAsync(input);

            output.HasErros.Should().BeTrue();
            output.Errors.Should().ContainEquivalentOf(new ErrorMessage("000;SampleProperty is invalid."));
        }

        [Fact]
        public async Task Execute_EntityThowsException_ReturnError()
        {
            var input = new SampleChildUseCaseInputBuilder()
                .WithMonthlySalary(0)
                .Build();

            var sampleEntity = new SampleEntityBuilder().Build();

            _sampleRepository.Setup(s => s.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(sampleEntity);

            var output = await _useCase.ExecuteAsync(input);

            output.HasErros.Should().BeTrue();
            output.Errors.Should().ContainEquivalentOf(new ErrorMessage(CommonTestContants.EntitySalaryMustBeGreaterThanZero));
        }
    }
}