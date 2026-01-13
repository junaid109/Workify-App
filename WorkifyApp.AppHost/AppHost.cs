var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .AddDatabase("workify-db");

var redis = builder.AddRedis("redis")
    .WithDataVolume();

var apiService = builder.AddProject<Projects.WorkifyApp_ApiService>("apiservice")
    .WithReference(postgres)
    .WithReference(redis)
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.WorkifyApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WithReference(redis) // Web might need cache too
    .WaitFor(apiService);

builder.Build().Run();
