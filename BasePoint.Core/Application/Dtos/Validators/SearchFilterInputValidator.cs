using BasePoint.Core.Application.Dtos.Input;
using BasePoint.Core.Shared;
using BasePoint.Core.Extensions;
using FluentValidation;

namespace BasePoint.Core.Application.Dtos.Validators
{
    public class SearchFilterInputValidator : AbstractValidator<SearchFilterInput>
    {
        public SearchFilterInputValidator()
        {
            RuleFor(x => x.FilterType)
                .IsInEnum()
                .WithMessage(Constants.ErrorMessages.PropertyIsInvalid.Format(nameof(SearchFilterInput.FilterType)));

            RuleFor(x => x.FilterProperty)
                .NotEmpty()
                .NotNull()
                .WithMessage(Constants.ErrorMessages.PropertyIsRequired.Format(nameof(SearchFilterInput.FilterProperty)));
        }
    }
}