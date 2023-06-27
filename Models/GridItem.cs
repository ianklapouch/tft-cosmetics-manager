using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace tft_cosmetics_manager.Models
{
    public class GridItem : INotifyPropertyChanged
    {
        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }
        private string companionId;
        public string CompanionId
        {
            get { return companionId; }
            set
            {
                companionId = value;
                OnPropertyChanged("CompanionId");
            }
        }
        private BitmapImage companionImage;
        public BitmapImage CompanionImage
        {
            get { return companionImage; }
            set
            {
                companionImage = value;
                OnPropertyChanged("CompanionImage");
            }
        }
        private string mapSkinId;
        public string MapSkinId
        {
            get { return mapSkinId; }
            set
            {
                mapSkinId = value;
                OnPropertyChanged("MapSkinId");
            }
        }
        private BitmapImage mapSkinImage;
        public BitmapImage MapSkinImage
        {
            get { return mapSkinImage; }
            set
            {
                mapSkinImage = value;
                OnPropertyChanged("MapSkinImage");
            }
        }
        private string damageSkinId;
        public string DamageSkinId
        {
            get { return damageSkinId; }
            set
            {
                damageSkinId = value;
                OnPropertyChanged("DamageSkinId");
            }
        }
        private BitmapImage damageSkinImage;
        public BitmapImage DamageSkinImage
        {
            get { return damageSkinImage; }
            set
            {
                damageSkinImage = value;
                OnPropertyChanged("DamageSkinImage");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
