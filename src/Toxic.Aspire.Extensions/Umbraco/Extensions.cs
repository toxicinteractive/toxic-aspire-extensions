using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Microsoft.Extensions.DependencyInjection;

namespace Toxic.Aspire.Extensions;

public static class UmbracoExtensions
{
    extension<TResource>(IResourceBuilder<TResource> builder) 
        where TResource : IResourceWithEndpoints, IResourceWithEnvironment
    {
        /// <summary>
        /// Tags this resource as being an Umbraco application.
        /// This allows the application to be automatically configured for various settings such as Codespace support.
        /// </summary>
        public IResourceBuilder<TResource> WithUmbraco()
        {
            builder.WithAnnotation(new IsUmbracoAnnotation());

            if (!builder.ApplicationBuilder.ExecutionContext.IsRunMode ||
                !builder.ApplicationBuilder.ExecutionContext.IsRunningInCodespace)
            {
                return builder;
            }

            builder.OnResourceEndpointsAllocated((resource, @event, cancellationToken) =>
            {
                var urlService = @event.Services.GetService<CodespaceUrlService>();

                if (urlService != null)
                {
                    var codespaceUrl = urlService.GetCodespaceUrl(resource);

                    builder.WithEnvironment("UMBRACO__CMS__WEBROUTING__UMBRACOAPPLICATIONURL", codespaceUrl);
                    builder.WithEnvironment("UMBRACO__CMS__SECURITY__BACKOFFICEHOST", codespaceUrl);
                }

                return Task.CompletedTask;
            });

            return builder;
        }

        /// <summary>
        /// Returns true if this resource has been tagged as being an Umbraco application.
        /// </summary>
        public bool IsUmbraco() =>
            builder.Resource.HasAnnotationOfType<IsUmbracoAnnotation>();
    }
}
