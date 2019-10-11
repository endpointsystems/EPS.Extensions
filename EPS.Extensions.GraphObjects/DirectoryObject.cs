using System;

namespace EPS.Extensions.GraphObjects
{
    /// <summary>
    /// Represents an Azure Active Directory object. The DirectoryObject type
    /// is the base type for most of the other directory entity types.
    /// </summary>
    public class DirectoryObject
    {
        /// <summary>
        /// The time at which the directory object was deleted. It only applies to those directory
        /// objects which can be restored. Currently it is only supported for deleted Application objects;
        /// all other entities return null for this property.
        /// </summary>
        public DateTime deletionTimestamp { get; set; }

        /// <summary>
        /// A Guid that is the unique identifier for the object; for example, 12345678-9abc-def0-1234-56789abcde.
        /// </summary>
        /// <remarks>
        /// Notes: key, immutable, not nullable, unique.
        /// </remarks>
        public string objectId { get; set; }

        /// <summary>
        /// A string that identifies the object type. For example, for groups the value is always "Group".
        /// </summary>
        public string objectType { get; set; }

//        /// <summary>
//        /// The directory objects that were created by the current object. Read only. Requires version 2013-11-08 or newer.
//        /// </summary>
//        public DirectoryObject createdObjects { get; set; }
//
//        /// <summary>
//        /// The directory object that that this object was created on behalf of. Read only. Requires version 2013-11-08 or newer
//        /// </summary>
//        public DirectoryObject createdOnBehalfOf { get; set; }
//
//        public DirectoryObject manager { get; set; }


    }
}
