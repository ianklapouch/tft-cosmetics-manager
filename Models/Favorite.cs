using System.Collections.Generic;

namespace tft_cosmetics_manager.Models
{
    public class Favorite
    {
        public Favorite()
        {
            Type = 0;
            Companions = new List<string>();
            MapSkins = new List<string>();
            DamageSkins = new List<string>();
        }
        public int Type { get; set; } = 0;
        public List<string> Companions { get; set; } 
        public List<string> MapSkins { get; set; } 
        public List<string> DamageSkins { get; set; }
    }
}
