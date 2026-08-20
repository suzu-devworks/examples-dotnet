namespace Examples.Web.WebApi.Grpc.Apis;

public static class DownloadApiExtensions
{
    public static RouteHandlerBuilder MapDownloadApi(this IEndpointRouteBuilder endpoints)
    {
        const string defaultDownloadFileName = "download.pdf";

        return endpoints.MapGet("/v1/files/{filename}", (string? filename, IWebHostEnvironment environment) =>
        {
            var downloadFileName = string.IsNullOrWhiteSpace(filename) ? defaultDownloadFileName : filename;
            var stream = GetPdfStream(environment.ContentRootPath);

            return Results.File(
                stream,
                contentType: System.Net.Mime.MediaTypeNames.Application.Pdf,
                fileDownloadName: downloadFileName,
                enableRangeProcessing: true);
        })
        .WithName("DownloadFile")
        .WithSummary("Download a PDF file")
        .WithDescription("Downloads the sample PDF with support for HTTP range requests.")
        .Produces<Stream>(StatusCodes.Status200OK, System.Net.Mime.MediaTypeNames.Application.Pdf)
        .Produces<Stream>(StatusCodes.Status206PartialContent, System.Net.Mime.MediaTypeNames.Application.Pdf)
        .Produces(StatusCodes.Status416RangeNotSatisfiable);
    }

    private static Stream GetPdfStream(string contentRootPath)
    {
        var sourcePath = Path.Combine(contentRootPath, "Files", "for-study.pdf");

        return new FileStream(
            sourcePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 64 * 1024,
            FileOptions.Asynchronous | FileOptions.SequentialScan);
    }
}
