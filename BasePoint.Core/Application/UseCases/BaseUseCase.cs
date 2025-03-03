using BasePoint.Core.Exceptions;
using BasePoint.Core.Extensions;
using BasePoint.Core.Shared;

namespace BasePoint.Core.Application.UseCases
{
    public abstract class BaseUseCase<Input, Output>
    {
        public BaseUseCase()
        {
        }

        public UseCaseOutput<Output> CreateSuccessOutput(Output output)
        {
            return new UseCaseOutput<Output>(output);
        }

        public UseCaseOutput<Output> CreateOutputWithErrors(IList<ErrorMessage> errors)
        {
            return new UseCaseOutput<Output>(errors);
        }

        public abstract Task<UseCaseOutput<Output>> InternalExecuteAsync(Input input);

        public virtual async Task<UseCaseOutput<Output>> ExecuteAsync(Input input)
        {
            UseCaseOutput<Output> output;
            try
            {
                output = await InternalExecuteAsync(input);
            }
            catch (ValidationException ex)
            {
                output = CreateOutputWithErrors(ex.Errors);
            }
            catch (FluentValidation.ValidationException ex)
            {
                if (ex.Errors.Any())
                    output = CreateOutputWithErrors(ex.Errors.SafeSelect(e => new ErrorMessage(e.ErrorMessage)).ToList());
                else
                    output = CreateOutputWithErrors([new($"Error message: {ex.Message}\nCall Stack: {ex.StackTrace}")]);

            }
            catch (BaseException ex)
            {
                output = CreateOutputWithErrors(ex.Errors);
            }
            catch (Exception ex)
            {
                output = CreateOutputWithErrors([new($"Error message: {ex.Message}\nCall Stack: {ex.StackTrace}")]);
            }

            return output;
        }
    }
}