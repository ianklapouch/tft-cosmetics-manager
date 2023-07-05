using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tft_cosmetics_manager.Models;

namespace tft_cosmetics_manager.View
{
    public class SelectedDataEventArgs : EventArgs
    {
        public string Name { get; set; }
        public Companion Companion { get; set; }
        public MapSkin MapSkin { get; set; }
        public DamageSkin DamageSkin{ get; set; }
    }
}
