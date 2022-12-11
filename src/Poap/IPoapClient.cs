namespace Poap;

public interface IPoapClient
{
    Task<Authenticated?> AuthenticateAsync(CancellationToken cancellationToken = default);

    Task<Token?> GetTokenAsync(string tokenId, CancellationToken cancellationToken = default);

    Task<PaginatedItems<Event>?> GetEventsPaginatedAsync(long offset = 0, int limit = 10, CancellationToken cancellationToken = default);

    Task<PaginatedTokens<Poap>?> GetEventPoapsAsync(string eventId, long offset = 0, int limit = 10, CancellationToken cancellationToken = default);
}
