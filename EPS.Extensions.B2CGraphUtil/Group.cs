using System;

namespace EPS.Extensions.B2CGraphUtil
{
    /// <summary>
    ///
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/azure/ad/graph/api/entity-and-complex-type-reference#group-entity"/>
    public class Group: DirectoryObject
    {
        /// <summary>
        /// An optional description for the group.
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// true if this object is synced from an on-premises directory; false if this object was originally synced from an on-premises directory but is no longer synced; null if this object has never been synced from an on-premises directory (default).
        /// </summary>
        public bool dirSyncEnabled { get; set; }

        /// <summary>
        /// The display name for the group. This property is required when a group is created and it cannot be cleared during updates.
        /// </summary>
        public string displayName { get; set; }

        /// <summary>
        /// Indicates the last time at which the object was synced with the on-premises directory.
        /// </summary>
        public DateTime lastDirSyncTime { get; set; }

        /// <summary>
        /// The SMTP address for the group, for example, "serviceadmins@contoso.onmicrosoft.com".
        /// </summary>
        public string mail { get; set; }

        /// <summary>
        /// Specifies whether the group is mail-enabled. If the securityEnabled property is also true, the group is a mail-enabled security group; otherwise, the group is a Microsoft Exchange distribution group. Only (pure) security groups can be created using Azure AD Graph. For this reason, the property must be set false when creating a group and it cannot be updated using Azure AD Graph.
        /// </summary>
        public bool mailEnabled { get; set; }

        /// <summary>
        /// The mail alias for the group. This property must be specified when a group is created.
        /// </summary>
        public string mailNickname { get; set; }

        /// <summary>
        /// Contains the on-premises security identifier (SID) for the group that was synchronized from on-premises to the cloud.
        /// </summary>
        public string onPremisesSecurityIdentifier { get; set; }

        /// <summary>
        /// A collection of error details that are preventing this group from being provisioned successfully.
        /// </summary>
        public ProvisioningError[] provisioningErrors { get; set; }

        /// <summary>
        /// The preferred language for an Office 365 group. Should follow ISO 639-1 Code; for example "en-US".
        /// </summary>
        public string preferredLanguage { get; set; }

        /// <summary>
        /// Notes: not nullable, the any operator is required for filter expressions on multi-valued properties; for more information, see Supported Queries, Filters, and Paging Options.
        /// </summary>
        public string[] proxyAddresses { get; set; }

        /// <summary>
        /// Specifies whether the group is a security group. If the mailEnabled property is also true, the group is a mail-enabled security group; otherwise it is a security group. Only (pure) security groups can be created using Azure AD Graph. For this reason, the property must be set true when creating a group.
        /// </summary>
        public bool securityEnabled { get; set; }

        /// <summary>
        /// Specifies an Office 365 group's color theme. Possible values are Teal, Purple, Green, Blue, Pink, Orange or Red.
        /// </summary>
        public string theme { get; set; }

    }
}
