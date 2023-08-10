using Microsoft.AspNetCore.Mvc;

namespace NZWaksUI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var response = await client.GetAsync("https://localhost:7209/api/regions");

                response.EnsureSuccessStatusCode();

                var stringResponseBody = await response.Content.ReadAsStringAsync();

                ViewBag.response = stringResponseBody;
            }
            catch(Exception ex) 
            {

            }

            return View();
        }
    }
}
