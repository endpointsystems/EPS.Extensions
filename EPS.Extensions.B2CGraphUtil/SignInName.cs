namespace EPS.Extensions.B2CGraphUtil
{
    /// <summary>
    /// Contains information about a sign-in name of a local account user in an Azure Active Directory B2C
    /// tenant. The signInNames property of the User entity is a collection of SignInName. For more information
    /// about Azure Active Directory B2C, see the Azure Active Directory B2C documentation.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/azure/ad/graph/api/entity-and-complex-type-reference#signinname-type"/>
    public class SignInName
    {
        /// <summary>
        /// A string value that can be used to classify user sign-in types in your directory, such as
        /// "emailAddress" or "userName".
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// The sign-in used by the local account. Must be unique across the company/tenant.
        /// For example, "johnc@example.com".
        /// </summary>
        public string value { get; set; }
    }
}
