using Aspire.Hosting.ApplicationModel;
using Microsoft.Extensions.Options;

namespace Toxic.Aspire.Extensions;

/// <summary>
/// Used to build a URL for a resource running in a GitHub Codespace.
/// </summary>
public class CodespaceUrlService
{
    private readonly IOptions<CodespaceOptions> _options;

    public CodespaceUrlService(IOptions<CodespaceOptions> options)
    {
        _options = options;
    }

    /// <summary>
    /// Gets the URL to the resource in the currently running GitHub Codespace, or null if the application isn't running in one.
    /// </summary>
    public string? GetCodespaceUrl(IResourceWithEndpoints resource, string endpointName = "https")
    {
        if (!_options.Value.IsCodespace)
        {
            return null;
        }

        var endpoint = resource.GetEndpoint(endpointName);
        var url = new Uri(endpoint.Url);

        return $"{url.Scheme}://{_options.Value.CodespaceName}-{url.Port}.{_options.Value.PortForwardingDomain}{url.AbsolutePath}{url.Query}";
    }
}
