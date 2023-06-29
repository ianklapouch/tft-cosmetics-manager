﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using tft_cosmetics_manager.Models;
using tft_cosmetics_manager.Services;

namespace tft_cosmetics_manager.ViewModels
{
    public class CreateProfileViewModel : INotifyPropertyChanged
    {
        private readonly CompanionService companionViewModel = new();

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
            foreach (Companion companion in CompanionService.Companions)
            {
                BitmapImage bitmapImage = ImageService.CreateBitmapImageFromUrl(companion.LoadoutsIcon);
                CompanionUrls.Add(bitmapImage);
            }
        }
    }
}
