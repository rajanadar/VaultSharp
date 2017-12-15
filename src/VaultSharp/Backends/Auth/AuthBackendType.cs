using System;
using Newtonsoft.Json;
using VaultSharp.Core;

namespace VaultSharp.Backends.Auth
{
    /// <summary>
    /// A helper class for retrieving and comparing Authentication Backend types.
    /// </summary>
    [JsonConverter(typeof(AuthBackendTypeJsonConverter))] 
    public class AuthBackendType : IEquatable<AuthBackendType>
    {
        /// <summary>
        /// The application role type
        /// </summary>
        private static readonly AuthBackendType AppRoleType = new AuthBackendType(AuthBackendDefaultPaths.AppRole);

        /// <summary>
        /// The aws ec2 role type
        /// </summary>
        private static readonly AuthBackendType AWSType = new AuthBackendType(AuthBackendDefaultPaths.AWS);

        /// <summary>
        /// The git hub type
        /// </summary>
        private static readonly AuthBackendType GitHubType = new AuthBackendType("github");

        /// <summary>
        /// The LDAP type
        /// </summary>
        private static readonly AuthBackendType LDAPType = new AuthBackendType("ldap");

        /// <summary>
        /// The certificate type
        /// </summary>
        private static readonly AuthBackendType CertType = new AuthBackendType("cert");

        /// <summary>
        /// The token type
        /// </summary>
        private static readonly AuthBackendType TokenType = new AuthBackendType("token");

        /// <summary>
        /// The username password type
        /// </summary>
        private static readonly AuthBackendType UserPassType = new AuthBackendType("userpass");

        /// <summary>
        /// The _type
        /// </summary>
        private readonly string _type;

        /// <summary>
        /// Gets the application role.
        /// </summary>
        /// <value>
        /// The application role.
        /// </value>
        public static AuthBackendType AppRole
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
        public static AuthBackendType AWS
        {
            get
            {
                return AWS;
            }
        }

        /// <summary>
        /// Gets the git hub type.
        /// </summary>
        /// <value>
        /// The git hub.
        /// </value>
        public static AuthBackendType GitHub
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
        public static AuthBackendType LDAP
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
        public static AuthBackendType Cert
        {
            get
            {
                return CertType;
            }
        }

        /// <summary>
        /// Gets the token type.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public static AuthBackendType Token
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
        public static AuthBackendType UserPass
        {
            get
            {
                return UserPassType;
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
        /// Initializes a new instance of the <see cref="AuthBackendType" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public AuthBackendType(string type)
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
        public static bool operator ==(AuthBackendType left, AuthBackendType right)
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
        public static bool operator !=(AuthBackendType left, AuthBackendType right)
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
        public bool Equals(AuthBackendType other)
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
            return Equals(obj as AuthBackendType);
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