using System;
using Newtonsoft.Json;

namespace VaultSharp.SecretEngines
{
    /// <summary>
    /// A helper class for retrieving and comparing SecretEngine Backend types.
    /// </summary>
    [JsonConverter(typeof(SecretEngineTypeJsonConverter))]
    public class SecretEngineType : IEquatable<SecretEngineType>
    {
        /// <summary>
        /// Gets the system type.
        /// </summary>
        /// <value>
        /// The system.
        /// </value>
        public static SecretEngineType System { get; } = new SecretEngineType(SecretEngineDefaultPaths.System);

        /// <summary>
        /// Gets the aws type.
        /// </summary>
        /// <value>
        /// The aws.
        /// </value>
        public static SecretEngineType AWS { get; } = new SecretEngineType(SecretEngineDefaultPaths.AWS);

        /// <summary>
        /// Gets the consul type.
        /// </summary>
        /// <value>
        /// The consul.
        /// </value>
        public static SecretEngineType Consul { get; } = new SecretEngineType(SecretEngineDefaultPaths.Consul);

        /// <summary>
        /// Gets the cubby hole type.
        /// </summary>
        /// <value>
        /// The cubby hole.
        /// </value>
        public static SecretEngineType CubbyHole { get; } = new SecretEngineType(SecretEngineDefaultPaths.Cubbyhole);

        /// <summary>
        /// Gets the generic type.
        /// </summary>
        /// <value>
        /// The generic.
        /// </value>
        public static SecretEngineType KeyValue { get; } = new SecretEngineType(SecretEngineDefaultPaths.KeyValue);

        /// <summary>
        /// Gets the Identity type.
        /// </summary>
        /// <value>
        /// The Identity.
        /// </value>
        public static SecretEngineType Identity { get; } = new SecretEngineType(SecretEngineDefaultPaths.Identity);

        /// <summary>
        /// Gets the Nomad type.
        /// </summary>
        /// <value>
        /// The Nomad.
        /// </value>
        public static SecretEngineType Nomad { get; } = new SecretEngineType(SecretEngineDefaultPaths.Nomad);

        /// <summary>
        /// Gets the pki type.
        /// </summary>
        /// <value>
        /// The pki.
        /// </value>
        public static SecretEngineType PKI { get; } = new SecretEngineType(SecretEngineDefaultPaths.PKI);

        /// <summary>
        /// Gets the rabbit mq type.
        /// </summary>
        /// <value>
        /// The rabbit mq.
        /// </value>
        public static SecretEngineType RabbitMQ { get; } = new SecretEngineType(SecretEngineDefaultPaths.RabbitMQ);

        /// <summary>
        /// Gets the SSH type.
        /// </summary>
        /// <value>
        /// The SSH.
        /// </value>
        public static SecretEngineType SSH { get; } = new SecretEngineType(SecretEngineDefaultPaths.SSH);

        /// <summary>
        /// Gets the TOTP type.
        /// </summary>
        /// <value>
        /// The TOTP.
        /// </value>
        public static SecretEngineType TOTP { get; } = new SecretEngineType(SecretEngineDefaultPaths.TOTP);

        /// <summary>
        /// Gets the transit type.
        /// </summary>
        /// <value>
        /// The transit.
        /// </value>
        public static SecretEngineType Transit { get; } = new SecretEngineType(SecretEngineDefaultPaths.Transit);

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretEngineType"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public SecretEngineType(string type)
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
        public static bool operator ==(SecretEngineType left, SecretEngineType right)
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
        public static bool operator !=(SecretEngineType left, SecretEngineType right)
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
        public bool Equals(SecretEngineType other)
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
            return Equals(obj as SecretEngineType);
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