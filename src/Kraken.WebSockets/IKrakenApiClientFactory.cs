namespace Kraken.WebSockets
{
    /// <summary>
    /// This interface describes the factory instance responsible for creating new 
    /// <see cref="IKrakenApiClient"/> instances
    /// </summary>
    public interface IKrakenApiClientFactory
    {
        /// <summary>
        /// Creates a new <see cref="IKrakenApiClient"/> instance connected to the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        IKrakenApiClient Create(string uri);
    }
}