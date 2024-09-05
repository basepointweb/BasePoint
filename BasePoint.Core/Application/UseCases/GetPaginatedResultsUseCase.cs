using BasePoint.Core.Application.Cqrs.QueryProviders;
using BasePoint.Core.Application.Dtos.Input;
using BasePoint.Core.Application.Dtos.Output;
using BasePoint.Core.Shared;
using BasePoint.Core.Exceptions;
using BasePoint.Core.Extensions;
using FluentValidation;

namespace BasePoint.Core.Application.UseCases
{
    public class GetPaginatedResultsUseCase<QueryProvider, Output> : BaseUseCase<GetPaginatedResultsInput, PaginatedOutput<Output>>
        where QueryProvider : IListItemOutputCqrsQueryProvider<Output>
    {
        private readonly QueryProvider _queryProvider;
        private readonly IValidator<GetPaginatedResultsInput> _inputValidator;
        public GetPaginatedResultsUseCase(
            IValidator<GetPaginatedResultsInput> inputValidator,
            QueryProvider queryProvider)
            : base()
        {
            _queryProvider = queryProvider;
            _inputValidator = inputValidator;
        }

        public override async Task<UseCaseOutput<PaginatedOutput<Output>>> InternalExecuteAsync(GetPaginatedResultsInput input)
        {
            _inputValidator.ValidateAndThrow(input);

            var resultsInPage = await _queryProvider.GetPaginatedResults(input.Filters, input.PageNumber, input.ItemsPerPage);

            var resultsCount = await _queryProvider.Count(input.Filters);

            var maxPage = (int)(resultsCount / input.ItemsPerPage);
            var remainder = (resultsCount % input.ItemsPerPage);

            maxPage += ((remainder > Constants.QuantityZeroItems) ? Constants.FirstIndex : Constants.ZeroBasedFirstIndex);

            if ((resultsCount > Constants.QuantityZeroItems) && (input.PageNumber > maxPage))
                throw new InvalidInputException(Constants.ErrorMessages.PageNumberMustBeLessOrEqualMaxPage.Format(maxPage));

            return CreateSuccessOutput(new PaginatedOutput<Output>(input.PageNumber, maxPage, resultsCount, resultsInPage));
        }
    }
}