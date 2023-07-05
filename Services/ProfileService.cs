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
    public static class ProfileService
    {
        private static readonly string APP_DATA = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string FOLDER_PATH = Path.Combine(APP_DATA, "TFTCosmeticsManager");
        private static readonly string FILE_PATH = Path.Combine(FOLDER_PATH, "profiles.json");

        public static List<Profile> LoadProfiles()
        {
            if (!Directory.Exists(FOLDER_PATH))
            {
                Directory.CreateDirectory(FOLDER_PATH);
            }

            if (!File.Exists(FILE_PATH))
            {
                File.Create(FILE_PATH);
            }

            string json = File.ReadAllText(FILE_PATH);

            List<Profile> profiles = JsonConvert.DeserializeObject<List<Profile>>(json);
            if (profiles != null)
                return profiles;

            return new List<Profile>();
        }
        private static void SaveProfiles(List<Profile> profiles)
        {
            string json = JsonConvert.SerializeObject(profiles, Formatting.Indented);
            File.WriteAllText(FILE_PATH, json);
        }

        public static void AddProfile(Profile profile)
        {
            List<Profile> profiles = LoadProfiles();
            profiles.Add(profile);
            SaveProfiles(profiles);
        }

        public static void RemoveProfile(Profile profile)
        {
            List<Profile> profiles = LoadProfiles();
            profiles.Remove(profile);
            SaveProfiles(profiles);
        }
    }
}
