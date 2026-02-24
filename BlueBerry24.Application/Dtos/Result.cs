using BlueBerry24.Application.Constants;

namespace BlueBerry24.Application.Dtos
{
    /// <summary>
    /// Represents the result of an operation, containing the status, data, or error information.
    /// This follows the Result Pattern to avoid using exceptions for business logic control flow.
    /// </summary>
    /// <typeparam name="T">The type of the value returned on success.</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// Returns true if the status is Success or NoContent.
        /// </summary>
        public bool IsSuccess => Status == ResultStatus.Success || Status == ResultStatus.NoContent;

        /// <summary>
        /// Gets the error message associated with a failed operation.
        /// </summary>
        public string? Error { get; }

        /// <summary>
        /// Gets the specific status of the operation (e.g., NotFound, Conflict, Success).
        /// </summary>
        public ResultStatus Status { get; set; }

        /// <summary>
        /// Gets the data returned by a successful operation.
        /// </summary>
        public T? Value { get; }

        private Result(ResultStatus status, T? value, string? error)
        {
            Status = status;
            Value = value;
            Error = error;
        }

        /// <summary>
        /// Returns a success result containing the specified value. (HTTP 200)
        /// </summary>
        public static Result<T> Success(T value)
            => new(ResultStatus.Success, value, null);

        /// <summary>
        /// Returns a success result with no content. (HTTP 204)
        /// </summary>
        public static Result<T> NoContent()
            => new(ResultStatus.NoContent, default, null);

        /// <summary>
        /// Indicates that the input data failed business or syntax validation rules. (HTTP 400)
        /// </summary>
        public static Result<T> ValidationError(string error)
            => new(ResultStatus.ValidationError, default, error);

        /// <summary>
        /// Indicates that the user is not authenticated. (HTTP 401)
        /// </summary>
        public static Result<T> Unauthorized(string error = "Unauthorized access.")
            => new(ResultStatus.Unauthorized, default, error);

        /// <summary>
        /// Indicates that the user is authenticated but does not have permission for this resource. (HTTP 403)
        /// </summary>
        public static Result<T> Forbidden(string error = "Access denied.")
            => new(ResultStatus.Forbidden, default, error);

        /// <summary>
        /// Indicates that the requested resource could not be found. (HTTP 404)
        /// </summary>
        public static Result<T> NotFound(string error = "Resource not found.")
            => new(ResultStatus.NotFound, default, error);

        /// <summary>
        /// Indicates a state conflict, such as trying to create a record that already exists. (HTTP 409)
        /// </summary>
        public static Result<T> Conflict(string error)
            => new(ResultStatus.Conflict, default, error);

        /// <summary>
        /// Indicates that a secondary step (like email/SMS verification) is required to proceed.
        /// </summary>
        public static Result<T> VerificationRequired(string message)
            => new(ResultStatus.VerificationRequired, default, message);

        /// <summary>
        /// Represents a general application failure or unhandled business logic error. (HTTP 500)
        /// </summary>
        public static Result<T> Failure(string error)
            => new(ResultStatus.Failure, default, error);
    }
}