using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.AppRole.Models
{
    public class RoleId
    {
        /// <summary>
        ///     Contains RoleId
        /// </summary>
        [JsonProperty("role_id")]
        public string Role_Id { get; set; }
    }
}