﻿using GalaSoft.MvvmLight.Command;
using Microsoft.Xaml.Behaviors.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using tft_cosmetics_manager.Models;
using tft_cosmetics_manager.Services;

namespace tft_cosmetics_manager.ViewModels
{
    public class CreateProfileViewModel : INotifyPropertyChanged
    {
        public List<BitmapImage> CompanionImages { get; } = new List<BitmapImage>();
        public List<BitmapImage> MapSkinImages { get; } = new List<BitmapImage>();
        public List<BitmapImage> DamageSkinImages { get; } = new List<BitmapImage>();

        public event PropertyChangedEventHandler PropertyChanged;
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(name)));
            }
        }
        private string selectedCompanionImage;
        public string SelectedCompanionImage
        {
            get { return selectedCompanionImage; }
            set
            {
                selectedCompanionImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(selectedCompanionImage)));
            }
        }
        private string selectedMapSkinImage;
        public string SelectedMapSkinImage
        {
            get { return selectedMapSkinImage; }
            set
            {
                selectedMapSkinImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(selectedMapSkinImage)));
            }
        }
        private string selectedDamageSkinImage;
        public string SelectedDamageSkinImage
        {
            get { return selectedDamageSkinImage; }
            set
            {
                selectedDamageSkinImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(selectedDamageSkinImage)));
            }
        }

        public CreateProfileViewModel()
        {
            name = "New Profile";
            LoadImages();
        }
        //


        private void LoadImages()
        {
            foreach (Companion companion in CompanionService.Companions)
            {
                BitmapImage bitmapImage = ImageService.CreateBitmapImageFromUrl(companion.LoadoutsIcon);
                CompanionImages.Add(bitmapImage);
            }
            foreach (MapSkin mapSkin in MapSkinService.MapSkins)
            {
                BitmapImage bitmapImage = ImageService.CreateBitmapImageFromUrl(mapSkin.LoadoutsIcon);
                MapSkinImages.Add(bitmapImage);
            }
            foreach (DamageSkin damageSkin in DamageSkinService.DamageSkins)
            {
                BitmapImage bitmapImage = ImageService.CreateBitmapImageFromUrl(damageSkin.LoadoutsIcon);
                DamageSkinImages.Add(bitmapImage);
            }
        }
    }
}
