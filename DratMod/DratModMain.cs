using System;
using System.Collections.Generic;
using StardewModdingAPI;

namespace DratMod
{
    public class DratModMain : Mod
    {
        List<IDratSubMod> _subMods;

        public DratModConfig Config;

        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<DratModConfig>();
            _subMods = new List<IDratSubMod>();

            LoadSubMods(helper, _subMods);
        }

        void LoadSubMods(IModHelper helper, List<IDratSubMod> subMods)
        {
            _subMods.Add(new DratModCoffeeStore());

            foreach (var subMod in subMods)
            {
                subMod.Entry(this, helper);

                if (subMod is IAssetEditor)
                {
                    helper.Content.AssetEditors.Add((IAssetEditor)subMod);
                }
            }
        }
    }
}
