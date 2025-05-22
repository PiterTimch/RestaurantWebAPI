using FluentResults;

namespace RestaurantWebAPI.Validators.Helpers
{
    public static class ResultExtensions
    {
        public static IEnumerable<object> ToFieldErrors(this IEnumerable<IError> errors)
        {
            return errors.Select(e => new
            {
                field = e.Metadata != null && e.Metadata.ContainsKey("Field") ? e.Metadata["Field"].ToString() : "error",
                error = e.Message
            });
        }
    }
}
