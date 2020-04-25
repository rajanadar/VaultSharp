using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.LDAP
{
    /// <summary>
    /// LDAP Auth method.
    /// </summary>
    public interface ILDAPAuthMethod
    {
        /// <summary>
        /// This endpoint creates or updates LDAP group policies.
        /// </summary>
        /// <param name="groupName"><para>[required]</para>
        /// The name of the LDAP group
        /// </param>
        /// <param name="policies"><para>[required]</para>
        /// List of policies associated to the group.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the LDAP backend. Defaults to <see cref="AuthMethodDefaultPaths.LDAP" />
        /// Provide a value only if you have customized the mount point.</param>        
        Task WriteGroupAsync(string groupName, IList<string> policies, string mountPoint = AuthMethodDefaultPaths.LDAP);

        /// <summary>
        /// This endpoint reads LDAP group policies.
        /// </summary>
        /// <param name="groupName"><para>[required]</para>
        /// The name of the LDAP group
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the LDAP backend. Defaults to <see cref="AuthMethodDefaultPaths.LDAP" />
        /// Provide a value only if you have customized the mount point.</param>  
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The policies.
        /// </returns>
        Task<Secret<List<string>>> ReadGroupAsync(string groupName, string mountPoint = AuthMethodDefaultPaths.LDAP, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint reads all LDAP groups.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the LDAP backend. Defaults to <see cref="AuthMethodDefaultPaths.LDAP" />
        /// Provide a value only if you have customized the mount point.</param>  
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The groups.
        /// </returns>
        Task<Secret<ListInfo>> ReadAllGroupsAsync(string mountPoint = AuthMethodDefaultPaths.LDAP, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint deletes the LDAP group.
        /// </summary>
        /// <param name="groupName"><para>[required]</para>
        /// The name of the LDAP group
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the LDAP backend. Defaults to <see cref="AuthMethodDefaultPaths.LDAP" />
        /// Provide a value only if you have customized the mount point.</param>  
        Task DeleteGroupAsync(string groupName, string mountPoint = AuthMethodDefaultPaths.LDAP);

        /// <summary>
        /// This endpoint creates or updates LDAP users policies and group associations.
        /// </summary>
        /// <param name="username"><para>[required]</para>
        /// The name of the user
        /// </param>
        /// <param name="policies"><para>[required]</para>
        /// List of policies associated to the user.
        /// </param>
        /// <param name="groups"><para>[required]</para>
        /// List of groups associated to the user.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the LDAP backend. Defaults to <see cref="AuthMethodDefaultPaths.LDAP" />
        /// Provide a value only if you have customized the mount point.</param>        
        Task WriteUserAsync(string username, IList<string> policies, IList<string> groups, string mountPoint = AuthMethodDefaultPaths.LDAP);

        /// <summary>
        /// This endpoint reads LDAP user.
        /// </summary>
        /// <param name="username"><para>[required]</para>
        /// The name of user
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the LDAP backend. Defaults to <see cref="AuthMethodDefaultPaths.LDAP" />
        /// Provide a value only if you have customized the mount point.</param>  
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The user.
        /// </returns>
        Task<Secret<Dictionary<string, object>>> ReadUserAsync(string username, string mountPoint = AuthMethodDefaultPaths.LDAP, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint reads all LDAP users.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the LDAP backend. Defaults to <see cref="AuthMethodDefaultPaths.LDAP" />
        /// Provide a value only if you have customized the mount point.</param>  
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The users.
        /// </returns>
        Task<Secret<ListInfo>> ReadAllUsersAsync(string mountPoint = AuthMethodDefaultPaths.LDAP, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint deletes the LDAP user.
        /// </summary>
        /// <param name="username"><para>[required]</para>
        /// The name of the LDAP user
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the LDAP backend. Defaults to <see cref="AuthMethodDefaultPaths.LDAP" />
        /// Provide a value only if you have customized the mount point.</param>  
        Task DeleteUserAsync(string username, string mountPoint = AuthMethodDefaultPaths.LDAP);
    }
}