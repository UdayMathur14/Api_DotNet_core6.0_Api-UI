using Microsoft.AspNetCore.Mvc;
using NZWaksUI.Models;

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
            List<RegionDto> resp = new List<RegionDto>();    
            try
            {
                var client = httpClientFactory.CreateClient();

                var response = await client.GetAsync("https://localhost:7209/api/regions");

                response.EnsureSuccessStatusCode();

                //var stringResponseBody = await response.Content.ReadAsStringAsync();
                //var stringResponseBody = await response.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>();

                resp.AddRange(await response.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
                //ViewBag.response = stringResponseBody;
            }
            catch(Exception ex) 
            {

            }

            return View(resp);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
    }
}
