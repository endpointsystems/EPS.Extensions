namespace EPS.Extensions.B2CGraphUtil
{
    /// <summary>
    /// Represents a CNAME record which needs to be added to the DNS zone file of a particular domain in the tenant. Inherited from <see cref="DomainDnsRecord"/>
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/azure/ad/graph/api/entity-and-complex-type-reference#domaindnscnamerecord-entity"/>
    public class DomainDnsCnameRecord
    {

        /// <summary>
        /// Indicates the value to use when configuring the canonical name of the CNAME record at the DNS host.
        /// </summary>
        public string canonicalName { get; set; }

        /// <summary>
        /// Unique identifier assigned to this entity.
        /// </summary>
        public string dnsRecordId { get; set; }

        /// <summary>
        /// Indicates whether this CNAME record must be configured by the customer at the DNS host for Microsoft Online Services to operate correctly with the domain.
        /// </summary>
        public bool isOptional { get; set; }

        /// <summary>
        /// Indicates the value to use when configuring the name of the CNAME record at the DNS host.
        /// </summary>
        public string record { get; set; }

        /// <summary>
        /// Indicates what type of DNS record this entity represents. The value is always "CName".
        /// </summary>
        public string recordType { get; set; }





    }
}
