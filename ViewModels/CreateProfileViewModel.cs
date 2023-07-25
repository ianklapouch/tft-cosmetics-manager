using GalaSoft.MvvmLight.Command;
using Microsoft.Xaml.Behaviors.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
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
        public ObservableCollection<GridItem> Companions { get; set; } = new();
        public ObservableCollection<GridItem> MapSkins { get; set; } = new();
        public ObservableCollection<GridItem> DamageSkins { get; set; } = new();

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
        public ICommand SortCompanionsNameCommand { get; private set; }
        public ICommand SortCompanionsRarityCommand { get; private set; }
        public ICommand SortMapSkinsNameCommand { get; private set; }
        public ICommand SortMapSkinsRarityCommand { get; private set; }
        public ICommand SortDamageSkinsNameCommand { get; private set; }
        public ICommand SortDamageSkinsRarityCommand { get; private set; }


        public CreateProfileViewModel()
        {
            name = "New Profile";
            LoadImages();

            SortCompanionsNameCommand = new RelayCommand(SortCompanionsByName);
            SortCompanionsRarityCommand = new RelayCommand(SortCompanionsByRarity);
            SortMapSkinsNameCommand = new RelayCommand(SortMapSkinsByName);
            SortMapSkinsRarityCommand = new RelayCommand(SortMapSkinsByRarity);
            SortDamageSkinsNameCommand = new RelayCommand(SortDamageSkinsByName);
            SortDamageSkinsRarityCommand = new RelayCommand(SortDamageSkinsByRarity);
        }

        private void LoadImages()
        {
            foreach (Companion companion in CompanionService.Companions)
            {
                BitmapImage bitmapImage = ImageService.CreateBitmapImageFromUrl(companion.ImageUrl);
                BitmapImage platingBitMapImage = ImageService.CreateBitmapImageFromRelativeUrl($"/Assets/Plating/{companion.Rarity}.png");

                Companions.Add(new()
                {
                    ItemId = companion.ItemId,
                    Name = companion.Name,
                    Image = bitmapImage,
                    RarityValue = companion.RarityValue,
                    PlatingImage = platingBitMapImage
                });
            }
            foreach (MapSkin mapSkin in MapSkinService.MapSkins)
            {
                BitmapImage bitmapImage = ImageService.CreateBitmapImageFromUrl(mapSkin.ImageUrl);
                BitmapImage platingBitMapImage = ImageService.CreateBitmapImageFromRelativeUrl($"/Assets/Plating/{mapSkin.Rarity}.png");
                MapSkins.Add(new()
                {
                    ItemId = mapSkin.ItemId,
                    Name = mapSkin.Name,
                    Image = bitmapImage,
                    RarityValue = mapSkin.RarityValue,
                    PlatingImage = platingBitMapImage
                });
            }
            foreach (DamageSkin damageSkin in DamageSkinService.DamageSkins)
            {
                BitmapImage bitmapImage = ImageService.CreateBitmapImageFromUrl(damageSkin.ImageUrl);
                BitmapImage platingBitMapImage = ImageService.CreateBitmapImageFromRelativeUrl($"/Assets/Plating/{damageSkin.Rarity}.png");
                DamageSkins.Add(new()
                {
                    ItemId = damageSkin.ItemId,
                    Name = damageSkin.Name,
                    Image = bitmapImage,
                    RarityValue = damageSkin.RarityValue,
                    PlatingImage = platingBitMapImage
                });
            }
        }
        private void SortCompanionsByName()
        {
            var sortedList = Companions.OrderBy(c => c.Name).ToList();
            Companions.Clear();
            foreach (var item in sortedList)
            {
                Companions.Add(item);
            }
        }
        private void SortCompanionsByRarity()
        {
            var sortedList = Companions.OrderByDescending(c => c.RarityValue % 2 == 0 ? c.RarityValue : int.MaxValue - c.RarityValue).ToList();
            Companions.Clear();
            foreach (var item in sortedList)
            {
                Companions.Add(item);
            }
        }
        private void SortMapSkinsByName()
        {
            var sortedList = MapSkins.OrderBy(c => c.Name).ToList();
            MapSkins.Clear();
            foreach (var item in sortedList)
            {
                MapSkins.Add(item);
            }
        }
        private void SortMapSkinsByRarity()
        {
            var sortedList = MapSkins.OrderByDescending(c => c.RarityValue % 2 == 0 ? c.RarityValue : int.MaxValue - c.RarityValue).ToList();
            MapSkins.Clear();
            foreach (var item in sortedList)
            {
                MapSkins.Add(item);
            }
        }
        private void SortDamageSkinsByName()
        {
            var sortedList = DamageSkins.OrderBy(c => c.Name).ToList();
            DamageSkins.Clear();
            foreach (var item in sortedList)
            {
                DamageSkins.Add(item);
            }
        }
        private void SortDamageSkinsByRarity()
        {
            var sortedList = DamageSkins.OrderByDescending(c => c.RarityValue % 2 == 0 ? c.RarityValue : int.MaxValue - c.RarityValue).ToList();
            DamageSkins.Clear();
            foreach (var item in sortedList)
            {
                DamageSkins.Add(item);
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
        public int RarityValue { get; set; }
        public BitmapImage PlatingImage { get; set; }
    }
}
