namespace EPS.Extensions.GraphObjects
{
    /// <summary>
    /// Represents a domain associated with the tenant.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/azure/ad/graph/api/entity-and-complex-type-reference#domain-entity"/>
    public class DomainEntity
    {
        /// <summary>
        /// Indicates what authentication type the domain is configured for. The value is either "Managed" or "Federated".
        /// </summary>
        public string authenticationType { get; set; }

        /// <summary>
        /// This property is always null except when the verify action is used. When the verify
        /// action is used, a Domain entity is returned in the response. The availabilityStatus property of
        /// the Domain entity in the response is either "AvailableImmediately" or "EmailVerifiedDomainTakeoverScheduled".
        /// </summary>
        public string availabilityStatus { get; set; }

        /// <summary>
        /// false, if the DNS record management of the domain has been delegated to Office 365. Otherwise, true.
        /// </summary>
        public bool isAdminManaged { get; set; }

        /// <summary>
        /// Indicates whether or not this is the default domain that is used for user creation. There is only
        /// one default domain per company.
        /// </summary>
        public bool isDefault { get; set; }

        /// <summary>
        /// Indicates whether or not this is the initial domain created by
        /// Microsoft Online Services (companyname.onmicrosoft.com). There is only one initial domain per company.
        /// </summary>
        public bool isInitial { get; set; }

        /// <summary>
        /// For subdomains, this represents the root domain. Only root domains need to be
        /// verified, and all subdomains will be automatically verified.
        /// </summary>
        public bool isRoot { get; set; }

        /// <summary>
        /// Indicates whether this domain has completed domain ownership verification or not.
        /// </summary>
        public bool isVerified { get; set; }

        /// <summary>
        /// The fully qualified name of the domain.
        /// </summary>
        /// <remarks>Notes: key, immutable, not nullable, unique</remarks>
        public string name { get; set; }

        /// <summary>
        /// The capabilities assigned to the domain. Can include 0, 1 or more of following values:
        /// <list type="bullet">
        /// <listItem><para>Email</para></listItem>
        /// <listItem><para>Sharepoint</para></listItem>
        /// <listItem><para>EmailInternalRelayOnly</para></listItem>
        /// <listItem><para>OfficeCommunicationsOnline</para></listItem>
        /// <listItem><para>SharePointDefaultDomain</para></listItem>
        /// <listItem><para>FullRedelegation</para></listItem>
        /// <listItem><para>SharePointPublic</para></listItem>
        /// <listItem><para>OrgIdAuthentication</para></listItem>
        /// <listItem><para>Yammer</para></listItem>
        /// <listItem><para>InTune</para></listItem>
        /// </list>
        /// </summary>
        public string[] supportedServices { get; set; }







    }
}
