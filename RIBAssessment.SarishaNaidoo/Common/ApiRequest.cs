namespace RIBAssessment.SarishaNaidoo.Common
{
    /// <summary>
    /// A generic request wrapper that can be used to standardize API requests.
    /// </summary>
    /// <typeparam name="T">The type of the data payload.</typeparam>
    public class ApiRequest<T>
    {
        /// <summary>
        /// Gets or sets the data payload for the request.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the request was created.
        /// </summary>
        public DateTime RequestTime { get; set; } = DateTime.Now;

    }
}
