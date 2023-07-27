using System.Collections.Generic;

namespace tft_cosmetics_manager.Models
{
    public class Favorite
    {
        public int Type { get; set; }
        public List<string> Companions { get; set; }
        public List<string> MapSkins { get; set; }
        public List<string> DamageSkins { get; set; }
    }
}
