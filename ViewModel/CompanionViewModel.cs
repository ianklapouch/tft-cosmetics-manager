using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tft_cosmetics_manager.Models;
using tft_cosmetics_manager.Services;
using System.Net.Http;
using System.Net.Http.Headers;

namespace tft_cosmetics_manager.ViewModel
{
    public class CompanionViewModel
    {
        private readonly IApiConfigService apiConfigService;
        public readonly List<Companion> companions = new();
        public CompanionViewModel(IApiConfigService apiConfigService)
        {
            this.apiConfigService = apiConfigService;
        }
        public async Task<bool> LoadCompanions()
        {
            bool hasLoadedIds = await LoadCompanionsIdsAsync().ConfigureAwait(false);
            bool hasLoadedImagesPaths = false;

            if (hasLoadedIds)
            {
                hasLoadedImagesPaths = await LoadCompanionsImagesPathsAsync().ConfigureAwait(false);
            }

            return hasLoadedIds && hasLoadedImagesPaths;
        }
        private async Task<bool> LoadCompanionsIdsAsync()
        {
            string url = $"{apiConfigService.BaseUrl}/lol-inventory/v1/inventory?inventoryTypes=%5B%22COMPANION%22%5D";
            string auth = apiConfigService.Auth;

            using HttpClientHandler handler = new();
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;

            using HttpClient client = new(handler);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            client.DefaultRequestHeaders.Add("ContentType", "application/json");


            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var items = JsonConvert.DeserializeObject<List<Response>>(jsonResponse);
            foreach (var item in items)
            {
                Companion companion = new()
                {
                    ItemId = item.ItemId
                };
                companions.Add(companion);
            }

            return true;
        }
        private async Task<bool> LoadCompanionsImagesPathsAsync()
        {
            using HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/companions.json");

            if (!response.IsSuccessStatusCode)
                return false;

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var jsonObjects = JsonConvert.DeserializeObject<List<Companion>>(jsonResponse);


            foreach (Companion companion in companions)
            {
                Companion responseObj = jsonObjects.FirstOrDefault(obj => obj.ItemId == companion.ItemId);
                if (responseObj != null)
                {
                    companion.LoadoutsIcon = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/" + responseObj.LoadoutsIcon.Replace("/lol-game-data/assets/", "").ToLower();
                }
            }

            return true;
        }
    }
}
