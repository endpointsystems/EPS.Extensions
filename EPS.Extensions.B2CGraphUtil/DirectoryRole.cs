
//https://docs.microsoft.com/en-us/previous-versions/azure/ad/graph/api/entity-and-complex-type-reference#directoryobject-entity
namespace EPS.Extensions.B2CGraphUtil
{
    /// <summary>
    /// Represents an Azure AD directory role. Azure AD directory roles are also known as
    /// administrator roles. For more information about directory (administrator) roles, see
    /// <see href="http://azure.microsoft.com/documentation/articles/active-directory-assign-admin-roles/"/>.
    /// </summary>
    public class DirectoryRole: DirectoryObject
    {

        /// <summary>
        /// The description for the directory role.
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// The display name for the directory role.
        /// </summary>
        public string displayName { get; set; }

        /// <summary>
        /// true if the role is a system role; otherwise, false.
        /// </summary>
        public bool isSystem { get; set; }

        /// <summary>
        /// true if the directory role is disabled; otherwise, false.
        /// </summary>
        public bool roleDisabled { get; set; }

        /// <summary>
        /// he objectId of the <see cref="DirectoryRoleTemplate"/> that this role is based on.
        /// </summary>
        public string roleTemplateId { get; set; }





    }
}
