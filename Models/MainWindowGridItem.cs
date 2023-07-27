using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace tft_cosmetics_manager.Models
{
    public class MainWindowGridItem : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string CompanionId { get; set; }
        public string CompanionName { get; set; }
        public BitmapImage CompanionImage { get; set; }
        public BitmapImage CompanionPlatingImage { get; set; }
        public string MapSkinId { get; set; }
        public string MapSkinName { get; set; }
        public BitmapImage MapSkinImage { get; set; }
        public BitmapImage MapSkinPlatingImage { get; set; }
        public string DamageSkinId { get; set; }
        public string DamageSkinName { get; set; }
        public BitmapImage DamageSkinImage { get; set; }
        public BitmapImage DamageSkinPlatingImage { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
