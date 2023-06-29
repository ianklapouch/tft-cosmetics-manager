using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using tft_cosmetics_manager.Models;
using tft_cosmetics_manager.Services;
using static System.Net.Mime.MediaTypeNames;

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

      

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            CreateProfile createProfile = new();
            createProfile.Owner = this;
            createProfile.Closed += CreateProfile_Closed;
            createProfile.Show();
            //Random random = new();
            //int randomIndex = random.Next(0, CompanionService.Companions.Count);
            //string companionId1 = CompanionService.Companions[randomIndex].ItemId.ToString();
            //string companionImage = CompanionService.Companions[randomIndex].LoadoutsIcon;

            //randomIndex = random.Next(0, MapSkinService.MapSkins.Count);
            //string mapSkinId1 = MapSkinService.MapSkins[randomIndex].ItemId.ToString();
            //string mapSkinImage = MapSkinService.MapSkins[randomIndex].LoadoutsIcon;

            //randomIndex = random.Next(0, DamageSkinService.DamageSkins.Count);
            //string damageSkinId1 = DamageSkinService.DamageSkins[randomIndex].ItemId.ToString();
            //string damageSkinImage = DamageSkinService.DamageSkins[randomIndex].LoadoutsIcon;


            //List<(string title, string companionId, string companionImage, string mapSkinId, string mapSkinImage, string damageSkinId, string damageSkinImage)> newItems = new()
            //{
            //    ("Profile 1", companionId1, companionImage, mapSkinId1, mapSkinImage, damageSkinId1, damageSkinImage),
            //};


            //foreach (var item in newItems)
            //{
            //    BitmapImage bitmapCompanionImage = ImageService.CreateBitmapImageFromUrl(item.companionImage);
            //    BitmapImage bitmapMapSkinImage = ImageService.CreateBitmapImageFromUrl(item.mapSkinImage);
            //    BitmapImage bitmapDamageSkinImage = ImageService.CreateBitmapImageFromUrl(item.damageSkinImage);

            //    itemList.Add(new GridItem
            //    {
            //        Text = item.title,
            //        CompanionId = item.companionId,
            //        CompanionImage = bitmapCompanionImage,
            //        MapSkinId = item.mapSkinId,
            //        MapSkinImage = bitmapMapSkinImage,
            //        DamageSkinId = item.damageSkinId,
            //        DamageSkinImage = bitmapDamageSkinImage
            //    });
            //}
        }
        private void CreateProfile_Closed(object sender, EventArgs e)
        {
            // Habilita a janela principal quando a janela modal for fechada
            this.IsEnabled = true;
        }
        private async Task LoadData()
        {
            bool hasLoadedCompanions = await CompanionService.FetchCompanions();
            bool hasLoadedMapSkins = await MapSkinService.FetchMapSkins();
            bool hasLoadedDamageSkins = await DamageSkinService.FetchDamageSkins();

            if (hasLoadedCompanions && hasLoadedMapSkins && hasLoadedDamageSkins)
            {
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
