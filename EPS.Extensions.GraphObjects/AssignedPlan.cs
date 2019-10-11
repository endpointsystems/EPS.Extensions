using System;

namespace EPS.Extensions.GraphObjects
{
    /// <summary>
    /// The assignedPlans property of both the User entity and the TenantDetail entity is a collection of AssignedPlan.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/azure/ad/graph/api/entity-and-complex-type-reference#assignedplan-type"/>
    public class AssignedPlan
    {
        /// <summary>
        /// The date and time at which the plan was assigned; for example: 2013-01-02T19:32:30Z.
        /// </summary>
        public DateTime assignedTimestamp { get; set; }

        /// <summary>
        /// For example, "Enabled".
        /// </summary>
        public string capabilityStatus { get; set; }

        /// <summary>
        /// The name of the service; for example, "SharePoint", "MicrosoftOffice", or "Exchange".
        /// </summary>
        public string service { get; set; }

        /// <summary>
        /// A GUID that identifies the service plan.
        /// </summary>
        public string servicePlanId { get; set; }

    }
}
