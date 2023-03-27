using System;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Database
{
    /// <summary>
    /// A helper class for retrieving and comparing DatabaseProviderTypes.
    /// </summary>
    [JsonConverter(typeof(DatabaseProviderTypeJsonConverter))] 
    public class DatabaseProviderType : IEquatable<DatabaseProviderType>
    {
        /// <summary>
        /// Gets the MySql provider name.
        /// </summary>
        public static DatabaseProviderType MySQL { get; } = new DatabaseProviderType(DatabaseProviders.MySQL);

        /// <summary>
        /// Gets the PostgreSQL provider name.
        /// </summary>
        public static DatabaseProviderType PostgreSQL { get; } = new DatabaseProviderType(DatabaseProviders.PostgreSQL);

        /// <summary>
        /// Gets the MongoDB provider name.
        /// </summary>
        public static DatabaseProviderType MongoDB { get; } = new DatabaseProviderType(DatabaseProviders.MongoDB);

        /// <summary>
        /// Gets the Oracle provider name.
        /// </summary>
        public static DatabaseProviderType Oracle { get; } = new DatabaseProviderType(DatabaseProviders.Oracle);

        /// <summary>
        /// Gets the RedShift provider name.
        /// </summary>
        public static DatabaseProviderType RedShift { get; } = new DatabaseProviderType(DatabaseProviders.RedShift);

        /// <summary>
        /// Gets the type type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseProviderType" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public DatabaseProviderType(string type)
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
        public static bool operator ==(DatabaseProviderType left, DatabaseProviderType right)
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
        public static bool operator !=(DatabaseProviderType left, DatabaseProviderType right)
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
        public bool Equals(DatabaseProviderType other)
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
            return Equals(obj as DatabaseProviderType);
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