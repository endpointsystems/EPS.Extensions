using System;

namespace EPS.Extensions.B2CGraphUtil
{
    /// <summary>
    /// The provisioningErrors property of the Contact, User, and Group entities is a collection of ProvisioningError.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/azure/ad/graph/api/entity-and-complex-type-reference#provisioningerror-type"/>
    public class ProvisioningError
    {
        /// <summary>
        /// A description of the error
        /// </summary>
        public string errorDetail { get; set; }

        /// <summary>
        /// true if the error was resolved; otherwise, false.
        /// </summary>
        public bool resolved { get; set; }

        /// <summary>
        /// The service instance for which the error occurred.
        /// </summary>
        public string serviceInstance { get; set; }

        /// <summary>
        /// The date and time at which the error occurred.
        /// </summary>
        public DateTime timestamp { get; set; }



    }
}
