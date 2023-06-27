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
using tft_cosmetics_manager.ViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace tft_cosmetics_manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<GridItem> itemList = new ObservableCollection<GridItem>();
        private readonly CompanionViewModel companionViewModel;
        private readonly MapSkinViewModel mapSkinViewModel;
        private readonly DamageSkinViewModel damageSkinViewModel;

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

            IApiConfigService apiConfigService = new ApiConfigService();
            companionViewModel = new(apiConfigService);
            mapSkinViewModel = new(apiConfigService);
            damageSkinViewModel = new(apiConfigService);

            itemListView.ItemsSource = itemList;
            Loaded += MainWindow_Loaded;
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
                _ = companionViewModel.SetCompanion(selectedItem.CompanionId);
                _ = mapSkinViewModel.SetMapSkin(selectedItem.MapSkinId);
                _ = damageSkinViewModel.SetDamageSkin(selectedItem.DamageSkinId);
            }
        }


        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Random random = new();
            int randomIndex = random.Next(0, companionViewModel.companions.Count);
            string companionId1 = companionViewModel.companions[randomIndex].ItemId.ToString();
            string companionImage = companionViewModel.companions[randomIndex].LoadoutsIcon;

            randomIndex = random.Next(0, mapSkinViewModel.mapSkins.Count);
            string mapSkinId1 = mapSkinViewModel.mapSkins[randomIndex].ItemId.ToString();
            string mapSkinImage = mapSkinViewModel.mapSkins[randomIndex].LoadoutsIcon;

            randomIndex = random.Next(0, damageSkinViewModel.damageSkins.Count);
            string damageSkinId1 = damageSkinViewModel.damageSkins[randomIndex].ItemId.ToString();
            string damageSkinImage = damageSkinViewModel.damageSkins[randomIndex].LoadoutsIcon;


            List<(string title, string companionId, string companionImage, string mapSkinId, string mapSkinImage, string damageSkinId, string damageSkinImage)> newItems = new()
            {
                ("Profile 1", companionId1, companionImage, mapSkinId1, mapSkinImage, damageSkinId1, damageSkinImage),
            };


            foreach (var item in newItems)
            {
                BitmapImage bitmapCompanionImage = LoadImageFromBase64(item.companionImage);
                BitmapImage bitmapMapSkinImage = LoadImageFromBase64(item.mapSkinImage);
                BitmapImage bitmapDamageSkinImage = LoadImageFromBase64(item.damageSkinImage);

                itemList.Add(new GridItem { 
                    Text = item.title,
                    CompanionId = item.companionId,
                    CompanionImage = bitmapCompanionImage,
                    MapSkinId = item.mapSkinId,
                    MapSkinImage = bitmapMapSkinImage,
                    DamageSkinId = item.damageSkinId,
                    DamageSkinImage = bitmapDamageSkinImage 
                });
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
        private async Task LoadData()
        {
            bool hasLoadedCompanions = await companionViewModel.LoadCompanions();
            bool hasLoadedMapSkins = await mapSkinViewModel.LoadMapSkins();
            bool hasLoadedDamageSkins = await damageSkinViewModel.LoadDamageSkins();

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


            Task<bool> task1 = companionViewModel.SetCompanion();
            Task<bool> task2 = mapSkinViewModel.SetMapSkin();
            Task<bool> task3 = damageSkinViewModel.SetDamageSkin();

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
