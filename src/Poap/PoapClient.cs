using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Poap;

public class PoapClient : IPoapClient
{
    public const string ApiKeyHeaderName = "x-api-key";

    private readonly HttpClient _client;
    private readonly string _baseUrl = "https://api.poap.tech/";
    private readonly PoapClientOptions _options;

    private readonly SemaphoreSlim _semaphore = new(1);

    public PoapClient(PoapClientOptions options, HttpClient? client = null)
    {
        if (string.IsNullOrEmpty(options.ApiKey))
        {
            throw new ArgumentException(string.Format("{0} must not be null.", nameof(options.ApiKey)), nameof(options));
        }

        _client = client ?? new HttpClient();
        _options = new PoapClientOptions
        {
            AccessToken = options.AccessToken,
            ApiKey = options.ApiKey,
            Audience = options.Audience,
            ClientId = options.ClientId,
            ClientSecret = options.ClientSecret,
        };
    }

    public async Task<Authenticated?> AuthenticateAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(_options.Audience))
        {
            throw new InvalidOperationException(string.Format("{0} must not be null.", nameof(_options.Audience)));
        }

        if (string.IsNullOrEmpty(_options.ClientId))
        {
            throw new InvalidOperationException(string.Format("{0} must not be null.", nameof(_options.ClientId)));
        }

        if (string.IsNullOrEmpty(_options.ClientSecret))
        {
            throw new InvalidOperationException(string.Format("{0} must not be null.", nameof(_options.ClientSecret)));
        }

        var url = "https://poapauth.auth0.com/oauth/token";

        var content = new StringContent(JsonConvert.SerializeObject(new
        {
            audience = _options.Audience,
            client_id = _options.ClientId,
            client_secret = _options.ClientSecret,
            grant_type = "client_credentials",
        }), Encoding.UTF8, "application/json");

        var response = await RequestAsync(url, "", HttpMethod.Post, content: content, ct: cancellationToken);

        var jo = JObject.Parse(response);

        return jo?.ToObject<Authenticated>();
    }

    public async Task<List<Token>> ScanAddressAsync(string address, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException(string.Format("{0} must not be null.", nameof(address)), nameof(address));
        }

        var uriPart = $"actions/scan/{address}";

        var response = await RequestAsync(_baseUrl, uriPart, HttpMethod.Get, ct: cancellationToken);

        var ja = JArray.Parse(response);

        var list = new List<Token>();

        foreach (var jo in ja)
        {
            var item = jo.ToObject<Token>();

            if (item != null)
            {
                list.Add(item);
            }
        }

        return list;
    }

    public async Task<Token?> GetTokenAsync(string tokenId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tokenId))
        {
            throw new ArgumentException(string.Format("{0} must not be null.", nameof(tokenId)), nameof(tokenId));
        }

        var uriPart = $"token/{tokenId}";

        var response = await RequestAsync(_baseUrl, uriPart, HttpMethod.Get, ct: cancellationToken);

        var jo = JObject.Parse(response);

        return jo?.ToObject<Token>();
    }

    public async Task<PaginatedItems<Event>?> GetEventsPaginatedAsync(long offset = 0, int limit = 10, CancellationToken cancellationToken = default)
    {
        if (limit > 1000)
        {
            throw new ArgumentException(string.Format("{0} must be set to a value lesser than 300.", nameof(limit)), nameof(limit));
        }

        var uriPart = $"paginated-events";

        var queryParams = new List<(string, string)> { ("offset", offset.ToString()), ("limit", limit.ToString()) };

        var response = await RequestAsync(_baseUrl, uriPart, HttpMethod.Get, queryParams: queryParams, ct: cancellationToken);

        var jo = JObject.Parse(response);

        return jo?.ToObject<PaginatedItems<Event>>();
    }

    public async Task<PaginatedTokens<Poap>?> GetEventPoapsAsync(string eventId, long offset = 0, int limit = 10, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(eventId))
        {
            throw new ArgumentException(string.Format("{0} must not be null.", nameof(eventId)), nameof(eventId));
        }

        if (limit > 300)
        {
            throw new ArgumentException(string.Format("{0} must be set to a value lesser than 300.", nameof(limit)), nameof(limit));
        }

        var queryParams = new List<(string, string)> { ("offset", offset.ToString()), ("limit", limit.ToString()) };

        var uriPart = $"event/{eventId}/poaps";

        var authorizationHeader = await GetAccessTokenAsync(cancellationToken);

        var response = await RequestAsync(_baseUrl, uriPart, HttpMethod.Get, queryParams: queryParams, authorizationHeader: authorizationHeader, ct: cancellationToken);

        var jo = JObject.Parse(response);

        return jo?.ToObject<PaginatedTokens<Poap>>();
    }

    protected async Task<AuthenticationHeaderValue> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        if (_options.AccessToken != null)
        {
            return new AuthenticationHeaderValue("Bearer", _options.AccessToken);
        }

        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            _options.AccessToken ??= (await AuthenticateAsync(cancellationToken))?.AccessToken;

            return new AuthenticationHeaderValue("Bearer", _options.AccessToken);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    protected async Task<string> RequestAsync(
        string baseUrl,
        string uriPart,
        HttpMethod method,
        HttpContent? content = null,
        IEnumerable<(string, string)>? queryParams = null,
        Dictionary<string, string>? headers = null,
        AuthenticationHeaderValue? authorizationHeader = null,
        CancellationToken ct = default)
    {
        var uri = new Uri(QueryHelpers.AddQueryString($"{baseUrl}{uriPart}", queryParams));

        using var request = new HttpRequestMessage(method, uri);

        if (!_client.DefaultRequestHeaders.Contains(ApiKeyHeaderName))
        {
            request.Headers.Add(ApiKeyHeaderName, _options.ApiKey);
        }

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        if (authorizationHeader != null)
        {
            _client.DefaultRequestHeaders.Authorization = authorizationHeader;
        }

        if (content != null)
        {
            request.Content = content;
        }

        using var response = await _client.SendAsync(request, ct);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(ct);
    }
}