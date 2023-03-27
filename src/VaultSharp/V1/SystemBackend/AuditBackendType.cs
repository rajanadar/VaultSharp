﻿using System;
using System.Text.Json.Serialization;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// A helper class for retrieving and comparing Audit Backend types.
    /// </summary>
    [JsonConverter(typeof(AuditBackendTypeJsonConverter))] 
    public class AuditBackendType : IEquatable<AuditBackendType>
    {
        /// <summary>
        /// The _type
        /// </summary>
        private readonly string _value;

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public static AuditBackendType File { get; } = new AuditBackendType("file");

        /// <summary>
        /// Gets the syslog.
        /// </summary>
        /// <value>
        /// The syslog.
        /// </value>
        public static AuditBackendType Syslog { get; } = new AuditBackendType("syslog");

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Value => _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditBackendType"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public AuditBackendType(string value)
        {
            _value = value;
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

            return string.Equals(left.Value, right.Value, StringComparison.OrdinalIgnoreCase);
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

            return string.Compare(_value, other._value, StringComparison.OrdinalIgnoreCase) == 0;
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
            return _value.ToUpperInvariant().GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _value;
        }
    }
}