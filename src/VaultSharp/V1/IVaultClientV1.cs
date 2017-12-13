namespace VaultSharp
{
    public interface IVaultClientV1
    {
        ISecretBackend Secret { get; }

        IAuthBackend Auth { get; }

        ISystemBackend System { get; }
    }
}
