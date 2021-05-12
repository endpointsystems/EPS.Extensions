namespace EPS.Extensions.B2CGraphUtil
{
    /// <summary>
    /// Contains information about a identity of a social account user in an Azure Active Directory B2C tenant. The userIdentities property of the User entity is a collection of userIdentity. For more information about Azure Active Directory B2C, see the Azure Active Directory B2C documentation.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/azure/ad/graph/api/entity-and-complex-type-reference#useridentity-type"/>
    public class UserIdentity
    {
        /// <summary>
        /// The string representation of the identity provider that issued the user identifier, such as facebook.com.
        /// </summary>
        public string issuer { get; set; }

        /// <summary>
        /// The unique user identifier used by the social identity provider.
        /// </summary>
        public string issuerUserId { get; set; }
    }
}
