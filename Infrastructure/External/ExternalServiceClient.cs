namespace Infrastructure.External;

public sealed class ExternalServiceClient : IExternalServiceClient
{
    private readonly HttpClient _client;

    public ExternalServiceClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> GetRemoteValueAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        using var response = await _client.GetAsync(relativePath, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }
}
