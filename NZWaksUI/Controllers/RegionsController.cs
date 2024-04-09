using Azure;
using Microsoft.AspNetCore.Mvc;
using NZWaksUI.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

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
            catch (Exception ex)
            {

            }

            return View(resp);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddRegionClass model)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7209/api/Regions/postRegions"),

                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "Application/json")
            };

            var responseMessage = await client.SendAsync(httpRequestMessage);

            responseMessage.EnsureSuccessStatusCode();

            var response = await responseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response != null)
            {
                return RedirectToAction("Index");
            }

            return View();

        }

        [HttpGet]

        public async Task<IActionResult> Edit(Guid id)
        {
            //ViewBag.Id = id;
            var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7209/api/regions/{id.ToString()}");

            if (response != null)
            {
                return View(response);
            }

            return View(null);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(RegionDto regionDto)
        {
            var client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7209/api/Regions/{regionDto.Id}"),

                Content = new StringContent(JsonSerializer.Serialize(regionDto), Encoding.UTF8, "Application/json")
            };
            var responseMessage = await client.SendAsync(request);

            responseMessage.EnsureSuccessStatusCode();

            var response = await responseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response != null)
            {
                return RedirectToAction("Edit", "Regions");
            }
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Delete(RegionDto regionDto)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var responseMessage = await client.DeleteAsync($"https://localhost:7209/api/Regions/{regionDto.Id}");

                responseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");
            }
            catch (Exception ex)
            {

            }

            return View("Edit");
        }
    }
}