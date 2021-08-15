using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Puppeteer_Sharp.Models;
using PuppeteerSharp;

namespace Puppeteer_Sharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PdfController : ControllerBase
    {
        [HttpPost]
        public async Task<byte[]> GetPdfData([FromBody] PDFRequest request)
        {
            var html = request.html;
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions {
                Headless = true,
                Args = new string[] { "--no-sandbox" }
            });
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(!string.IsNullOrWhiteSpace(html) ? html : "<div>My Receipt</div>");

            var bytes = await page.PdfDataAsync();
            System.IO.File.WriteAllBytes("output.pdf", bytes);
            return bytes;
        }
    }
}
