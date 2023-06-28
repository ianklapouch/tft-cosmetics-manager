using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using tft_cosmetics_manager.Models;

namespace tft_cosmetics_manager.ViewModels
{
    public class CreateProfileViewModel : INotifyPropertyChanged
    {
        private readonly CompanionViewModel companionViewModel = new();

        public event PropertyChangedEventHandler PropertyChanged;

        private string _selectedImage;
        public string SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                _selectedImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedImage)));
            }
        }

        public List<BitmapImage> CompanionUrls { get; } = new List<BitmapImage>();

        public CreateProfileViewModel()
        {
            LoadImagesAsync();
        }

        private async void LoadImagesAsync()
        {
            foreach (Companion companion in CompanionViewModel.Companions)
            {
                BitmapImage image = LoadImageFromBase64(companion.LoadoutsIcon);
                CompanionUrls.Add(image);
            }
        }

        private BitmapImage LoadImageFromBase64(string base64Image)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(base64Image);
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}
