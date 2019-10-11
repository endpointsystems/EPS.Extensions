namespace EPS.Extensions.GraphObjects
{
    /// <summary>
    /// Represents a directory role template. A directory role template specifies the
    /// property values of a directory role (<see cref="DirectoryRole"/>). There is an associated
    /// directory role template object for each of the directory roles that
    /// may be activated in a tenant. Inherits from <see cref="DirectoryObject"/>.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/previous-versions/azure/ad/graph/api/entity-and-complex-type-reference#directoryroletemplate-entity"/>
    public class DirectoryRoleTemplate: DirectoryObject
    {

        /// <summary>
        /// The description to set for the directory role.
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// The display name to set for the directory role.
        /// </summary>
        public string displayName { get; set; }


    }
}
