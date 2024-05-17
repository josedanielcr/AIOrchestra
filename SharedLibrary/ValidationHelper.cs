using FluentValidation;

namespace SharedLibrary
{
    public static class ValidationHelper
    {
        public static string ValidateRequest<TRequest>(IValidator<TRequest> validator, TRequest request)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return string.Join(", ", validationResult.Errors);
            }
            return null!;
        }
    }
}
