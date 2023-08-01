using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tft_cosmetics_manager.Models;

namespace tft_cosmetics_manager.Services
{
    public static class FileManager
    {
        private static readonly string APP_DATA = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string FOLDER_PATH = Path.Combine(APP_DATA, "TFTCosmeticsManager");
        private static readonly string PROFILES_PATH = Path.Combine(FOLDER_PATH, "profiles.json");
        private static readonly string FAVORITES_PATH = Path.Combine(FOLDER_PATH, "favorites.json");
        public static List<Profile> LoadProfiles()
        {
            if (!Directory.Exists(FOLDER_PATH))
            {
                Directory.CreateDirectory(FOLDER_PATH);
                
                File.Create(PROFILES_PATH);

                string favoriteJson = JsonConvert.SerializeObject(new Favorite(), Formatting.Indented);
                File.WriteAllText(FAVORITES_PATH, favoriteJson);
                //File.Create(FAVORITES_PATH);

                return new List<Profile>();
            }
            string json = File.ReadAllText(PROFILES_PATH);

            List<Profile> profiles = JsonConvert.DeserializeObject<List<Profile>>(json);
            if (profiles != null)
            {
                for (int i = 0; i < profiles.Count; i++)
                {
                    profiles[i].Id = i.ToString();
                }

                json = JsonConvert.SerializeObject(profiles, Formatting.Indented);
                File.WriteAllText(PROFILES_PATH, json);

                return profiles;
            }

            return new List<Profile>();
        }
        private static void SaveProfiles(List<Profile> profiles)
        {
            string json = JsonConvert.SerializeObject(profiles, Formatting.Indented);
            File.WriteAllText(PROFILES_PATH, json);
        }
        public static void AddProfile(Profile profile)
        {
            List<Profile> profiles = LoadProfiles();
            profiles.Add(profile);
            SaveProfiles(profiles);
        }
        public static void RemoveProfile(string id)
        {
            List<Profile> profiles = LoadProfiles();
            Profile profile = profiles.Find(p => p.Id == id);
            profiles.Remove(profile);
            SaveProfiles(profiles);
        }
        public static void ReplaceProfile(Profile profile)
        {
            List<Profile> profiles = LoadProfiles();

            int index = profiles.FindIndex(p => p.Id == profile.Id);
            if (index != -1)
            {
                profiles[index] = profile;
            }
            SaveProfiles(profiles);
        }

        public static void SetFavorite(Favorite favorite)
        {
            string json = JsonConvert.SerializeObject(favorite, Formatting.Indented);
            File.WriteAllText(FAVORITES_PATH, json);
        }
        public static Favorite GetFavorite()
        {
            string json = File.ReadAllText(FAVORITES_PATH);
            return JsonConvert.DeserializeObject<Favorite>(json);
        }
    }
}
