using System;
using System.Security.Principal;
using Newtonsoft.Json.Linq;

namespace EPS.Extensions.GraphObjects
{
    /// <summary>
    /// Represents an Azure AD user account. Inherits from DirectoryObject.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/azure/ad/graph/api/entity-and-complex-type-reference#user-entity"/>
    public class User: DirectoryObject
    {

        /// <summary>
        /// true if the account is enabled; otherwise, false. This property is required when a user is created.
        /// </summary>
        public bool accountEnabled { get; set; }

        /// <summary>
        /// The licenses that are assigned to the user.
        /// </summary>
        public AssignedLicense[] assignedLicenses { get; set; }


        /// <summary>
        /// The plans that are assigned to the user.
        /// </summary>
        public AssignedPlan[] assignedPlans { get; set; }

        /// <summary>
        /// The city in which the user is located.
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// The country/region in which the user is located; for example, "US" or "UK".
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// Indicates whether the user account is a local account for an Azure Active Directory B2C tenant. Possible
        /// values are "LocalAccount" and null. When creating a local account, the property is required
        /// and you must set it to "LocalAccount". When creating a work or school account, do not specify the
        /// property or set it to null. For more information about Azure Active Directory B2C, see the
        /// Azure Active Directory B2C documentation.
        /// </summary>
        public string creationType { get; set; }

        /// <summary>
        /// The name for the department in which the user works.
        /// </summary>
        public string department { get; set; }

        /// <summary>
        /// true if this object is synced from an on-premises directory; false if this object was originally synced from an on-premises directory but is no longer synced; null if this object has never been synced from an on-premises directory (default).
        /// </summary>
        public bool? dirSyncEnabled { get; set; }

        /// <summary>
        /// The name displayed in the address book for the user. This is usually the combination of the user's first name, middle initial and last name. This property is required when a user is created and it cannot be cleared during updates.
        /// </summary>
        public string displayName { get; set; }

        /// <summary>
        /// The employee identifier assigned to the user by the organization.
        /// </summary>
        public string employeeId { get; set; }

        /// <summary>
        /// The telephone number of the user's business fax machine.
        /// </summary>
        public string facsimileTelephoneNumber { get; set; }

        /// <summary>
        /// The given name (first name) of the user.
        /// </summary>
        public string givenName { get; set; }

        /// <summary>
        /// This property is used to associate an on-premises Active Directory user account to their Azure AD user object. This property must be specified when creating a new user account in the Graph if you are using a federated domain for the user's userPrincipalName (UPN) property.
        /// Important: The $ and _ characters cannot be used when specifying this property.
        /// </summary>
        public string immutableId { get; set; }

        /// <summary>
        /// user's job title.
        /// </summary>
        public string jobTitle { get; set; }

        /// <summary>
        /// Indicates the last time at which the object was synced with the on-premises directory; for example: "2013-02-16T03:04:54Z"
        /// </summary>
        public DateTime lastDirSyncTime { get; set; }

        /// <summary>
        /// The SMTP address for the user, for example, "jeff@contoso.onmicrosoft.com".
        /// </summary>
        public string mail { get; set; }

        /// <summary>
        /// The mail alias for the user. This property is required when you create a work or school account; it is optional for a local account.
        /// </summary>
        public string mailNickname { get; set; }

        /// <summary>
        /// the user's mobile number.
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// Contains the on-premises security identifier (SID) for the user that was synchronized from on-premises to the cloud.
        /// </summary>
        public string onPremisesSecurityIdentifier { get; set; }

        /// <summary>
        /// A list of additional email addresses for the user; for example: ["bob@contoso.com", "Robert@fabrikam.com"].
        /// </summary>
        public string[] otherMails { get; set; }

        /// <summary>
        /// Specifies password policies for the user. This value is an enumeration with one possible value being "DisableStrongPassword", which allows weaker passwords than the default policy to be specified. "DisablePasswordExpiration" can also be specified. The two may be specified together; for example: "DisablePasswordExpiration, DisableStrongPassword".
        /// </summary>
        public string passwordPolicies { get; set; }

        /// <summary>
        /// The office location in the user's place of business.
        /// </summary>
        public string physicalDeliveryOfficeName { get; set; }

        /// <summary>
        /// The postal code for the user's postal address. The postal code is specific to the user's country/region. In the United States of America, this attribute contains the ZIP code.
        /// </summary>
        public string postalCode { get; set; }

        /// <summary>
        /// The preferred language for the user. Should follow ISO 639-1 Code; for example "en-US".
        /// </summary>
        public string preferredLanguage { get; set; }

        /// <summary>
        /// The plans that are provisioned for the user.
        /// </summary>
        public ProvisionedPlan[] provisionedPlans { get; set; }

        /// <summary>
        /// Fpr example: ["SMTP: bob@contoso.com", "smtp: bob@sales.contoso.com"]
        /// </summary>
        public string[] proxyAddresses { get; set; }

        /// <summary>
        /// Any refresh tokens or sessions tokens (session cookies) issued before this time are invalid, and applications will get an error when using an invalid refresh or sessions token to acquire a delegated access token (to access APIs such as AD Graph). If this happens, the application will need to acquire a new refresh token by making a request to the authorize endpoint. Use Invalidate all refresh tokens to reset.
        /// </summary>
        public DateTime refreshTokensValidFromDateTime { get; set; }

        /// <summary>
        /// true if the Outlook global address list should contain this user, otherwise false. If not set, this will be treated as true. For users invited through the invitation manager, this property will be set to false.
        /// </summary>
        public bool showInAddressList { get; set; }

        /// <summary>
        /// Specifies the collection of sign-in names for a local account in an Azure Active Directory B2C tenant. Each sign-in name must be unique in the tenant. The property must be specified when you create a local account user; do not specify it when you create a work or school account. For more information about Azure Active Directory B2C, see the Azure Active Directory B2C documentation.
        /// </summary>
        public SignInName[] signInNames { get; set; }

        /// <summary>
        /// Specifies the voice over IP (VOIP) session initiation protocol (SIP) address for the user.
        /// </summary>
        public string sipProxyAddress { get; set; }

        /// <summary>
        /// The state or province in the user's address.
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// The street address of the user's place of business.
        /// </summary>
        public string streetAddress { get; set; }

        /// <summary>
        /// The user's surname (family name or last name).
        /// </summary>
        public string surname { get; set; }

        /// <summary>
        /// The primary telephone number of the user's place of business.
        /// </summary>
        public string telephoneNumber { get; set; }

        /// <summary>
        /// A thumbnail photo to be displayed for the user.
        /// </summary>
        public string thumbnailPhoto { get; set; }

        /// <summary>
        /// A two letter country code (ISO standard 3166). Required for users that will be assigned licenses due to legal requirement to check for availability of services in countries. Examples include: "US", "JP", and "GB".
        /// </summary>
        public string usageLocation { get; set; }

        /// <summary>
        /// Specifies the collection of userIdentities for a social user account in an Azure Active Directory B2C tenant. Each userIdentity (issuer and issuerIdentity as a pair) must be unique in the tenant. For more information about Azure Active Directory B2C, see the Azure Active Directory B2C documentation.
        /// </summary>
        public UserIdentity[] userIdentities { get; set; }

        /// <summary>
        /// The user principal name (UPN) of the user. The UPN is an Internet-style login name for the user based on the Internet standard RFC 822. By convention, this should map to the user's email name. The general format is "alias@domain". For work or school accounts, the domain must be present in the tenant's collection of verified domains. This property is required when a work or school account is created; it is optional for local accounts.
        /// The verified domains for the tenant can be accessed from the VerifiedDomains property of TenantDetail.
        /// </summary>
        public string userPrincipalName { get; set; }

        /// <summary>
        /// A string value that can be used to classify user types in your directory, such as "Member" and "Guest".
        /// </summary>
        public string userType { get; set; }

    }
}
