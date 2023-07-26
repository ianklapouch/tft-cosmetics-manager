using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tft_cosmetics_manager.Models;

namespace tft_cosmetics_manager.Services
{
    public static class MessengerService
    {
        public static event EventHandler<SelectionMessage<CreateProfileGridItem>> CompanionSelected;
        public static event EventHandler<SelectionMessage<CreateProfileGridItem>> MapSkinSelected;
        public static event EventHandler<SelectionMessage<CreateProfileGridItem>> DamageSkinSelected;

        public static void SendCompanionSelected(CreateProfileGridItem selectedCompanion)
        {
            CompanionSelected?.Invoke(null, new SelectionMessage<CreateProfileGridItem>(selectedCompanion));
        }

        public static void SendMapSkinSelected(CreateProfileGridItem selectedMapSkin)
        {
            MapSkinSelected?.Invoke(null, new SelectionMessage<CreateProfileGridItem>(selectedMapSkin));
        }

        public static void SendDamageSkinSelected(CreateProfileGridItem selectedDamageSkin)
        {
            DamageSkinSelected?.Invoke(null, new SelectionMessage<CreateProfileGridItem>(selectedDamageSkin));
        }
    }
}
