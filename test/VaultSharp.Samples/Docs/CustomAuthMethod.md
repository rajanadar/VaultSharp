# Custom Authentication Method Guide
This guide expands on the VaultSharp Custom Auth Method documentation and demonstrates how to:
- Use a custom token provider
- Add retry logic for token expiration

### Implementing GetCustomAuthMethodInfoAsync
The CustomAuthMethodInfo constructor accepts a delegate that returns an AuthInfo object. This is where you provide your Vault token—whether it's read from memory, the file system, or fetched from a remote service.
If your token is already available in memory, the implementation can be simple:
 
```C#
private DateTime? VaultTokenLastGenerated { get; set; }
private Task<AuthInfo> GetCustomAuthMethodInfo()
{
    VaultTokenLastGenerated = DateTime.UtcNow;
    var vaultOptions = new VaultOptions();
    return Task.FromResult(new AuthInfo()
    {
        ClientToken = vaultOptions.VaultToken
    });
}
```
 
 
💡 This method can also be asynchronous if your token source requires I/O (e.g., reading from disk or calling an API).

### Creating the Vault Client with Custom Auth
Once you have your token provider, you can initialize the VaultClient using CustomAuthMethodInfo. This is especially useful in singleton services where you want to avoid rebuilding the client every time the token changes.
 
```C#
private VaultClient GenerateNewVaultClientAsync()
{
    if (_vault == null)
    {
        var vaultOptions = new VaultOptions();
        var vaultSettings = new VaultClientSettings(
            vaultOptions.VaultAddress,
            new CustomAuthMethodInfo("vault-server-auth-method", GetCustomAuthMethodInfo)
        );
        if (_httpClient is not null)
        {
            vaultSettings.MyHttpClientProviderFunc = handler => _httpClient;
        }
        _vault = new VaultClient(vaultSettings);
    }
    return _vault;
}
private VaultClient _vaultClient
{
    get
    {
        if (_vault == null)
        {
            GenerateNewVaultClientAsync();
        }
        else if (VaultTokenLastGenerated == null
            || DateTime.UtcNow.Subtract((DateTime)VaultTokenLastGenerated).Seconds > (VaultOptions.VaultTokenTimeToLive))
        {
            _vault.V1.Auth.ResetVaultToken();
        }
        return _vault;
    }
    set => _vault = value;
}
```
 
 
This setup ensures that the client always uses the latest token provided by your orchestrator or token management system.

### Adding Retry Logic for Token Expiration
Vault tokens can expire or be revoked. To handle this gracefully, you can catch VaultApiException errors and reset the token before retrying the operation:
 
```C#
public async Task<Secret<T>> ReadSecretAsync<T>(string path, string mountPoint = null, string wrapTimeToLive = null)
{
    try
    {
        return await _vaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync<T>(path, mountPoint, wrapTimeToLive);
    }
    catch (VaultApiException ex) when (ex.HttpStatusCode == HttpStatusCode.Forbidden)
    {
        _logger?.LogError(ex, "Vault Could not be authenticated with current token retrieving new token and trying again.");
        _vaultClient.V1.Auth.ResetVaultToken();
        return await _vaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync<T>(path, mountPoint, wrapTimeToLive);
    }
}
```

### Summary
VaultSharp’s CustomAuthMethodInfo is a powerful extension point for integrating dynamic token sources. By combining it with retry logic and singleton-safe client initialization, you can build a robust and flexible Vault integration.