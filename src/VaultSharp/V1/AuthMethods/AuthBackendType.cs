using System;
using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods
{
    /// <summary>
    /// A helper class for retrieving and comparing Authentication Backend types.
    /// </summary>
    [JsonConverter(typeof(AuthBackendTypeJsonConverter))] 
    public class AuthBackendType : IEquatable<AuthBackendType>
    {
        /// <summary>
        /// Gets the application role.
        /// </summary>
        /// <value>
        /// The application role.
        /// </value>
        public static AuthBackendType AppRole { get; } = new AuthBackendType(AuthBackendDefaultPaths.AppRole);

        /// <summary>
        /// Gets the aws type.
        /// </summary>
        /// <value>
        /// The aws type.
        /// </value>
        public static AuthBackendType AWSRole { get; } = new AuthBackendType(AuthBackendDefaultPaths.AWS);

        /// <summary>
        /// Gets the git hub type.
        /// </summary>
        /// <value>
        /// The git hub.
        /// </value>
        public static AuthBackendType GitHub { get; } = new AuthBackendType(AuthBackendDefaultPaths.GitHub);

        /// <summary>
        /// Gets the GoogleCloud type.
        /// </summary>
        /// <value>
        /// The GoogleCloud.
        /// </value>
        public static AuthBackendType GoogleCloud { get; } = new AuthBackendType(AuthBackendDefaultPaths.GoogleCloud);

        /// <summary>
        /// Gets the Kubernetes type.
        /// </summary>
        /// <value>
        /// The Kubernetes.
        /// </value>
        public static AuthBackendType Kubernetes { get; } = new AuthBackendType(AuthBackendDefaultPaths.Kubernetes);

        /// <summary>
        /// Gets the LDAP type.
        /// </summary>
        /// <value>
        /// The LDAP.
        /// </value>
        public static AuthBackendType LDAP { get; } = new AuthBackendType(AuthBackendDefaultPaths.LDAP);

        /// <summary>
        /// Gets the Okta type.
        /// </summary>
        /// <value>
        /// The Okta.
        /// </value>
        public static AuthBackendType Okta { get; } = new AuthBackendType(AuthBackendDefaultPaths.Okta);

        /// <summary>
        /// Gets the RADIUS type.
        /// </summary>
        /// <value>
        /// The RADIUS.
        /// </value>
        public static AuthBackendType RADIUS { get; } = new AuthBackendType(AuthBackendDefaultPaths.RADIUS);

        /// <summary>
        /// Gets the certificate type.
        /// </summary>
        /// <value>
        /// The certificate.
        /// </value>
        public static AuthBackendType Cert { get; } = new AuthBackendType(AuthBackendDefaultPaths.Cert);

        /// <summary>
        /// Gets the token type.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public static AuthBackendType Token { get; } = new AuthBackendType(AuthBackendDefaultPaths.Token);

        /// <summary>
        /// Gets the generic type.
        /// </summary>
        /// <value>
        /// The generic.
        /// </value>
        public static AuthBackendType UserPass { get; } = new AuthBackendType(AuthBackendDefaultPaths.UserPass);

        /// <summary>
        /// Gets the type type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthBackendType" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public AuthBackendType(string type)
        {
            Type = type;
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

            return string.Compare(Type, other.Type, StringComparison.OrdinalIgnoreCase) == 0;
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
            return Type.ToUpperInvariant().GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Type;
        }
    }
}