using FluentResults;

namespace RestaurantWebAPI.Validators.Helpers
{
    public static class ResultExtensions
    {
        public static object ToFieldErrors(this IEnumerable<IError> errors)
        {
            var groupedErrors = errors
                .Where(e => e.Metadata != null && e.Metadata.ContainsKey("Field"))
                .GroupBy(e => e.Metadata["Field"]!.ToString())
                .ToDictionary(
                    g => g.Key!,
                    g => g.Select(e => e.Message).ToArray()
                );

            return new
            {
                status = 400,
                isValid = false,
                errors = groupedErrors
            };
        }
    }
}
