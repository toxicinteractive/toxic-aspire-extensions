using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Toxic.Aspire.Extensions;

internal class CodespaceOptionsConfigurator : IConfigureOptions<CodespaceOptions>
{
    private readonly IConfiguration _configuration;

    public CodespaceOptionsConfigurator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(CodespaceOptions options)
    {
        if (!_configuration.GetValue<bool>(CodespaceOptions.IsCodespaceConfigName, false))
        {
            options.IsCodespace = false;
            return;
        }

        options.IsCodespace = true;
        options.CodespaceName = _configuration[CodespaceOptions.CodespaceNameConfigName];
        options.PortForwardingDomain = _configuration[CodespaceOptions.PortForwardingDomainConfigName];
    }
}
