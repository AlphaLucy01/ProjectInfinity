﻿using Microsoft.Xna.Framework;
using ProjectInfinity.Content.Items.Blocks.CrystalDesert;
using ProjectInfinity.Content.Projectiles;
using ProjectInfinity.Core;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ProjectInfinity.Content.Tiles.CrystalDesert
{
    internal class CrystalLaser_Tile : ModTile
    {
        public override string Texture => AssetDirectory.CrystalDesert_Tiles + "CrystalSandstone_Tile";
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.lightning = 15f;

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(Color.DarkOliveGreen, name);
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            int index = Projectile.NewProjectile(new EntitySource_TileUpdate(i, j), new Vector2(i, j) * 16, new Vector2(0, 1), ModContent.ProjectileType<LaserCrystal>(), 0, 0, Main.myPlayer);
            Projectile proj = Main.projectile[index];
            if (proj.ModProjectile is LaserCrystal)
            {
                (proj.ModProjectile as LaserCrystal).parentTileX = i;
                (proj.ModProjectile as LaserCrystal).parentTileY = j;
            }
        }
    }
}
