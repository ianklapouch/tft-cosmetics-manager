using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using tft_cosmetics_manager.Models;
using tft_cosmetics_manager.Services;
using System.Text;

namespace tft_cosmetics_manager.ViewModels
{
    public class MapSkinViewModel
    {
        public readonly List<MapSkin> mapSkins = new();
        public async Task<bool> LoadMapSkins()
        {
            bool hasLoadedIds = await LoadMapSkinsIdsAsync().ConfigureAwait(false);
            bool hasLoadedImagesPaths = false;

            if (hasLoadedIds)
            {
                hasLoadedImagesPaths = await LoadMapSkinsImagesPathsAsync().ConfigureAwait(false);
            }

            return hasLoadedIds && hasLoadedImagesPaths;
        }

        private async Task<bool> LoadMapSkinsIdsAsync()
        {
            string url = $"{App.BaseUrl}/lol-inventory/v1/inventory?inventoryTypes=%5B%22TFT_MAP_SKIN%22%5D";
            string auth = App.Auth;

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
                MapSkin mapSkin = new()
                {
                    ItemId = item.ItemId
                };
                mapSkins.Add(mapSkin);
            }

            return true;
        }

        private async Task<bool> LoadMapSkinsImagesPathsAsync()
        {
            using HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/tftmapskins.json");

            if (!response.IsSuccessStatusCode)
                return false;

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var jsonObjects = JsonConvert.DeserializeObject<List<MapSkin>>(jsonResponse);


            foreach (MapSkin mapSkin in mapSkins)
            {
                MapSkin responseObj = jsonObjects.FirstOrDefault(obj => obj.ItemId == mapSkin.ItemId);
                if (responseObj != null)
                {
                    mapSkin.LoadoutsIcon = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/" + responseObj.LoadoutsIcon.Replace("/lol-game-data/assets/", "").ToLower();
                }
            }

            return true;
        }

        public async Task<bool> SetMapSkin(string id = "")
        {
            string selectedId;
            if (string.IsNullOrEmpty(id))
            {
                Random random = new();
                int randomIndex = random.Next(0, mapSkins.Count);
                selectedId = mapSkins[randomIndex].ItemId.ToString();
            }
            else
            {
                selectedId = id;
            }

            string url = $"{App.BaseUrl}/lol-cosmetics/v1/selection/tft-map-skin";
            string auth = App.Auth;

            using HttpClientHandler handler = new();
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;

            using HttpClient client = new(handler);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            client.DefaultRequestHeaders.Add("ContentType", "application/json");

            try
            {
                byte[] data = Encoding.ASCII.GetBytes(selectedId);
                using var content = new ByteArrayContent(data);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PutAsync(url, content);
                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Request successful.");
                return true; // Retorna true para indicar sucesso
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Retorna false para indicar erro
            }
        }
    }
}
