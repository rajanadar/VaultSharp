using System;
using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods
{
    /// <summary>
    /// A helper class for retrieving and comparing Authentication Backend types.
    /// </summary>
    [JsonConverter(typeof(AuthMethodTypeJsonConverter))] 
    public class AuthMethodType : IEquatable<AuthMethodType>
    {
        /// <summary>
        /// Gets the ali cloud.
        /// </summary>
        /// <value>
        /// The ali cloud.
        /// </value>
        public static AuthMethodType AliCloud { get; } = new AuthMethodType(AuthMethodDefaultPaths.AliCloud);

        /// <summary>
        /// Gets the application role.
        /// </summary>
        /// <value>
        /// The application role.
        /// </value>
        public static AuthMethodType AppRole { get; } = new AuthMethodType(AuthMethodDefaultPaths.AppRole);

        /// <summary>
        /// Gets the aws type.
        /// </summary>
        /// <value>
        /// The aws type.
        /// </value>
        public static AuthMethodType AWS { get; } = new AuthMethodType(AuthMethodDefaultPaths.AWS);


        /// <summary>
        /// Gets the aws type.
        /// </summary>
        /// <value>
        /// The aws type.
        /// </value>
        public static AuthMethodType Azure { get; } = new AuthMethodType(AuthMethodDefaultPaths.Azure);

        /// <summary>
        /// Gets the git hub type.
        /// </summary>
        /// <value>
        /// The git hub.
        /// </value>
        public static AuthMethodType GitHub { get; } = new AuthMethodType(AuthMethodDefaultPaths.GitHub);

        /// <summary>
        /// Gets the GoogleCloud type.
        /// </summary>
        /// <value>
        /// The GoogleCloud.
        /// </value>
        public static AuthMethodType GoogleCloud { get; } = new AuthMethodType(AuthMethodDefaultPaths.GoogleCloud);

        /// <summary>
        /// Gets the JWT type.
        /// </summary>
        /// <value>
        /// The JWT.
        /// </value>
        public static AuthMethodType JWT { get; } = new AuthMethodType(AuthMethodDefaultPaths.JWT);

        /// <summary>
        /// Gets the Kubernetes type.
        /// </summary>
        /// <value>
        /// The Kubernetes.
        /// </value>
        public static AuthMethodType Kubernetes { get; } = new AuthMethodType(AuthMethodDefaultPaths.Kubernetes);

        /// <summary>
        /// Gets the LDAP type.
        /// </summary>
        /// <value>
        /// The LDAP.
        /// </value>
        public static AuthMethodType LDAP { get; } = new AuthMethodType(AuthMethodDefaultPaths.LDAP);

        /// <summary>
        /// Gets the Kerberos type.
        /// </summary>
        /// <value>
        /// The Kerberos.
        /// </value>
        public static AuthMethodType Kerberos { get; } = new AuthMethodType(AuthMethodDefaultPaths.Kerberos);

        /// <summary>
        /// Gets the Okta type.
        /// </summary>
        /// <value>
        /// The Okta.
        /// </value>
        public static AuthMethodType Okta { get; } = new AuthMethodType(AuthMethodDefaultPaths.Okta);

        /// <summary>
        /// Gets the RADIUS type.
        /// </summary>
        /// <value>
        /// The RADIUS.
        /// </value>
        public static AuthMethodType RADIUS { get; } = new AuthMethodType(AuthMethodDefaultPaths.RADIUS);

        /// <summary>
        /// Gets the certificate type.
        /// </summary>
        /// <value>
        /// The certificate.
        /// </value>
        public static AuthMethodType Cert { get; } = new AuthMethodType(AuthMethodDefaultPaths.Cert);

        /// <summary>
        /// Gets the token type.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public static AuthMethodType Token { get; } = new AuthMethodType(AuthMethodDefaultPaths.Token);

        /// <summary>
        /// Gets the generic type.
        /// </summary>
        /// <value>
        /// The generic.
        /// </value>
        public static AuthMethodType UserPass { get; } = new AuthMethodType(AuthMethodDefaultPaths.UserPass);

        /// <summary>
        /// Gets the Cloud Foundry type.
        /// </summary>
        /// <value>
        /// The Cloud Foundry.
        /// </value>
        public static AuthMethodType CloudFoundry { get; } = new AuthMethodType(AuthMethodDefaultPaths.CloudFoundry);

        /// <summary>
        /// Gets the type type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMethodType" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public AuthMethodType(string type)
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
        public static bool operator ==(AuthMethodType left, AuthMethodType right)
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
        public static bool operator !=(AuthMethodType left, AuthMethodType right)
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
        public bool Equals(AuthMethodType other)
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
            return Equals(obj as AuthMethodType);
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