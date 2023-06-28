using System;
using System.Windows.Media.Imaging;

namespace tft_cosmetics_manager.Services
{
    public class ImageService
    {
        public static BitmapImage CreateBitmapImageFromUrl(string url)
        {
            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(url);
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
