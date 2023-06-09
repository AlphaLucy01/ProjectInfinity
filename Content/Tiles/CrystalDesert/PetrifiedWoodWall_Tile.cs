﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ProjectInfinity.Core;

namespace ProjectInfinity.Content.Tiles.CrystalDesert
{
    internal class PetrifiedWoodWall_Tile : ModWall
    {
        public override string Texture => AssetDirectory.CrystalDesert_Tiles + Name;
        public override void SetStaticDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(Color.Gray, name);
            
        }
    }
}
