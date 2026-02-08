using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace BlueBerry24.API.OpenApi.Transformers;

public sealed class VersionInfoTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var version = context.DocumentName;

        document.Info.Version = version;
        document.Info.Title = $"BlueBerry24 API {version}";

        return Task.CompletedTask;
    }
}