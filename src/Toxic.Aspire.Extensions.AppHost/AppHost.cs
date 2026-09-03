using Azure.Provisioning.AppContainers;
using Toxic.Aspire.Extensions;

var builder = DistributedApplication
    .CreateBuilder(args)
    .WithCodespaces();

builder
    .AddAzureContainerAppEnvironment("app-env")
    .ExcludeFromDevelopmentManifest();

builder
    .AddProject<Projects.Toxic_Aspire_Extensions_SampleApp>("sample-app")
    .WithExternalHttpEndpoints()
    .WithUrlForEndpoint("https", cfg => cfg.Url = $"{cfg.Url}/weatherforecast")
    .WithNewRelicTelemetry("apikey", "entity", includeInDevelopmentEnvironment: true)
    .WithUmbraco()
    .PublishAsAzureContainerApp((infra, app) =>
    {
        var resource = infra.GetProvisionableResource<ContainerApp>();
        resource.Template.Scale.MinReplicas = 0;
        resource.Template.Scale.MaxReplicas = 1;
    })
    .ExcludeFromDevelopmentManifest();

builder
    .Build()
    .Run();
