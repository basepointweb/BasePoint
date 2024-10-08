﻿using BasePoint.Core.Application.Cqrs.QueryProviders;
using BasePoint.Core.Application.Dtos.Input;
using BasePoint.Core.Application.UseCases;
using BasePoint.Core.Shared;
using BasePoint.Core.Domain.Enumerators;
using BasePoint.Core.Extensions;
using BasePoint.Core.Tests.Application.Dtos.Builders;
using BasePoint.Core.Tests.Application.SampleUseCasesDtos;
using FluentAssertions;
using FluentValidation;
using Moq;
using Xunit;

namespace BasePoint.Core.Tests.Application.UseCases
{
    public class GetPaginatedResultsUseCaseTests
    {
        private readonly GetPaginatedResultsUseCase<IListItemOutputCqrsQueryProvider<SampleChildUseCaseOutput>, SampleChildUseCaseOutput> _useCase;
        private readonly Mock<IValidator<GetPaginatedResultsInput>> _inputValidator;
        private readonly Mock<IListItemOutputCqrsQueryProvider<SampleChildUseCaseOutput>> _queryProvider;

        public GetPaginatedResultsUseCaseTests()
        {
            _inputValidator = new Mock<IValidator<GetPaginatedResultsInput>>();
            _queryProvider = new Mock<IListItemOutputCqrsQueryProvider<SampleChildUseCaseOutput>>();

            _useCase = new GetPaginatedResultsUseCase<IListItemOutputCqrsQueryProvider<SampleChildUseCaseOutput>, SampleChildUseCaseOutput>(
                _inputValidator.Object,
                _queryProvider.Object);
        }

        [Fact]
        public async Task ExecuteAsync_InputIsValid_ReturnsResults()
        {
            // Arrange
            var filter = new SearchFilterInputBuilder()
                .WithFilterProperty("Name")
                .WithFilterType(FilterType.Equals)
                .WithFilterValue("SampleNameValue")
                .Build();

            var input = new GetPaginatedResultsInputBuilder()
                .WithFilters([filter])
                .WithPageNumber(1)
                .WithItemsPerPage(10)
                .Build();

            var resultOutPut = new SampleChildUseCaseOutputBuilder().Build();

            var results = new List<SampleChildUseCaseOutput>() { resultOutPut };

            _queryProvider.Setup(s => s.GetPaginatedResults(input.Filters, input.PageNumber, input.ItemsPerPage))
                .ReturnsAsync(results);

            _queryProvider.Setup(x => x.Count(input.Filters))
                .ReturnsAsync(results.Count);

            // Act
            var output = await _useCase.ExecuteAsync(input);

            // Assert
            output.HasErros.Should().BeFalse();
            output.OutputObject.ActualPage.Should().Be(input.PageNumber);
            output.OutputObject.MaxPage.Should().Be(1);
            output.OutputObject.ResultsInPage.Should().Contain(resultOutPut);
            output.OutputObject.TotalResultsCount.Should().Be(1);
            _queryProvider.Verify(x => x.GetPaginatedResults(It.IsAny<IList<SearchFilterInput>>(), input.PageNumber, input.ItemsPerPage), Times.Once);
            _queryProvider.Verify(x => x.Count(It.IsAny<IList<SearchFilterInput>>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_QueryProviderReturnsNoItems_ReturnsEmptyResults()
        {
            // Arrange
            var filter = new SearchFilterInputBuilder()
                .WithFilterProperty("Name")
                .WithFilterType(FilterType.Equals)
                .WithFilterValue("SampleNameValue")
                .Build();

            var input = new GetPaginatedResultsInputBuilder()
                .WithFilters([filter])
                .WithPageNumber(1)
                .WithItemsPerPage(10)
                .Build();

            _queryProvider.Setup(s => s.GetPaginatedResults(input.Filters, input.PageNumber, input.ItemsPerPage))
                .ReturnsAsync([]);

            _queryProvider.Setup(x => x.Count(input.Filters))
                .ReturnsAsync(0);

            // Act
            var output = await _useCase.ExecuteAsync(input);

            // Assert
            output.HasErros.Should().BeFalse();
            output.OutputObject.ActualPage.Should().Be(input.PageNumber);
            output.OutputObject.MaxPage.Should().Be(0);
            output.OutputObject.TotalResultsCount.Should().Be(0);
            _queryProvider.Verify(x => x.GetPaginatedResults(It.IsAny<IList<SearchFilterInput>>(), input.PageNumber, input.ItemsPerPage), Times.Once);
            _queryProvider.Verify(x => x.Count(It.IsAny<IList<SearchFilterInput>>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_PagaNumberGreaterThanMaxPage_ReturnsError()
        {
            // Arrange
            var filter = new SearchFilterInputBuilder()
                .WithFilterProperty("Name")
                .WithFilterType(FilterType.Equals)
                .WithFilterValue("SampleNameValue")
                .Build();

            var input = new GetPaginatedResultsInputBuilder()
                .WithFilters([filter])
                .WithPageNumber(2)
                .WithItemsPerPage(10)
                .Build();

            var resultOutPut = new SampleChildUseCaseOutputBuilder().Build();

            var results = new List<SampleChildUseCaseOutput>() { resultOutPut };

            _queryProvider.Setup(s => s.GetPaginatedResults(input.Filters, input.PageNumber, input.ItemsPerPage))
                .ReturnsAsync(results);

            _queryProvider.Setup(x => x.Count(input.Filters))
                .ReturnsAsync(results.Count);

            // Act
            var output = await _useCase.ExecuteAsync(input);

            // Assert
            output.HasErros.Should().BeTrue();
            output.Errors.Should().ContainEquivalentOf(new ErrorMessage(Constants.ErrorMessages.PageNumberMustBeLessOrEqualMaxPage.Format(1)));
            output.OutputObject.Should().BeNull();
            _queryProvider.Verify(x => x.GetPaginatedResults(It.IsAny<IList<SearchFilterInput>>(), input.PageNumber, input.ItemsPerPage), Times.Once);
            _queryProvider.Verify(x => x.Count(It.IsAny<IList<SearchFilterInput>>()), Times.Once);
        }
    }
}