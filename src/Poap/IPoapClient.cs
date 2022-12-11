namespace Poap;

public interface IPoapClient
{
    /// <summary>
    /// For security purposes the access token expires after 24 hours. This means you will need to generate a new access token every rolling 24 hour period.
    /// Note: Generating more than 4 access tokens per hour will lead to a ban.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Authenticated?> AuthenticateAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// This endpoint returns a list of POAPs held by an address and the event details, token ID, chain, and owner address for each.
    /// A 400 error is returned if the address does not hold any POAPs.
    /// </summary>
    /// <param name="address">The Ethereum address, ENS, or email.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<Event>> ScanAddressAsync(string address, CancellationToken cancellationToken = default);

    /// <summary>
    /// For the specified token ID, this endpoint returns the event details, token ID, owner's address, layer the POAP is currently on, and the POAP supply for that event.
    /// </summary>
    /// <param name="tokenId">The unique POAP token ID.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Token?> GetTokenAsync(string tokenId, CancellationToken cancellationToken = default);

    /// <summary>
    /// This endpoint returns a paginated list of events in descending order by start date.
    /// </summary>
    /// <param name="offset">The offset to paginate the results.</param>
    /// <param name="limit">The amount of results per response (default = 10, max = 1000).</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PaginatedItems<Event>?> GetEventsPaginatedAsync(long offset = 0, int limit = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// For the specified event ID, this endpoint returns paginated info on the token holders including the token ID, POAP transfer count, and the owner's information like address, amount of POAPs owned, and ENS.
    /// </summary>
    /// <param name="eventId">The numeric ID of the event.</param>
    /// <param name="offset">The offset to paginate the results.</param>
    /// <param name="limit">The amount of results per response (default = 10, max = 300).</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PaginatedTokens<Poap>?> GetEventPoapsAsync(string eventId, long offset = 0, int limit = 10, CancellationToken cancellationToken = default);
}
