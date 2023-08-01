using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tft_cosmetics_manager.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Text;

namespace tft_cosmetics_manager.Services
{
    public class CompanionService
    {
        public static List<Companion> Companions { get; } = new();

        public static async Task<bool> FetchCompanions()
        {
            bool hasFetchedIds = await FetchCompanionsIdsAsync().ConfigureAwait(false);
            bool hasFetchedImagesPaths = false;

            if (hasFetchedIds)
            {
                hasFetchedImagesPaths = await FetchCompanionsImagesPathsAsync().ConfigureAwait(false);
            }

            return hasFetchedIds && hasFetchedImagesPaths;
        }
        private static async Task<bool> FetchCompanionsIdsAsync()
        {
            string url = $"{App.BaseUrl}/lol-inventory/v1/inventory?inventoryTypes=%5B%22COMPANION%22%5D";
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
            items = items.OrderBy(e => e.ItemId).ToList();
            foreach (var item in items)
            {
                Companion companion = new()
                {
                    ItemId = item.ItemId
                };
                Companions.Add(companion);
            }

            return true;
        }
        private static async Task<bool> FetchCompanionsImagesPathsAsync()
        {
            using HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/companions.json");

            if (!response.IsSuccessStatusCode)
                return false;

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var jsonObjects = JsonConvert.DeserializeObject<List<CompanionCDragon>>(jsonResponse);

            if (jsonObjects == null)
                return false;

            foreach (Companion companion in Companions)
            {
                CompanionCDragon responseObj = jsonObjects.FirstOrDefault(obj => obj.ItemId == companion.ItemId);
                if (responseObj != null)
                {
                    companion.Name = responseObj.Name;
                    companion.ImageUrl = $"https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/{responseObj.LoadoutsIcon.Replace("/lol-game-data/assets/", "").ToLower()}";
                    companion.RarityValue = responseObj.RarityValue;
                    companion.Rarity = responseObj.Rarity;
                }
            }

            return true;
        }
        static string GetRandom()
        {
            Favorite favorite = FileManager.GetFavorite();
            List<Companion> selectedCompanions = new();

            if (favorite.Type == 0 && favorite.Companions.Count > 0)
            {
                selectedCompanions = Companions.Where(e => favorite.Companions.Contains(e.ItemId.ToString())).ToList();
            }
            else if (favorite.Type == 1 && favorite.Companions.Count < Companions.Count)
            {
                selectedCompanions = Companions.Where(e => !favorite.Companions.Contains(e.ItemId.ToString())).ToList();
            }

            if (selectedCompanions.Count > 0)
            {
                Random random = new();
                int randomIndex = random.Next(0, selectedCompanions.Count);
                return selectedCompanions[randomIndex].ItemId.ToString();
            }

            Random fallbackRandom = new();
            int fallbackRandomIndex = fallbackRandom.Next(0, Companions.Count);
            return Companions[fallbackRandomIndex].ItemId.ToString();
        }

        public static async Task<bool> SetCompanion(string id = "")
        {
            string selectedId;
            if (string.IsNullOrEmpty(id))
            {
                selectedId = GetRandom();
            }
            else
            {
                selectedId = id;
            }

            string url = $"{App.BaseUrl}/lol-cosmetics/v1/selection/companion";
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
