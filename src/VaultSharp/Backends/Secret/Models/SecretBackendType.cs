using System;
using Newtonsoft.Json;
using VaultSharp.Infrastructure.JsonConverters;
using VaultSharp.Infrastructure.Validation;

namespace VaultSharp.Backends.Secret.Models
{
    /// <summary>
    /// A helper class for retrieving and comparing Secret Backend types.
    /// </summary>
    [JsonConverter(typeof(SecretBackendTypeJsonConverter))] 
    public class SecretBackendType : IEquatable<SecretBackendType>
    {
        /// <summary>
        /// The system type
        /// </summary>
        private static readonly SecretBackendType SystemType = new SecretBackendType(SecretBackendDefaultMountPoints.System);

        /// <summary>
        /// The aws type
        /// </summary>
        private static readonly SecretBackendType AWSType = new SecretBackendType(SecretBackendDefaultMountPoints.AWS);

        /// <summary>
        /// The cassandra type
        /// </summary>
        private static readonly SecretBackendType CassandraType = new SecretBackendType(SecretBackendDefaultMountPoints.Cassandra);

        /// <summary>
        /// The consul type
        /// </summary>
        private static readonly SecretBackendType ConsulType = new SecretBackendType(SecretBackendDefaultMountPoints.Consul);

        /// <summary>
        /// The cubby hole type.
        /// It is mounted at the cubbyhole/ prefix by default and cannot be mounted elsewhere or removed.
        /// </summary>
        private static readonly SecretBackendType CubbyHoleType = new SecretBackendType(SecretBackendDefaultMountPoints.Cubbyhole);

        /// <summary>
        /// The generic type
        /// </summary>
        private static readonly SecretBackendType GenericType = new SecretBackendType("generic");

        /// <summary>
        /// MongoDb type
        /// </summary>
        private static readonly SecretBackendType MongoDbType = new SecretBackendType(SecretBackendDefaultMountPoints.MongoDb);

        /// <summary>
        /// Microsoft SQL type
        /// </summary>
        private static readonly SecretBackendType MicrosoftSqlType = new SecretBackendType(SecretBackendDefaultMountPoints.MicrosoftSql);

        /// <summary>
        /// My SQL type
        /// </summary>
        private static readonly SecretBackendType MySqlType = new SecretBackendType(SecretBackendDefaultMountPoints.MySql);

        /// <summary>
        /// The pki type
        /// </summary>
        private static readonly SecretBackendType PKIType = new SecretBackendType(SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// The postgre SQL type
        /// </summary>
        private static readonly SecretBackendType PostgreSqlType = new SecretBackendType(SecretBackendDefaultMountPoints.PostgreSql);
        
        /// <summary>
        /// The rabbit mq type
        /// </summary>
        private static readonly SecretBackendType RabbitMQType = new SecretBackendType(SecretBackendDefaultMountPoints.RabbitMQ);

        /// <summary>
        /// The SSH type
        /// </summary>
        private static readonly SecretBackendType SSHType = new SecretBackendType(SecretBackendDefaultMountPoints.SSH);

        /// <summary>
        /// The transit type
        /// </summary>
        private static readonly SecretBackendType TransitType = new SecretBackendType(SecretBackendDefaultMountPoints.Transit);

        /// <summary>
        /// The _type
        /// </summary>
        private readonly string _type;

        /// <summary>
        /// Gets the system type.
        /// </summary>
        /// <value>
        /// The system.
        /// </value>
        public static SecretBackendType System
        {
            get
            {
                return SystemType;
            }
        }

        /// <summary>
        /// Gets the aws type.
        /// </summary>
        /// <value>
        /// The aws.
        /// </value>
        public static SecretBackendType AWS
        {
            get
            {
                return AWSType;
            }
        }

        /// <summary>
        /// Gets the cassandra type.
        /// </summary>
        /// <value>
        /// The cassandra.
        /// </value>
        public static SecretBackendType Cassandra
        {
            get
            {
                return CassandraType;
            }
        }

        /// <summary>
        /// Gets the consul type.
        /// </summary>
        /// <value>
        /// The consul.
        /// </value>
        public static SecretBackendType Consul
        {
            get
            {
                return ConsulType;
            }
        }

        /// <summary>
        /// Gets the cubby hole type.
        /// </summary>
        /// <value>
        /// The cubby hole.
        /// </value>
        public static SecretBackendType CubbyHole
        {
            get
            {
                return CubbyHoleType;
            }
        }

        /// <summary>
        /// Gets the generic type.
        /// </summary>
        /// <value>
        /// The generic.
        /// </value>
        public static SecretBackendType Generic
        {
            get
            {
                return GenericType;
            }
        }

        /// <summary>
        /// Gets the mongo database.
        /// </summary>
        /// <value>
        /// The mongo database.
        /// </value>
        public static SecretBackendType MongoDb
        {
            get
            {
                return MongoDbType;
            }
        }

        /// <summary>
        /// Gets the Microsoft Sql type.
        /// </summary>
        /// <value>
        /// Microsoft Sql.
        /// </value>
        public static SecretBackendType MicrosoftSql
        {
            get
            {
                return MicrosoftSqlType;
            }
        }

        /// <summary>
        /// Gets the MySql type.
        /// </summary>
        /// <value>
        /// My SQL.
        /// </value>
        public static SecretBackendType MySql
        {
            get
            {
                return MySqlType;
            }
        }

        /// <summary>
        /// Gets the pki type.
        /// </summary>
        /// <value>
        /// The pki.
        /// </value>
        public static SecretBackendType PKI
        {
            get
            {
                return PKIType;
            }
        }

        /// <summary>
        /// Gets the postgre SQL type.
        /// </summary>
        /// <value>
        /// The postgre SQL.
        /// </value>
        public static SecretBackendType PostgreSql
        {
            get
            {
                return PostgreSqlType;
            }
        }

        /// <summary>
        /// Gets the rabbit mq type.
        /// </summary>
        /// <value>
        /// The rabbit mq.
        /// </value>
        public static SecretBackendType RabbitMQ
        {
            get
            {
                return RabbitMQType;
            }
        }

        /// <summary>
        /// Gets the SSH type.
        /// </summary>
        /// <value>
        /// The SSH.
        /// </value>
        public static SecretBackendType SSH
        {
            get
            {
                return SSHType;
            }
        }

        /// <summary>
        /// Gets the transit type.
        /// </summary>
        /// <value>
        /// The transit.
        /// </value>
        public static SecretBackendType Transit
        {
            get
            {
                return TransitType;
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
        /// Initializes a new instance of the <see cref="SecretBackendType"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public SecretBackendType(string type)
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
        public static bool operator ==(SecretBackendType left, SecretBackendType right)
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
        public static bool operator !=(SecretBackendType left, SecretBackendType right)
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
        public bool Equals(SecretBackendType other)
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
            return Equals(obj as SecretBackendType);
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