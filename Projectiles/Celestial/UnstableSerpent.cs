using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Celestial
{    
    public class UnstableSerpent : ModProjectile 
    {	
		int worm = 0;
		int wait = 1;         
		float oldVelocityY = 0;	
		float oldVelocityX = 0;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Serpent");
			
		}
		
        public override void SetDefaults()
        {
			projectile.width = 48;
			projectile.height = 58;
            Main.projFrames[projectile.type] = 3;
			projectile.friendly = false;
			projectile.timeLeft = 720;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 145;
			projectile.netImportant = true;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit) 
		{
			target.AddBuff(31, 90, false);
		}	
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(projectile.rotation);
			writer.Write(projectile.spriteDirection);
			writer.Write(projectile.frame);
			writer.Write(projectile.timeLeft);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			projectile.rotation = reader.ReadSingle();
			projectile.spriteDirection = reader.ReadInt32();
			projectile.frame = reader.ReadInt32();
			projectile.timeLeft = reader.ReadInt32();
		}
		public override void AI()
		{
			float modUnit = 10f;
			if(projectile.ai[0] == 1)
			{
				projectile.netUpdate = true;
				projectile.friendly = true;
				projectile.hostile = false;
				projectile.magic = true;
				projectile.timeLeft = projectile.timeLeft > 90 ? 50 : projectile.timeLeft;
				modUnit = 5f;
			}
			else
			{
				projectile.friendly = false;
				projectile.hostile = true;
				modUnit = 10f;
			}
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 2.5f / 255f, (255 - projectile.alpha) * 1.6f / 255f, (255 - projectile.alpha) * 2.4f / 255f);
			if(projectile.frame == 0)
				projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(90);
				
			if(projectile.frame == 0 && projectile.hostile)
			{
				int Probe = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("UnstableSerpent"), projectile.damage, 0, projectile.owner, projectile.ai[0], 0);
				Main.projectile[Probe].rotation = projectile.rotation;
				Main.projectile[Probe].timeLeft = 90;
				Main.projectile[Probe].frame = 1;
			}
			else if(projectile.frame == 0 && Main.myPlayer == projectile.owner)
			{
				int Probe = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("UnstableSerpent"), projectile.damage, 0, projectile.owner, projectile.ai[0], 0);
				Main.projectile[Probe].rotation = projectile.rotation;
				Main.projectile[Probe].timeLeft = 20;
				Main.projectile[Probe].frame = 1;
			}
			if(projectile.timeLeft <= 4 && projectile.frame != 0)
			{
				projectile.frame = 2;
			}
			if(wait == 1)
			{
				wait++;
				oldVelocityY = projectile.velocity.Y;	
				oldVelocityX = projectile.velocity.X;
			}
			worm++;
			if(worm <= modUnit * 2)
			{
			projectile.velocity.X += oldVelocityY / modUnit;
			projectile.velocity.Y += -oldVelocityX / modUnit;
			}
			else if(worm > modUnit * 2 && worm <= modUnit * 4)
			{
			projectile.velocity.X += -oldVelocityY / modUnit;
			projectile.velocity.Y += oldVelocityX / modUnit;
			}
			if(worm >= modUnit * 4)
			{
			worm = 0;
			}
		}
	}
}
		