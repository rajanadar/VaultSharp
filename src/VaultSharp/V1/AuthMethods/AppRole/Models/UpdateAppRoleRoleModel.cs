using System;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.AuthMethods.AppRole.Models
{
    public class UpdateAppRoleRoleModel : AppRoleRoleModel
    {
        [JsonIgnore]
        [Obsolete("LocalSecretIds can only be set when creating an app role. Use AppRoleRoleModel if creating a new app role", true)]
        public override bool LocalSecretIds { get; set; }
    }
}
