using Aspire.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Toxic.Aspire.Extensions;

/// <summary>
/// Extensions for the execution context.
/// </summary>
public static class CodespaceExtensions
{
    extension(IDistributedApplicationBuilder builder)
    {
        /// <summary>
        /// Adds services to support GitHub Codespace URL rewriting.
        /// </summary>
        public IDistributedApplicationBuilder WithCodespaces()
        {
            builder.Services
                .ConfigureOptions<CodespaceOptionsConfigurator>()
                .AddSingleton<CodespaceUrlService>();

            return builder;
        }
    }

    extension(DistributedApplicationExecutionContext context)
    {
        /// <summary>
        /// Returns true if the application is running in a GitHub Codespace.
        /// </summary>
        public bool IsRunningInCodespace => bool.TryParse(
            Environment.GetEnvironmentVariable(CodespaceOptions.IsCodespaceConfigName), out var val) && val;
    }
}
