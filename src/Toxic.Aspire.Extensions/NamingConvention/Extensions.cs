using Aspire.Hosting;
using Azure.Provisioning.Primitives;
using Toxic.Aspire.Extensions.NamingConvention;

namespace Toxic.Aspire.Extensions;

public static class NamingConventionExtensions
{
    extension(IDistributedApplicationBuilder builder)
    {
        /// <summary>
        /// Returns a standardized Azure resource name according to the following convention: prefix-project-workload-environment.
        /// Example: app-lrf-cms-stage, kv-toxic-serviceportal-prod.
        /// Will throw an exception if the prefix for this resource is not known. You can use the overloaded method to specify it instead.
        /// </summary>
        /// <param name="projectName">The project or client abbreviated name, e.g. "lrf" or "toxic".</param>
        /// <param name="workloadName">What the resource does, e.g. "cms" or "api". Can be left out.</param>
        public string GetAzureResourceName<TResource>(string projectName, string? workloadName = null)
            where TResource : ProvisionableResource
        {
            if (!ResourcePrefixes.KnownTypePrefixes.TryGetValue(typeof(TResource), out var resourcePrefix))
            {
                throw new InvalidOperationException($"Couldn't find a resource prefix for {typeof(TResource).Name}, specify it explicitly instead.");
            }

            return builder.GetAzureResourceName(resourcePrefix, projectName, workloadName);
        }

        /// <summary>
        /// Returns a standardized Azure resource name according to the following convention: prefix-project-workload-environment.
        /// Example: app-lrf-cms-stage, kv-toxic-serviceportal-prod.
        /// </summary>
        /// <param name="resourcePrefix">Resource abbreviation, e.g. "app" or "kv".</param>
        /// <param name="projectName">The project or client abbreviated name, e.g. "lrf" or "toxic".</param>
        /// <param name="workloadName">What the resource does, e.g. "cms" or "api". Can be left out.</param>
        public string GetAzureResourceName(string resourcePrefix, string projectName, string? workloadName)
        {
            var environmentName = builder.Environment.EnvironmentName.ToLowerInvariant() switch
            {
                "development" => "dev",
                "staging" => "stage",
                "production" => "prod",
                _ => null
            };

            return string.Join(
                '-', 
                new string?[] 
                {
                    resourcePrefix,
                    projectName,
                    workloadName,
                    environmentName
                }
                .Where(x => !string.IsNullOrWhiteSpace(x)));
        }
    }
}
