﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using tft_cosmetics_manager.Models;
using System.Text;

namespace tft_cosmetics_manager.Services
{
    public class MapSkinService
    {
        public static List<MapSkin> MapSkins { get; } = new();
        public static async Task<bool> FetchMapSkins()
        {
            bool hasFetchedIds = await FetchMapSkinsIdsAsync().ConfigureAwait(false);
            bool hasFetchedImagesPaths = false;

            if (hasFetchedIds)
            {
                hasFetchedImagesPaths = await FetchMapSkinsImagesPathsAsync().ConfigureAwait(false);
            }

            return hasFetchedIds && hasFetchedImagesPaths;
        }

        private static async Task<bool> FetchMapSkinsIdsAsync()
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
            items = items.OrderBy(e => e.ItemId).ToList();
            foreach (var item in items)
            {
                MapSkin mapSkin = new()
                {
                    ItemId = item.ItemId
                };
                MapSkins.Add(mapSkin);
            }

            return true;
        }

        private static async Task<bool> FetchMapSkinsImagesPathsAsync()
        {
            using HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/tftmapskins.json");

            if (!response.IsSuccessStatusCode)
                return false;

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var jsonObjects = JsonConvert.DeserializeObject<List<MapSkinCDragon>>(jsonResponse);


            foreach (MapSkin mapSkin in MapSkins)
            {
                MapSkinCDragon responseObj = jsonObjects.FirstOrDefault(obj => obj.ItemId == mapSkin.ItemId);
                if (responseObj != null)
                {
                    mapSkin.Name = responseObj.Name;
                    mapSkin.ImageUrl = $"https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/{responseObj.LoadoutsIcon.Replace("/lol-game-data/assets/", "").ToLower()}";
                   mapSkin.RarityValue = responseObj.RarityValue; 
                    mapSkin.Rarity = responseObj.Rarity;
                }
            }

            return true;
        }
        static string GetRandom()
        {
            Favorite favorite = FileManager.GetFavorite();
            List<MapSkin> selectedMapSkins = new();

            if (favorite.Type == 0 && favorite.MapSkins.Count > 0)
            {
                selectedMapSkins = MapSkins.Where(e => favorite.MapSkins.Contains(e.ItemId.ToString())).ToList();
            }
            else if (favorite.Type == 1 && favorite.Companions.Count < MapSkins.Count)
            {
                selectedMapSkins = MapSkins.Where(e => !favorite.MapSkins.Contains(e.ItemId.ToString())).ToList();
            }

            if (selectedMapSkins.Count > 0)
            {
                Random random = new();
                int randomIndex = random.Next(0, selectedMapSkins.Count);
                return selectedMapSkins[randomIndex].ItemId.ToString();
            }

            Random fallbackRandom = new();
            int fallbackRandomIndex = fallbackRandom.Next(0, MapSkins.Count);
            return MapSkins[fallbackRandomIndex].ItemId.ToString();
        }
        public static async Task<bool> SetMapSkin(string id = "")
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
