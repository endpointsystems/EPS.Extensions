using System;
using System.Security.Principal;
using Newtonsoft.Json.Linq;

namespace EPS.Extensions.GraphObjects
{
    /// <summary>
    /// Represents an organizational contact. Inherits from <see cref="DirectoryObject"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Organizational contacts represent users that are not in your company directory. They are mail-enabled entities
    /// and typically represent individuals who are external to your company or organization. Organizational contacts
    /// cannot be authenticated using Azure AD, nor can they be assigned licenses.
    /// </para>
    /// <para>
    /// Organizational contacts can be created in your tenant through syncing with an on-premises directory using
    /// Azure AD Connect, or they can be created through one of the Exchange Online management portals or the Exchange
    /// Online PowerShell cmdlets. For more information about Azure AD Connect, see Integrating your on-premises
    /// identities with Azure Active Directory. For more information about Exchange Online management tools,
    /// see Exchange Online Setup and Administration.
    /// </para>
    /// <para>
    /// You cannot create organizational contacts with the Graph API. You can, however, update and delete contacts
    /// that are not currently synced from an on-premises directory; that is, contacts for which the dirSyncEnabled
    /// property is null or false. You cannot update or delete contacts for which the dirSyncEnabled property is true.
    /// </para>
    /// <para>
    /// Organizational contacts are directory entities, which represent external users. They should not be confused
    /// with O365 Outlook Personal contacts.
    /// </para>
    /// </remarks>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/azure/ad/graph/api/entity-and-complex-type-reference#contact-entity"/>
    public class Contact: DirectoryObject
    {
        /// <summary>
        /// The city in which the contact is located.
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// The country/region in which the contact is located.
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// The name for the department in which the contact works.
        /// </summary>
        public string department { get; set; }

        /// <summary>
        /// true if this object is synced from an on-premises directory; false if this object was originally synced from an on-premises directory but is no longer synced; null if this object has never been synced from an on-premises directory (default).
        /// </summary>
        public bool dirSyncEnabled { get; set; }

        /// <summary>
        /// The display name for the contact.
        /// </summary>
        public string displayName { get; set; }

        /// <summary>
        /// The telephone number of the contact's business fax machine.
        /// </summary>
        public string facsimileTelephoneNumber { get; set; }

        /// <summary>
        /// The given name (first name) of the contact.
        /// </summary>
        public string givenName { get; set; }

        /// <summary>
        /// The contact's job title.
        /// </summary>
        public string jobTitle { get; set; }

        /// <summary>
        /// Indicates the last time at which the object was synced with the on-premises directory.
        /// </summary>
        public DateTime lastDirSyncTime { get; set; }

        /// <summary>
        /// The SMTP address for the contact, for example, "jeff@contoso.onmicrosoft.com".
        /// </summary>
        public string mail { get; set; }

        /// <summary>
        /// The mail alias for the contact.
        /// </summary>
        public string mailNickname { get; set; }

        /// <summary>
        /// The primary cellular telephone number for the contact.
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// The office location in the contact's place of business.
        /// </summary>
        public string physicalDeliveryOfficeName { get; set; }

        /// <summary>
        /// The postal code for the contact's postal address. The postal code is specific to the contact's country/region. In the United States of America, this attribute contains the ZIP code.
        /// </summary>
        public string postalCode { get; set; }

        public ProvisioningError[] provisioningErrors { get; set; }

        public string[] proxyAddresses { get; set; }

        /// <summary>
        /// Specifies the voice over IP (VOIP) session initiation protocol (SIP) address for the contact.
        /// </summary>
        public string sipProxyAddress { get; set; }

        /// <summary>
        /// The state or province in the contact's address.
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// The street address of the contact's place of business.
        /// </summary>
        public string streetAddress { get; set; }

        /// <summary>
        /// The contact's surname (family name or last name).
        /// </summary>
        public string surname { get; set; }

        /// <summary>
        /// The primary telephone number of the contact's place of business.
        /// </summary>
        public string telephoneNumber { get; set; }

        /// <summary>
        /// A thumbnail photo to be displayed for the contact.
        /// </summary>
        public string thumbnailPhoto { get; set; }

    }
}
