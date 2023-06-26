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

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            List<(string text, string base64Image)> newItems = new List<(string, string)>
            {
                ("Profile 1", companionViewModel.companions[0].LoadoutsIcon),
                ("Profile 2", mapSkinViewModel.mapSkins[0].LoadoutsIcon),
                ("Profile 3", damageSkinViewModel.damageSkins[0].LoadoutsIcon),
            };

            foreach (var item in newItems)
            {
                BitmapImage bitmapImage = LoadImageFromBase64(item.base64Image);
                itemList.Add(new GridItem { Text = item.text, CompanionImage = bitmapImage });
            }

            _ = companionViewModel.SetRandom();
            _ = mapSkinViewModel.SetRandom();
            _ = damageSkinViewModel.SetRandom();
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
            //Button? button = FindName("randomizeButton") as Button;


            //if (button != null)
            //{
            //    button.IsEnabled = false;
            //    button.Cursor = Cursors.No;
            //    button.Content = "Loading...";
            //}


            //Task<bool> task1 = SetCompanion();
            //Task<bool> task2 = SetMapSkin();
            //Task<bool> task3 = SetDamageSkin();
            //await Task.WhenAll(task1, task2, task3);


            //if (button != null)
            //{
            //    button.IsEnabled = true;
            //    button.Cursor = Cursors.Hand;
            //    button.Content = "Randomize!";
            //}
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
