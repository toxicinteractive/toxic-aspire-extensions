using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Azure;
using Azure.Provisioning.Primitives;
using Microsoft.Extensions.Hosting;

namespace Toxic.Aspire.Extensions;

public static class Extensions
{
    extension<TResource>(IResourceBuilder<TResource> builder) where TResource : IResource
    {
        /// <summary>
        /// Excludes the resource from the Aspire provisioning manifest in Production publish mode.
        /// </summary>
        public IResourceBuilder<TResource> ExcludeFromProductionManifest() =>
            builder.ExcludeFromManifest([Environments.Production]);

        /// <summary>
        /// Excludes the resource from the Aspire provisioning manifest in Staging publish mode.
        /// </summary>
        public IResourceBuilder<TResource> ExcludeFromStageManifest() =>
            builder.ExcludeFromManifest([Environments.Staging]);

        /// <summary>
        /// Excludes the resource from the Aspire provisioning manifest in Development publish mode.
        /// </summary>
        public IResourceBuilder<TResource> ExcludeFromDevelopmentManifest() =>
            builder.ExcludeFromManifest([Environments.Development]);

        /// <summary>
        /// Excludes the resource from the Aspire provisioning manifest in publish mode if an environment name is matched.
        /// Use this to not provision certain resources to Azure in development mode for example.
        /// </summary>
        public IResourceBuilder<TResource> ExcludeFromManifest(IEnumerable<string> environmentNames)
        {
            if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode &&
                environmentNames.Any(x => string.Equals(
                    x,
                    builder.ApplicationBuilder.Environment.EnvironmentName,
                    StringComparison.OrdinalIgnoreCase)))
            {
                builder.ExcludeFromManifest();
            }

            return builder;
        }
    }

    extension(AzureResourceInfrastructure infrastructure)
    {
        /// <summary>
        /// Shortcut for getting a resource from <see cref="Azure.Provisioning.Infrastructure.GetProvisionableResources"/>.
        /// Assumes only a single resource of the type exists.
        /// </summary>
        public TResource GetProvisionableResource<TResource>() where TResource : ProvisionableResource
        {
            return infrastructure
                .GetProvisionableResources()
                .OfType<TResource>()
                .Single();
        }
    }
}
