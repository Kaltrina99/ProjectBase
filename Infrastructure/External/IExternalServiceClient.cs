namespace Infrastructure.External;

public interface IExternalServiceClient
{
    Task<string> GetRemoteValueAsync(string relativePath, CancellationToken cancellationToken = default);
}
