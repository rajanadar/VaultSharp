using System;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines
{
    /// <summary>
    /// A helper class for retrieving and comparing SecretsEngine Backend types.
    /// </summary>
    [JsonConverter(typeof(SecretsEngineTypeJsonConverter))]
    public class SecretsEngineType : IEquatable<SecretsEngineType>
    {
        /// <summary>
        /// Gets the system type.
        /// </summary>
        /// <value>
        /// The system.
        /// </value>
        public static SecretsEngineType System { get; } = new SecretsEngineType(SecretsEngineDefaultPaths.System);

        /// <summary>
        /// Gets the alicloud type.
        /// </summary>
        /// <value>
        /// The aws.
        /// </value>
        public static SecretsEngineType AliCloud { get; } = new SecretsEngineType(SecretsEngineDefaultPaths.AliCloud);

        /// <summary>
        /// Gets the aws type.
        /// </summary>
        /// <value>
        /// The aws.
        /// </value>
        public static SecretsEngineType AWS { get; } = new SecretsEngineType(SecretsEngineDefaultPaths.AWS);

        /// <summary>
        /// Gets the consul type.
        /// </summary>
        /// <value>
        /// The consul.
        /// </value>
        public static SecretsEngineType Consul { get; } = new SecretsEngineType(SecretsEngineDefaultPaths.Consul);

        /// <summary>
        /// Gets the cubby hole type.
        /// </summary>
        /// <value>
        /// The cubby hole.
        /// </value>
        public static SecretsEngineType CubbyHole { get; } = new SecretsEngineType(SecretsEngineDefaultPaths.Cubbyhole);

        /// <summary>
        /// Gets the generic type v1.
        /// </summary>
        /// <value>
        /// The generic.
        /// </value>
        public static SecretsEngineType KeyValueV1 { get; } = new SecretsEngineType(SecretsEngineDefaultPaths.KeyValueV1);

        /// <summary>
        /// Gets the generic type v2.
        /// </summary>
        /// <value>
        /// The generic.
        /// </value>
        public static SecretsEngineType KeyValueV2 { get; } = new SecretsEngineType(SecretsEngineDefaultPaths.KeyValueV2);

        /// <summary>
        /// Gets the Identity type.
        /// </summary>
        /// <value>
        /// The Identity.
        /// </value>
        public static SecretsEngineType Identity { get; } = new SecretsEngineType(SecretsEngineDefaultPaths.Identity);

        /// <summary>
        /// Gets the Nomad type.
        /// </summary>
        /// <value>
        /// The Nomad.
        /// </value>
        public static SecretsEngineType Nomad { get; } = new SecretsEngineType(SecretsEngineDefaultPaths.Nomad);

        /// <summary>
        /// Gets the pki type.
        /// </summary>
        /// <value>
        /// The pki.
        /// </value>
        public static SecretsEngineType PKI { get; } = new SecretsEngineType(SecretsEngineDefaultPaths.PKI);

        /// <summary>
        /// Gets the rabbit mq type.
        /// </summary>
        /// <value>
        /// The rabbit mq.
        /// </value>
        public static SecretsEngineType RabbitMQ { get; } = new SecretsEngineType(SecretsEngineDefaultPaths.RabbitMQ);

        /// <summary>
        /// Gets the SSH type.
        /// </summary>
        /// <value>
        /// The SSH.
        /// </value>
        public static SecretsEngineType SSH { get; } = new SecretsEngineType(SecretsEngineDefaultPaths.SSH);

        /// <summary>
        /// Gets the TOTP type.
        /// </summary>
        /// <value>
        /// The TOTP.
        /// </value>
        public static SecretsEngineType TOTP { get; } = new SecretsEngineType(SecretsEngineDefaultPaths.TOTP);

        /// <summary>
        /// Gets the transit type.
        /// </summary>
        /// <value>
        /// The transit.
        /// </value>
        public static SecretsEngineType Transit { get; } = new SecretsEngineType(SecretsEngineDefaultPaths.Transit);

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretsEngineType"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public SecretsEngineType(string type)
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
        public static bool operator ==(SecretsEngineType left, SecretsEngineType right)
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
        public static bool operator !=(SecretsEngineType left, SecretsEngineType right)
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
        public bool Equals(SecretsEngineType other)
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
            return Equals(obj as SecretsEngineType);
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