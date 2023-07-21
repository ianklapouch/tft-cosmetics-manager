using GalaSoft.MvvmLight.Command;
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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using tft_cosmetics_manager.Models;
using tft_cosmetics_manager.Services;

namespace tft_cosmetics_manager.ViewModels
{
    public class CreateProfileViewModel : INotifyPropertyChanged
    {
        public List<GridItem> Companions { get; } = new();
        public List<GridItem> MapSkins { get; } = new();
        public List<GridItem> DamageSkins { get; } = new();

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
        private GridItem selectedCompanion;
        public GridItem SelectedCompanion
        {
            get => selectedCompanion;
            set
            {
                selectedCompanion = value;
                OnPropertyChanged(nameof(SelectedCompanion));
            }
        }
        private GridItem selectedMapSkin;
        public GridItem SelectedMapSkin
        {
            get => selectedMapSkin;
            set
            {
                selectedMapSkin = value;
                OnPropertyChanged(nameof(SelectedMapSkin));
            }
        }

        private GridItem selectedDamageSkin;
        public GridItem SelectedDamageSkin
        {
            get => selectedDamageSkin;
            set
            {
                selectedDamageSkin = value;
                OnPropertyChanged(nameof(SelectedDamageSkin));
            }
        }
        public CreateProfileViewModel()
        {
            name = "New Profile";
            LoadImages();
        }

        private void LoadImages()
        {
            foreach (Companion companion in CompanionService.Companions)
            {
                BitmapImage bitmapImage = ImageService.CreateBitmapImageFromUrl(companion.ImageUrl);
                BitmapImage rarityBitMapImage = ImageService.CreateBitmapImageFromRelativeUrl($"/Assets/rarity-icons/{companion.Rarity}.png");

                Companions.Add(new()
                {
                    ItemId = companion.ItemId,
                    Name = companion.Name,
                    Image = bitmapImage,
                    RarityImage = rarityBitMapImage
                });
            }
            foreach (MapSkin mapSkin in MapSkinService.MapSkins)
            {
                BitmapImage bitmapImage = ImageService.CreateBitmapImageFromUrl(mapSkin.ImageUrl);
                MapSkins.Add(new()
                {
                    ItemId = mapSkin.ItemId,
                    Name = mapSkin.Name,
                    Image = bitmapImage
                });
            }
            foreach (DamageSkin damageSkin in DamageSkinService.DamageSkins)
            {
                BitmapImage bitmapImage = ImageService.CreateBitmapImageFromUrl(damageSkin.ImageUrl);
                DamageSkins.Add(new()
                {
                    ItemId = damageSkin.ItemId,
                    Name = damageSkin.Name,
                    Image = bitmapImage
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class GridItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public BitmapImage Image { get; set; }
        public BitmapImage RarityImage { get; set; }
    }
}
