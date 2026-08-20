using System.Net.Http.Headers;
using Examples.Web.Infrastructure;
using Examples.Web.WebApi.Grpc.Inspection;
using Google.Api;
using Google.Protobuf;
using Grpc.Core;

namespace Examples.Web.WebApi.Grpc.Services.Inspection;

public partial class InspectorService
{
    public override async Task<HttpBody> Download(DownloadRequest request, ServerCallContext context)
    {
        var httpContext = context.GetHttpContext();
        var environment = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
        using var stream = GetPdfStream(environment.ContentRootPath);

        // 'attachment' means the file will be downloaded, not displayed in the browser.
        // 'inline' means the file will be displayed in the browser, not downloaded.
        var contentDisposition = new ContentDispositionHeaderValue("attachment")
        {
            FileName = GetContentDispositionFileName(request.Filename),
            FileNameStar = request.Filename?.Sanitize(),
        };

        httpContext.Response.Headers.ContentDisposition = contentDisposition.ToString();

        var httpBody = new HttpBody
        {
            ContentType = System.Net.Mime.MediaTypeNames.Application.Pdf,
            Data = await ByteString.FromStreamAsync(stream, context.CancellationToken),
        };

        return httpBody;
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

    private static string GetContentDispositionFileName(string? fileName)
    {
        const string defaultDownloadFileName = "download.pdf";

        if (string.IsNullOrWhiteSpace(fileName))
        {
            return defaultDownloadFileName;
        }

        if (fileName.Any(c => c > 0x7F))
        {
            return defaultDownloadFileName;
        }

        return fileName;
    }
}
