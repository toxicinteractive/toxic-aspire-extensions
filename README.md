# Toxic.Aspire.Extensions
Provides common extensions for Aspire projects. 

## How to use
1. Add the `Toxic.Aspire.Extensions` nuget to your AppHost project. The major version number of the package version aligns with the corresponding Aspire version.
2. Add `using Toxic.Aspire.Extensions`.

## Included
### IDistributedApplicationBuilder.GetAzureResourceName()
Returns a standardized Azure resource name according to the following convention: prefix-project-workload-environment (example: app-lrf-cms-stage, kv-toxic-serviceportal-prod).

Use it inside `ConfigureInfrastructure` or `PublishAsXxx` - see the example AppHost project in the repo for more details.

### IDistributedApplicationBuilder.WithCodespaces()
Adds a `CodespaceUrlService` and `CodespaceOptions` to the service provider that can provide information about the Codespace if Aspire is running within one.

### ExecutionContext.IsRunningInCodespace
Returns true if Aspire is running inside a GitHub Codespace.

### IResourceBuilder.WithNewRelicTelemetry()
Redirects OTEL telemetry to a New Relic instance.

### IResourceBuilder.WithUmbraco()
Tags a resource as an Umbraco application which has the following effects:

* Rewrites the application and backoffice URL:s if Aspire is running inside a GitHub Codespace (requires calling `DistributedApplicationBuilder.WithCodespaces()`)

You can check if a resource has been tagged as an Umbraco application in your own extensions with `IsUmbraco()`.

### AzureResourceInfrastructure.GetProvisionableResource<>()
Shortcut for the ubiquitous

```csharp
infra.GetProvisionableResources()
     .OfType<TResource>()
     .Single();
```

### ResourceBuilder.ExcludeFromManifest()
### ResourceBuilder.ExcludeFromDevelopmentManifest()
### ResourceBuilder.ExcludeFromStagingManifest()
### ResourceBuilder.ExcludeFromProductionManifest()
Shortcuts for excluding a resource from being published in certain environments.
