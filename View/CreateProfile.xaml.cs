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
    public partial class CreateProfile : Window
    {
        public CreateProfile()
        {
            InitializeComponent();
            Loaded += CreateProfile_Loaded;

            // Definir o contexto de dados da janela como uma instância de CreateProfileViewModel
            DataContext = new CreateProfileViewModel();
        }

        public event EventHandler<SelectedDataEventArgs> DataSent;

        // Método para chamar o evento ao fechar a janela
        protected virtual void OnDataSent(SelectedDataEventArgs e)
        {
            DataSent?.Invoke(this, e);
        }

        private void CreateProfile_Loaded(object sender, RoutedEventArgs e)
        {
            txtCreateProfile.Focus();
            txtCreateProfile.SelectAll();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            CreateProfileViewModel? context = DataContext as CreateProfileViewModel;

            if (!string.IsNullOrEmpty(context.Name) && context.SelectedCompanion != null && context.SelectedMapSkin != null && context.SelectedDamageSkin != null)
            {
                string name = context.Name;
                Companion companion = CompanionService.Companions.FirstOrDefault(obj => obj.ItemId == context.SelectedCompanion.ItemId);
                MapSkin mapSkin = MapSkinService.MapSkins.FirstOrDefault(obj => obj.ItemId == context.SelectedMapSkin.ItemId);
                DamageSkin damageSkin = DamageSkinService.DamageSkins.FirstOrDefault(obj => obj.ItemId == context.SelectedDamageSkin.ItemId);

                var args = new SelectedDataEventArgs()
                {
                    Name = name,
                    Companion = companion,
                    MapSkin = mapSkin,
                    DamageSkin = damageSkin
                };

                OnDataSent(args);
                this.Close();
            }
            else
            {
                MessageBox.Show("Make sure to select items in all tabs and add a name before saving!", "Warning!", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
