using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
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
        public static BitmapImage CreateBitmapImageFromRelativeUrl(string url)
        {
            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(url, UriKind.Relative);
            bitmapImage.EndInit();
            return bitmapImage;
        }
        public static async Task<BitmapImage> CreateBitmapImageFromUrlAsync(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    byte[] imageData = await httpClient.GetByteArrayAsync(url);

                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = new MemoryStream(imageData);
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();

                    return bitmapImage;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }



    }
}
