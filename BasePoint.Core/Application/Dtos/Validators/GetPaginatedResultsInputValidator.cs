using BasePoint.Core.Application.Dtos.Input;
using BasePoint.Core.Shared;
using FluentValidation;

namespace BasePoint.Core.Application.Dtos.Validators
{
    public class GetPaginatedResultsInputValidator : AbstractValidator<GetPaginatedResultsInput>
    {
        public GetPaginatedResultsInputValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage(Constants.ErrorMessages.PageNumberMustBeGreaterThanOrEqualToOne);

            RuleFor(x => x.ItemsPerPage)
                .GreaterThanOrEqualTo(1)
                .WithMessage(Constants.ErrorMessages.ItemsPerPageMustBeGreaterThanOrEqualToOne);

            RuleForEach(x => x.Filters)
                .SetValidator(new SearchFilterInputValidator());
        }
    }
}