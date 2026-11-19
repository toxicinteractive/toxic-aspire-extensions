using Azure.Provisioning.AppContainers;
using Toxic.Aspire.Extensions;

var builder = DistributedApplication
    .CreateBuilder(args)
    .WithCodespaces();

builder
    .AddAzureContainerAppEnvironment("app-env")
    .ExcludeFromDevelopmentManifest();

builder
    .AddDotnetProject("sample-app", "../Toxic.Aspire.Extensions.SampleApp")
    .WithExternalHttpEndpoints()
    .WithUrlForEndpoint("https", cfg => cfg.Url = $"{cfg.Url}/weatherforecast")
    .WithNewRelicTelemetry("apikey", "entity", includeInDevelopmentEnvironment: true)
    .WithUmbraco()
    .PublishAsAzureContainerApp((infra, app) =>
    {
        var resource = infra.GetProvisionableResource<ContainerApp>();

        // will output "ca-lrf-cms-stage" when running "aspire publish --environment Staging"
        resource.Name = builder.GetAzureResourceName(resource, "lrf", "cms");
        
        resource.Template.Scale.MinReplicas = 0;
        resource.Template.Scale.MaxReplicas = 1;
    })
    .ExcludeFromDevelopmentManifest();

builder
    .Build()
    .Run();
