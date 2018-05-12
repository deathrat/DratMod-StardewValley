using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Locations;

namespace DratMod
{
    public class DratModArtifactCart : IDratSubMod
    {
        DratModMain _parentMod;
        DratModConfig _config;
        IReflectionHelper _reflectionHelper;
        bool _dayInitialized = false;

        public void Entry(DratModMain parentMod, IModHelper helper)
        {
            if (!parentMod.Config.ArtifactSubmodEnabled)
                return;

            _parentMod = parentMod;
            _reflectionHelper = helper.Reflection;
            _config = parentMod.Config;
            HookEvents();
            _parentMod.Monitor.Log("[DratMod.Artifact] Submod loaded");
        }

        void HookEvents()
        {
            PlayerEvents.Warped += Event_LocationsChanged;
            TimeEvents.AfterDayStarted += Event_AfterDayStarted;
        }

        void Event_LocationsChanged(object sender, EventArgs e)
        {
            var args = (EventArgsPlayerWarped) e;
            if (args.NewLocation is Forest)
            {
                if(!_dayInitialized)
                    PopulateStock();
            }
        }

        void Event_AfterDayStarted(object sender, EventArgs e)
        {
            _dayInitialized = false;
        }

        void PopulateStock()
        {
            var forestLocation = (Forest)Game1.getLocationFromName("Forest");
            var player = Game1.MasterPlayer;
            if (forestLocation.travelingMerchantDay)
            {
                List<int> missingItems = new List<int>();

                string[] strArray = null;

                foreach (KeyValuePair<int, string> pair in Game1.objectInformation)
                {
                    char[] seperator = { '/' };
                    strArray = pair.Value.Split(seperator);
                    if (strArray[3].Contains("Arch"))
                    {
                        var itemIndex = pair.Key;
                        if (!player.archaeologyFound.ContainsKey(itemIndex))
                        {
                            missingItems.Add(itemIndex);
                        }
                    }
                }

                if (missingItems.Count <= _config.ArtifactThreshold)
                {
                    Random random = new Random((int)Game1.uniqueIDForThisGame * missingItems.Count + (SDate.Now().Day * SDate.Now().Year));
                    int itemIndex = missingItems[random.Next(missingItems.Count)];
                    var priceQuantity = new[] { _config.ArtifactPrice, 1 };

                    var stock = _reflectionHelper.GetField<Dictionary<Item, int[]>>(forestLocation, "travelerStock").GetValue();

                    var item = new StardewValley.Object(itemIndex, 1, false, _config.ArtifactPrice);
                    stock.Add(item, priceQuantity);
                    _dayInitialized = true;
                }
            }
        }
    }
}
