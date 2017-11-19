﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Virtuous.Items;

namespace Virtuous.Projectiles
{
    public class ProjTitanAOE : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Titan Shield");
        }

        private bool firstTick { get { return projectile.ai[1] == 0; } set { projectile.ai[1] = value ? 0 : 1; } } //Stores into the projectile's built-in ai[1], which is 0 by default (true in this case)

        public override void SetDefaults()
        {
            projectile.width = 300;
            projectile.height = 200;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.alpha = 255; //Transparent
            projectile.timeLeft = 10;
            if (TitanShield.ExplosionCumulativeMode)
            {
                projectile.usesLocalNPCImmunity = true; //Invincibility per individual projectile
                projectile.localNPCHitCooldown = TitanShield.AoEInvincibility;
            }
            else
            {
                projectile.usesIDStaticNPCImmunity = true; //Invincibility per projectile type
                projectile.idStaticNPCHitCooldown = TitanShield.AoEInvincibility;
            }
        }

        public override void AI()
        {
            if(firstTick)
            {
                firstTick = false;
                for(int i = 0; i < 25; i++)
                {
                    Vector2 gorePosition = projectile.Center + new Vector2(Main.rand.Next(-projectile.width / 2, +projectile.width / 2 + 1), Main.rand.Next(-projectile.height / 2, +projectile.height / 2 + 1));
                    Gore.NewGore(gorePosition, new Vector2(1, 1), Main.rand.Next(61, 63 + 1), 0.2f + Main.rand.NextFloat()*1.3f);
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, /*Type*/31, 0f, 0f, /*Alpha*/0, default(Color), Main.rand.NextFloat()*2f);
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = projectile.ai[0] == 1 ? true : false; //The crit got passed as ai[0]
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            crit = projectile.ai[0] == 1 ? true : false; //The crit got passed as ai[0]
        }
    }
}