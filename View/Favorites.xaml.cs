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
using tft_cosmetics_manager.View;
using tft_cosmetics_manager.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace tft_cosmetics_manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Favorites : Window
    {
        public Favorites()
        {
            InitializeComponent();
            Loaded += Favorites_Loaded;

            // Definir o contexto de dados da janela como uma instância de CreateProfileViewModel
            DataContext = new FavoritesViewModel();
        }

        public event EventHandler<SelectedDataEventArgs> DataSent;

        // Método para chamar o evento ao fechar a janela
        protected virtual void OnDataSent(SelectedDataEventArgs e)
        {
            DataSent?.Invoke(this, e);
        }

        private void Favorites_Loaded(object sender, RoutedEventArgs e)
        {
            if (
                (companionsListBox.ItemsSource is ObservableCollection<CreateProfileGridItem> companions && companions.Any()) &&
                (mapSkinsListBox.ItemsSource is ObservableCollection<CreateProfileGridItem> mapSkins && mapSkins.Any()) &&
                (damageSkinsListBox.ItemsSource is ObservableCollection<CreateProfileGridItem> damageSkins && damageSkins.Any())
                )
            {

                Favorite favorite = FileManager.GetFavorite();

                if (favorite.Type == 0)
                    radioButtonWhiteList.IsChecked = true;

                if (favorite.Type == 1)
                    radioButtonBlackList.IsChecked = true;



                List<CreateProfileGridItem> selectedCompanions = companions.Where(e => favorite.Companions.Contains(e.ItemId.ToString())).ToList();
                foreach (var item in selectedCompanions)
                {
                    companionsListBox.SelectedItems.Add(item);
                }

                List<CreateProfileGridItem> selectedMapSkins = mapSkins.Where(e => favorite.MapSkins.Contains(e.ItemId.ToString())).ToList();
                foreach (var item in selectedMapSkins)
                {
                    mapSkinsListBox.SelectedItems.Add(item);
                }

                List<CreateProfileGridItem> selectedDamageSkins = damageSkins.Where(e => favorite.DamageSkins.Contains(e.ItemId.ToString())).ToList();
                foreach (var item in selectedDamageSkins)
                {
                    damageSkinsListBox.SelectedItems.Add(item);
                }
            }
        }
        public void CompanionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is FavoritesViewModel context)
            {
                if (e.AddedItems.Count > 0)
                {
                    foreach (CreateProfileGridItem companion in e.AddedItems)
                    {
                        context.SelectedCompanionIds.Add(companion.ItemId);
                    }
                }
                if (e.RemovedItems.Count > 0)
                {
                    foreach (CreateProfileGridItem companion in e.RemovedItems)
                    {
                        context.SelectedCompanionIds.Remove(companion.ItemId);
                    }
                }
            }
        }
        public void MapSkinListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is FavoritesViewModel context)
            {
                if (e.AddedItems.Count > 0)
                {
                    foreach (CreateProfileGridItem mapSkin in e.AddedItems)
                    {
                        context.SelectedMapSkinIds.Add(mapSkin.ItemId);
                    }
                }
                if (e.RemovedItems.Count > 0)
                {
                    foreach (CreateProfileGridItem mapSkin in e.RemovedItems)
                    {
                        context.SelectedMapSkinIds.Remove(mapSkin.ItemId);
                    }
                }
            }
        }
        public void DamageSkinsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is FavoritesViewModel context)
            {
                if (e.AddedItems.Count > 0)
                {
                    foreach (CreateProfileGridItem damageSkin in e.AddedItems)
                    {
                        context.SelectedDamageSkinIds.Add(damageSkin.ItemId);
                    }
                }
                if (e.RemovedItems.Count > 0)
                {
                    foreach (CreateProfileGridItem damageSkin in e.RemovedItems)
                    {
                        context.SelectedDamageSkinIds.Remove(damageSkin.ItemId);
                    }
                }
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            FavoritesViewModel? context = DataContext as FavoritesViewModel;
            if (context != null)
            {
                bool isWhiteListChecked = (bool)radioButtonWhiteList.IsChecked;

                List<string> companions = context.selectedCompanionIds.Select(i => i.ToString()).ToList();
                List<string> mapSkins = context.selectedMapSkinIds.Select(i => i.ToString()).ToList();
                List<string> damageSkins = context.selectedDamageSkinIds.Select(i => i.ToString()).ToList();

                Favorite favorite = new()
                {
                    Type = isWhiteListChecked ? 0 : 1,
                    Companions = companions,
                    MapSkins = mapSkins,
                    DamageSkins = damageSkins
                };

                FileManager.SetFavorite(favorite);
            }
            this.Close();


            //CreateProfileViewModel? context = DataContext as CreateProfileViewModel;

            //if (!string.IsNullOrEmpty(context.Name) && context.SelectedCompanion != null && context.SelectedMapSkin != null && context.SelectedDamageSkin != null)
            //{
            //    string name = context.Name;
            //    Companion companion = CompanionService.Companions.FirstOrDefault(obj => obj.ItemId == context.SelectedCompanion.ItemId);
            //    MapSkin mapSkin = MapSkinService.MapSkins.FirstOrDefault(obj => obj.ItemId == context.SelectedMapSkin.ItemId);
            //    DamageSkin damageSkin = DamageSkinService.DamageSkins.FirstOrDefault(obj => obj.ItemId == context.SelectedDamageSkin.ItemId);

            //    var args = new SelectedDataEventArgs()
            //    {
            //        ProfileId = context.ProfileId,
            //        Name = name,
            //        Companion = companion,
            //        MapSkin = mapSkin,
            //        DamageSkin = damageSkin
            //    };

            //    OnDataSent(args);
            //    this.Close();
            //}
            //else
            //{
            //    MessageBox.Show("Make sure to select items in all tabs and add a name before saving!", "Warning!", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            //}
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
