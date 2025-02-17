namespace RIBAssessment.SarishaNaidoo.Common
{
    public class ApiResponse<T>
    {
        /// <summary>
        /// HTTP status code (e.g., 200, 400, 500, etc.)
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Indicates if the request was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// The data returned from the API, if any.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Error message to display if the request fails.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Creates a successful response.
        /// </summary>
        public static ApiResponse<T> Success(int statusCode, T data)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                IsSuccess = true,
                Data = data,
                ErrorMessage = null
            };
        }

        /// <summary>
        /// Creates a failure response.
        /// </summary>
        public static ApiResponse<T> Failure(int statusCode, string errorMessage)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                IsSuccess = false,
                Data = default,
                ErrorMessage = errorMessage
            };
        }
    }
}