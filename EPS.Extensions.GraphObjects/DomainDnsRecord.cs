namespace EPS.Extensions.GraphObjects
{
    /// <summary>
    /// For each domain in the tenant, you may be required to add DNS record(s) to the DNS
    /// zone file of the domain before the domain can be used by Microsoft Online Services.
    /// The <see cref="DomainDnsRecord"/> entity is used to present such DNS records. Base
    /// entity for <see cref="DomainDnsCnameRecord"/>, <see cref="DomainDnsMxRecord"/>,
    /// <see cref="DomainDnsSrvRecord"/> and <see cref="DomainDnsSrvRecord"/> entities.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/azure/ad/graph/api/entity-and-complex-type-reference#domaindnsrecord-entity"/>
    public class DomainDnsRecord
    {
        /// <summary>
        /// Unique identifier assigned to this entity.
        /// </summary>
        public string dnsRecordId { get; set; }

        /// <summary>
        /// Indicates whether this record must be configured by the customer at the DNS host for Microsoft Online Services to operate correctly with the domain.
        /// </summary>
        public bool isOptional { get; set; }

        /// <summary>
        /// Indicates the value to use when configuring the name of the DNS record at the DNS host.
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// Indicates what type of DNS record this entity represents. The value can be one of the following:
        /// Cname, Mx, Srv, Txt
        /// </summary>
        public string recordType { get; set; }

        /// <summary>
        /// The capabilities assigned to the domain. Can be one of the following values:
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
        public string supportedService { get; set; }

        /// <summary>
        /// Indicates the value to use when configuring the time-to-live (ttl) property of the DNS record at the DNS host.
        /// </summary>
        public int ttl { get; set; }

    }
}
