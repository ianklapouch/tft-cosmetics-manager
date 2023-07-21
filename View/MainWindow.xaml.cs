﻿using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using tft_cosmetics_manager.Models;
using tft_cosmetics_manager.Services;
using tft_cosmetics_manager.View;

namespace tft_cosmetics_manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<GridItem> itemList = new ObservableCollection<GridItem>();

        private bool IsDarkThemeEnabled { get; set; } = false;

        public void UpdateTheme(object sender, RoutedEventArgs e)
        {
            PaletteHelper palette = new PaletteHelper();

            ITheme theme = palette.GetTheme();

            if (IsDarkThemeEnabled)
            {
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                theme.SetBaseTheme(Theme.Dark);
            }
            palette.SetTheme(theme);
            IsDarkThemeEnabled = !IsDarkThemeEnabled;
        }

        private void DarkModeToggle_Checked(object sender, RoutedEventArgs e)
        {

        }


        public MainWindow()
        {
            InitializeComponent();

            bool hasLoadedLCUKeys = LCUService.FetchKeys();

            if (hasLoadedLCUKeys)
            {
                itemListView.ItemsSource = itemList;
                Loaded += MainWindow_Loaded;
            }
            else
            {
                MessageBox.Show("No running instance of League of Legends found, make sure the client is open and try again!", "Warning!");
                System.Windows.Application.Current.Shutdown();
            }


        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ShowOverlay();
            LoadData();
        }

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            var listViewItem = (ListViewItem)sender;
            var selectedItem = (GridItem)listViewItem.DataContext;

            if (selectedItem != null)
            {
                _ = CompanionService.SetCompanion(selectedItem.CompanionId);
                _ = MapSkinService.SetMapSkin(selectedItem.MapSkinId);
                _ = DamageSkinService.SetDamageSkin(selectedItem.DamageSkinId);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this profile? this action cannot be undone!", "Warning!", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.OK)
            {
                Button clickedButton = (Button)sender;
                GridItem selectedItem = (GridItem)clickedButton.DataContext;
                string selectedId = selectedItem.Id;

                ProfileService.RemoveProfile(selectedItem.Id);

                GridItem itemToRemove = itemList.FirstOrDefault(item => item.Id == selectedId);

                if (itemToRemove != null)
                {
                    itemList.Remove(itemToRemove);
                }
            }
        }



        private void CreateProfile_DataSent(object sender, SelectedDataEventArgs e)
        {
            string id = itemList.Count + 1.ToString();

            BitmapImage companionBitMapImage = ImageService.CreateBitmapImageFromUrl(e.Companion.ImageUrl);
            BitmapImage mapSkinBitMapImage = ImageService.CreateBitmapImageFromUrl(e.MapSkin.ImageUrl);
            BitmapImage damageSkinBitMapImage = ImageService.CreateBitmapImageFromUrl(e.DamageSkin.ImageUrl);

            itemList.Add(new GridItem
            {
                Id = id,
                Text = e.Name,
                CompanionId = e.Companion.ItemId.ToString(),
                CompanionImage = companionBitMapImage,
                MapSkinId = e.MapSkin.ItemId.ToString(),
                MapSkinImage = mapSkinBitMapImage,
                DamageSkinId = e.DamageSkin.ItemId.ToString(),
                DamageSkinImage = damageSkinBitMapImage
            });

            Profile profile = new()
            {
                Id = id,
                Title = e.Name,
                CompanionId = e.Companion.ItemId.ToString(),
                MapSkinId = e.MapSkin.ItemId.ToString(),
                DamageSkinId = e.DamageSkin.ItemId.ToString()
            };

            ProfileService.AddProfile(profile);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            CreateProfile createProfile = new()
            {
                Owner = this
            };
            createProfile.DataSent += CreateProfile_DataSent;
            createProfile.Closed += CreateProfile_Closed;
            createProfile.ShowDialog();
        }
        private void CreateProfile_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
        }
        private async Task LoadData()
        {
            bool hasLoadedCompanions = await CompanionService.FetchCompanions();
            bool hasLoadedMapSkins = await MapSkinService.FetchMapSkins();
            bool hasLoadedDamageSkins = await DamageSkinService.FetchDamageSkins();

            if (hasLoadedCompanions && hasLoadedMapSkins && hasLoadedDamageSkins)
            {
                List<Profile> profiles = ProfileService.LoadProfiles();

                if (profiles.Count > 0)
                {
                    foreach (Profile profile in profiles)
                    {


                        Companion companion = CompanionService.Companions.FirstOrDefault(obj => obj.ItemId.ToString() == profile.CompanionId);
                        MapSkin mapSkin = MapSkinService.MapSkins.FirstOrDefault(obj => obj.ItemId.ToString() == profile.MapSkinId);
                        DamageSkin damageSkin = DamageSkinService.DamageSkins.FirstOrDefault(obj => obj.ItemId.ToString() == profile.DamageSkinId);

                        BitmapImage companionBitMapImage = ImageService.CreateBitmapImageFromUrl(companion.ImageUrl);
                        BitmapImage mapSkinBitMapImage = ImageService.CreateBitmapImageFromUrl(mapSkin.ImageUrl);
                        BitmapImage damageSkinBitMapImage = ImageService.CreateBitmapImageFromUrl(damageSkin.ImageUrl);


                        itemList.Add(new GridItem
                        {
                            Id = profile.Id,
                            Text = profile.Title,
                            CompanionId = profile.CompanionId,
                            CompanionImage = companionBitMapImage,
                            MapSkinId = profile.MapSkinId,
                            MapSkinImage = mapSkinBitMapImage,
                            DamageSkinId = profile.DamageSkinId,
                            DamageSkinImage = damageSkinBitMapImage,
                        });
                    }
                }



                HideOverlay();
            }
        }
        private async void RandomizeButton_Click(object sender, RoutedEventArgs e)
        {
            Button? button = FindName("btnRandomize") as Button;


            if (button != null)
            {
                button.IsEnabled = false;
                button.Cursor = Cursors.No;
                button.Content = "Loading...";
            }


            Task<bool> task1 = CompanionService.SetCompanion();
            Task<bool> task2 = MapSkinService.SetMapSkin();
            Task<bool> task3 = DamageSkinService.SetDamageSkin();

            await Task.WhenAll(task1, task2, task3);


            if (button != null)
            {
                button.IsEnabled = true;
                button.Cursor = Cursors.Hand;
                button.Content = "Randomize!";
            }
        }
        private void ShowOverlay()
        {
            Overlay.Visibility = Visibility.Visible;
            Mouse.OverrideCursor = Cursors.Wait;
            System.Windows.Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = null);
        }
        private void HideOverlay()
        {
            Overlay.Visibility = Visibility.Collapsed;
        }
    }
}
