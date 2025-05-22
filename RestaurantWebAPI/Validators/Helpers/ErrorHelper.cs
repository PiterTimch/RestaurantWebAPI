using FluentResults;

namespace RestaurantWebAPI.Validators.Helpers
{
    public static class ErrorHelper
    {
        public static Error FieldError(string message, string field)
        {
            var error = Result.Fail(message).Errors[0] as Error; 
            if (error != null)
            {
                error = error.WithMetadata("Field", field);
                return error;
            }
            return Result.Fail(message).Errors[0] as Error;
        }

        public static Result AddFieldError(this Result result, string message, string field)
        {
            var error = new Error(message).WithMetadata("Field", field);
            result.WithError(error);
            return result;
        }

        public static Result<T> AddFieldError<T>(this Result<T> result, string message, string field)
        {
            var error = new Error(message).WithMetadata("Field", field);
            result.WithError(error);
            return result;
        }
    }
}
