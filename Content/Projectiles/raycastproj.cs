﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectInfinity.Content.Projectiles.BaseProjectiles;
using ProjectInfinity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ProjectInfinity.Content.Projectiles
{
    internal class raycastproj : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectiles + Name;
        public float maxDist { get; set; }
        public static Rectangle checkBox = new();
        
        public float Distance
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        private const float MOVE_DISTANCE = 60f;
        public int ParentIndex
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 480;
            Projectile.penetrate = 1;
        }
        
        public override void AI()
        {
            Projectile proj = Main.projectile[ParentIndex];
            checkBox = new Rectangle((int)proj.position.X, (int)proj.position.Y, 10, 10);

            if (!proj.active)
            {
                Projectile.Kill();
            }

            //Main.NewText(checkBox.BottomRight());
            UpdatePosition();
            UpdateProj(proj);
            SetLaserPosition(proj);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // We start drawing the laser if we have charged up
            Vector2 c = Main.projectile[ParentIndex].Center;
            DrawLaser(Main.spriteBatch, TextureAssets.Projectile[ModContent.ProjectileType<raycastproj>()].Value, c,
                Projectile.velocity, 2, -1.57f, 1f, (int)MOVE_DISTANCE);
            return false;

        }
        private void SetLaserPosition(Projectile proj)
        {
            for (Distance = MOVE_DISTANCE; Distance <= maxDist; Distance += 5f)
            {
                var start = proj.Center + Projectile.velocity * Distance;
                if (!Collision.CanHit(proj.Center, 1, 1, start, 1, 1))
                {
                    Distance -= 5f;
                    break;
                }
            }
        }
        public void UpdatePosition()
        {
            Projectile.Center = Main.projectile[ParentIndex].Center;
        }
        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, float rotation = 0f, float scale = 1f, int transDist = 50)
        {
            float r = unit.ToRotation() + rotation;

            // Draws the laser 'body'
            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.White;
                var origin = start + i * unit;
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 26, 22, 26), i < transDist ? Color.Transparent : c, r,
                    new Vector2(22 * .5f, 26 * .5f), scale, 0, 0);
            }

            // Draws the laser 'tail'
            spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
                new Rectangle(0, 0, 22, 26), Color.White, r, new Vector2(22 * .5f, 26 * .5f), scale, 0, 0);

            // Draws the laser 'head'
            spriteBatch.Draw(texture, start + (Distance + step) * unit - Main.screenPosition,
                new Rectangle(0, 52, 22, 26), Color.White, r, new Vector2(22 * .5f, 26 * .5f), scale, 0, 0);
        }
        public override bool ShouldUpdatePosition() => false;
        
        public void UpdateProj(Projectile proj)
        {
            Projectile.netUpdate = true;
        }
    }

}
