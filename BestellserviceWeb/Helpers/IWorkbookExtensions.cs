using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using NPOI.SS.UserModel;
using NPOI.XWPF.UserModel;
using PdfSharpCore.Pdf;

namespace BestellserviceWeb.Helpers
{
    public static class IWorkBookExtensions
    {

        public static void WriteExcelToResponse(this IWorkbook book, HttpContext httpContext, string templateName)
        {
            var response = httpContext.Response;
            response.ContentType = "application/vnd.ms-excel";
            if (!string.IsNullOrEmpty(templateName))
            {
                var contentDisposition = new Microsoft.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                contentDisposition.SetHttpFileName(templateName);
                response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
            }
            book.Write(response.Body);
        }

        public static void WriteWordToResponse(this XWPFDocument book, HttpContext httpContext, string templateName)
        {
            var response = httpContext.Response;
            response.ContentType = "APPLICATION/octet-stream";
            if (!string.IsNullOrEmpty(templateName))
            {
                var contentDisposition = new Microsoft.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                contentDisposition.SetHttpFileName(templateName);
                response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
            }
            book.Write(response.Body);
        }

       public static void WritePdfToResponse(this PdfDocument book, HttpContext httpContext, string templateName)
        {
            var response = httpContext.Response;
            response.ContentType = "APPLICATION/octet-stream";
            if (!string.IsNullOrEmpty(templateName))
            {
                var contentDisposition = new Microsoft.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                contentDisposition.SetHttpFileName(templateName);
                response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
            }
            book.Save(response.Body);
        }

    }
}
