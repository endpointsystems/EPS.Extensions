namespace EPS.Extensions.GraphObjects
{
    /// <summary>
    /// The provisionedPlans property of the <see cref="User"/> entity and the TenantDetail entity is a
    /// collection of ProvisionedPlan.
    /// </summary>
    public class ProvisionedPlan
    {
        /// <summary>
        /// For example, "Enabled" or "Deleted".
        /// </summary>
        public string capabilityStatus { get; set; }

        /// <summary>
        /// For example, "Success".
        /// </summary>
        public string provisioningStatus { get; set; }

        /// <summary>
        /// The name of the service; for example, "SharePoint", "MicrosoftOffice", or "Exchange".
        /// </summary>
        public string service { get; set; }
    }
}
