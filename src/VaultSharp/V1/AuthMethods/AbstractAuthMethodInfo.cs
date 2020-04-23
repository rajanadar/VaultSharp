using Newtonsoft.Json;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods
{
    /// <summary>
    /// Abstract class. You don't say.
    /// </summary>
    public abstract class AbstractAuthMethodInfo : IAuthMethodInfo
    {
        [JsonIgnore]
        public AuthInfo ReturnedLoginAuthInfo { get; internal set; }

        public abstract AuthMethodType AuthMethodType { get; }
    }
}