namespace EPS.Extensions.GraphObjects
{
    /// <summary>
    /// Allows an application to define and use a set of additional properties that can be added
    /// to directory objects (users, groups, tenant details, devices, applications, and service principals) without
    /// the application requiring an external data store. For more information about extension properties,
    /// see Directory Schema Extensions. Inherits from <see cref="DirectoryObject"/>.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/azure/ad/graph/api/entity-and-complex-type-reference#extensionproperty-entity"/>
    public class ExtensionProperty
    {

        public string appDisplayName { get; set; }

        /// <summary>
        /// Specifies the type of the directory extension property being added. Supported types are: Integer, LargeInteger, DateTime (must be specified in ISO 8601 - DateTime is stored in UTC), Binary, Boolean, and String.
        /// </summary>
        public string dataType { get; set; }

        /// <summary>
        /// Specifies the display name for the directory extension property.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Indicates whether the extension property is synced from the on premises directory.
        /// </summary>
        public bool isSyncedFromOnPremises { get; set; }

        /// <summary>
        /// The directory objects to which the directory extension property is being added. Supported directory entities that
        /// can be extended are: "User", "Group", "TenantDetail", "Device", "Application" and "ServicePrincipal"
        /// </summary>
        public string[] targetObjects { get; set; }




    }
}
