using FluentValidation;

namespace AIOrchestra.APIGateway.Helpers
{
    public static class Validation
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
