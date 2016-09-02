using System;
using Newtonsoft.Json;
using VaultSharp.Infrastructure.JsonConverters;
using VaultSharp.Infrastructure.Validation;

namespace VaultSharp.Backends.Audit.Models
{
    /// <summary>
    /// A helper class for retrieving and comparing Audit Backend types.
    /// </summary>
    [JsonConverter(typeof(AuditBackendTypeJsonConverter))] 
    public class AuditBackendType : IEquatable<AuditBackendType>
    {
        /// <summary>
        /// The file type
        /// </summary>
        private static readonly AuditBackendType FileType = new AuditBackendType("file");

        /// <summary>
        /// The syslog type
        /// </summary>
        private static readonly AuditBackendType SyslogType = new AuditBackendType("syslog");

        /// <summary>
        /// The _type
        /// </summary>
        private readonly string _type;

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public static AuditBackendType File
        {
            get
            {
                return FileType;
            }
        }

        /// <summary>
        /// Gets the syslog.
        /// </summary>
        /// <value>
        /// The syslog.
        /// </value>
        public static AuditBackendType Syslog
        {
            get
            {
                return SyslogType;
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditBackendType"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public AuditBackendType(string type)
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
        public static bool operator ==(AuditBackendType left, AuditBackendType right)
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
        public static bool operator !=(AuditBackendType left, AuditBackendType right)
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
        public bool Equals(AuditBackendType other)
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
            return Equals(obj as AuditBackendType);
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