public sealed class HttpClientFactorySingleton
{
    private static readonly Lazy<HttpClient> _client = new(() =>
    {
        return new HttpClient();
    });

    public static HttpClient Instance => _client.Value;
}
