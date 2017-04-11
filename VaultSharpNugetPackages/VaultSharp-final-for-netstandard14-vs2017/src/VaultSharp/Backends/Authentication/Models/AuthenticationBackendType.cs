using System;
using Newtonsoft.Json;
using VaultSharp.Infrastructure.JsonConverters;
using VaultSharp.Infrastructure.Validation;

namespace VaultSharp.Backends.Authentication.Models
{
    /// <summary>
    /// A helper class for retrieving and comparing Authentication Backend types.
    /// </summary>
    [JsonConverter(typeof(AuthenticationBackendTypeJsonConverter))] 
    public class AuthenticationBackendType : IEquatable<AuthenticationBackendType>
    {
        /// <summary>
        /// The application identifier type
        /// </summary>
        private static readonly AuthenticationBackendType AppIdType = new AuthenticationBackendType(AuthenticationBackendDefaultPaths.AppId);

        /// <summary>
        /// The application role type
        /// </summary>
        private static readonly AuthenticationBackendType AppRoleType = new AuthenticationBackendType(AuthenticationBackendDefaultPaths.AppRole);

        /// <summary>
        /// The aws ec2 role type
        /// </summary>
        private static readonly AuthenticationBackendType AwsEc2RoleType = new AuthenticationBackendType(AuthenticationBackendDefaultPaths.AwsEc2);

        /// <summary>
        /// The git hub type
        /// </summary>
        private static readonly AuthenticationBackendType GitHubType = new AuthenticationBackendType("github");

        /// <summary>
        /// The LDAP type
        /// </summary>
        private static readonly AuthenticationBackendType LDAPType = new AuthenticationBackendType("ldap");

        /// <summary>
        /// The certificate type
        /// </summary>
        private static readonly AuthenticationBackendType CertificateType = new AuthenticationBackendType("cert");

        /// <summary>
        /// The token type
        /// </summary>
        private static readonly AuthenticationBackendType TokenType = new AuthenticationBackendType("token");

        /// <summary>
        /// The username password type
        /// </summary>
        private static readonly AuthenticationBackendType UsernamePasswordType = new AuthenticationBackendType("userpass");

        /// <summary>
        /// The _type
        /// </summary>
        private readonly string _type;

        /// <summary>
        /// Gets the application identifier type.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        [Obsolete("The AppId Authentication backend in Vault is now deprecated with the addition " +
          "of the new AppRole backend. There are no plans to remove it, but we encourage " +
          "using AppRole whenever possible, as it offers enhanced functionality " +
          "and can accommodate many more types of authentication paradigms.")]
        public static AuthenticationBackendType AppId
        {
            get
            {
                return AppIdType;
            }
        }

        /// <summary>
        /// Gets the application role.
        /// </summary>
        /// <value>
        /// The application role.
        /// </value>
        public static AuthenticationBackendType AppRole
        {
            get
            {
                return AppRoleType;
            }
        }

        /// <summary>
        /// Gets the aws ec2.
        /// </summary>
        /// <value>
        /// The aws ec2.
        /// </value>
        public static AuthenticationBackendType AwsEc2
        {
            get
            {
                return AwsEc2RoleType;
            }
        }

        /// <summary>
        /// Gets the git hub type.
        /// </summary>
        /// <value>
        /// The git hub.
        /// </value>
        public static AuthenticationBackendType GitHub
        {
            get
            {
                return GitHubType;
            }
        }

        /// <summary>
        /// Gets the LDAP type.
        /// </summary>
        /// <value>
        /// The LDAP.
        /// </value>
        public static AuthenticationBackendType LDAP
        {
            get
            {
                return LDAPType;
            }
        }

        /// <summary>
        /// Gets the certificate type.
        /// </summary>
        /// <value>
        /// The certificate.
        /// </value>
        public static AuthenticationBackendType Certificate
        {
            get
            {
                return CertificateType;
            }
        }

        /// <summary>
        /// Gets the token type.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public static AuthenticationBackendType Token
        {
            get
            {
                return TokenType;
            }
        }

        /// <summary>
        /// Gets the generic type.
        /// </summary>
        /// <value>
        /// The generic.
        /// </value>
        public static AuthenticationBackendType UsernamePassword
        {
            get
            {
                return UsernamePasswordType;
            }
        }

        /// <summary>
        /// Gets the type type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationBackendType" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public AuthenticationBackendType(string type)
        {
            Checker.NotNull(type, "type");

            _type = type;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(AuthenticationBackendType left, AuthenticationBackendType right)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)left == null) || ((object)right == null))
            {
                return false;
            }

            return string.Equals(left.Type, right.Type, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(AuthenticationBackendType left, AuthenticationBackendType right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(AuthenticationBackendType other)
        {
            if ((object)other == null)
                return false;

            return string.Compare(_type, other._type, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as AuthenticationBackendType);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return _type.ToUpperInvariant().GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _type;
        }
    }
}