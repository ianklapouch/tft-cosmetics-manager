using System.Windows.Media.Imaging;

namespace tft_cosmetics_manager.Models
{
    public class CreateProfileGridItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public BitmapImage Image { get; set; }
        public int RarityValue { get; set; }
        public BitmapImage PlatingImage { get; set; }
    }
}
