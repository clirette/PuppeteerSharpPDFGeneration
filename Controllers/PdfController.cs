using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;

namespace Puppeteer_Sharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PdfController : ControllerBase
    {
        [HttpGet]
        public async Task<byte[]> GetPdfData(string html)
        {
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions {Headless = true});
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(!string.IsNullOrWhiteSpace(html) ? html : "<div>My Receipt</div>");

            var bytes = await page.PdfDataAsync();
            System.IO.File.WriteAllBytes("output.pdf", bytes);
            return bytes;
        }
    }
}
