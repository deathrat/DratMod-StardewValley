using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace DratMod
{
    public class DratModCoffeeStore : IDratSubMod, IAssetEditor
    {
        DialogueBox _lastMenu;

        Dictionary<Item, int[]> _shopItems;

        public void Entry(DratModMain parentMod, IModHelper helper)
        {
            if (!parentMod.Config.CoffeeBeanStoreEnabled)
                return;

            HookEvents();

            _shopItems = new Dictionary<Item, int[]>();
            _shopItems.Add(new StardewValley.Object(433, int.MaxValue, false, parentMod.Config.CoffeeBeanPrice, 0), new[] { parentMod.Config.CoffeeBeanPrice, int.MaxValue });
        }


        void HookEvents()
        {
            MenuEvents.MenuChanged += Event_MenuChanged;
            MenuEvents.MenuClosed += Event_MenuClosed;
        }

        void Event_MenuChanged(object sender, EventArgs e)
        {
            if (Game1.activeClickableMenu is DialogueBox)
            {
                var menu = (DialogueBox)Game1.activeClickableMenu;

                if (menu.getCurrentString() == "Merchant: Mmm... smells great, doesn\'t it? Interested in buying?")
                {
                    _lastMenu = menu;
                }
            }
        }

        void Event_MenuClosed(object sender, EventArgs e)
        {
            var args = (EventArgsClickableMenuClosed)e;
            if (args.PriorMenu is DialogueBox)
            {
                var menu = (DialogueBox)args.PriorMenu;
                if (_lastMenu != null && menu == _lastMenu)
                {
                    var newMenu = new ShopMenu(_shopItems, 0);
                    Game1.activeClickableMenu = newMenu;
                }
            }
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals(@"Strings\Locations");
        }

        public void Edit<T>(IAssetData asset)
        {
            string key = "BeachNightMarket_GiftGiverEnjoy";
            asset.AsDictionary<string, string>().Set(key, "Merchant: Mmm... smells great, doesn\'t it? Interested in buying?");
        }
    }
}
