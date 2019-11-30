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
namespace SOTS.Projectiles.Crushers
{    
    public class IrradiatedArm : ModProjectile 
    {	bool flip = false;
		bool initiateDamage = true;
		int initialDamage;
		float increaseDamage = 1f;
		float initiateClap = -5;
		float accelerate = 0;
		float accelerateAmount = 0;
		float rotateFloat;
		float rotateAmount;
		float rotateCurrent;
		int initiateTimer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("IrradiatedArm");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 26;
			projectile.width = 32;
			projectile.penetrate = 24;
			projectile.friendly = false;
			projectile.timeLeft = 6004;
			projectile.tileCollide = false;
			projectile.hostile = false;
		}
		public override bool PreAI()
        {
			if(initiateDamage)
			{
				initiateDamage = false;
				initialDamage = projectile.damage;
			}
			if(rotateCurrent <= 170 && initiateClap == -5)
			{
				increaseDamage = (float)((8f/170f * rotateCurrent) + 1);
			}
			else if(rotateCurrent > 170 && initiateClap == -5)
			{
				rotateCurrent = 170;
				increaseDamage = 9f; //setting the full charge value differently because it was not neccessary last time
			}
			if(increaseDamage >= 8.7f && initiateClap == -5)
			{
				initiateTimer += 1;
			}
			projectile.damage = (int)(initialDamage * increaseDamage);
			
			rotateAmount = 2.1f;
			
			if(projectile.active)
			return true;
		
			return false;
		}
		public override void AI()
		{
			Player player  = Main.player[projectile.owner];
			
			Vector2 cursorArea;
						
							if (player.gravDir == 1f)
						{
						cursorArea.Y = (float)Main.mouseY + Main.screenPosition.Y;
						}
						else
						{
						cursorArea.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
						}
							cursorArea.X = (float)Main.mouseX + Main.screenPosition.X;
			
					   float shootToX = cursorArea.X - player.Center.X;
					   float shootToY = cursorArea.Y - player.Center.Y;
					   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

					   distance = 6.25f / distance;
		   
					   shootToX *= distance * 5;
					   shootToY *= distance * 5;
					   
			double startingDirection = Math.Atan2((double)-shootToY, (double)-shootToX);
			startingDirection *= 180/Math.PI;
			
			if(projectile.knockBack != -1)
			{				
			projectile.rotation = projectile.velocity.ToRotation();
			//projectile.ai[1] += projectile.rotation;
			}
			if(initiateTimer >= 40)
			{
				initiateClap = 1;
			}
			if(player.channel || projectile.timeLeft > 6001)
            {
				projectile.timeLeft = 6000;
				projectile.alpha = 0;
            }
            else
            {
				if(initiateClap == -5)
				{
					initiateClap = 1;
				}
            }
				if(projectile.knockBack == 1)
				{
					rotateFloat = 5;
					flip = true;
					projectile.spriteDirection = -1;
					projectile.knockBack = -1;
				}
				if(projectile.knockBack == 0)
				{
					rotateFloat = -5;
					projectile.knockBack = -1;
				}
				if(flip)
				{
					if(rotateCurrent < 170 && initiateClap == -5)
					{
					rotateFloat += rotateAmount;
					rotateCurrent += rotateAmount;
					}
					projectile.ai[1] = rotateFloat + (float)startingDirection;
					projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 315);
					//projectile.rotation += MathHelper.ToRadians(315);
					
				}
				else
				{
					if(rotateCurrent < 170 && initiateClap == -5)
					{
					rotateFloat -= rotateAmount;
					rotateCurrent += rotateAmount;
					}
					projectile.ai[1] = rotateFloat + (float)startingDirection;
					projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225);
					//projectile.rotation += MathHelper.ToRadians(225);
					
				}
				
			if(initiateClap == -5)
			{
				
				double deg = (double) projectile.ai[1]; 
				double rad = deg * (Math.PI / 180);
				double dist = 48;
				projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
				projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
			}
			else if(initiateClap > 0)
			{
				accelerateAmount += 0.3f;
				accelerate += accelerateAmount;
				
				if(rotateCurrent < 0)
				{
				initiateClap = -1;
					if(projectile.owner == Main.myPlayer)
					{
						double deg1 = (double)startingDirection; 
						double rad1 = deg1 * (Math.PI / 180);
						for(int i = 0; i < (int)increaseDamage; i++)
						{
							double dist1 = 48 * (i + 1);
							float positionX = player.Center.X - (int)(Math.Cos(rad1) * dist1);
							float positionY = player.Center.Y - (int)(Math.Sin(rad1) * dist1);
							Projectile.NewProjectile(positionX, positionY, 0, 0, mod.ProjectileType("IrradiatedCrush"), projectile.damage, initialDamage, Main.myPlayer, 0f, 0f);
						}
					}
					projectile.Kill();
				}
				if(flip)
				{
				projectile.ai[1] = rotateFloat -accelerate + (float)startingDirection;;
				rotateCurrent -= accelerateAmount;
				projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 315);
				}
				if(!flip)
				{
				projectile.ai[1] = rotateFloat +accelerate + (float)startingDirection;;
				rotateCurrent -= accelerateAmount;
				projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225);
				}
				
				double deg = (double) projectile.ai[1]; 
				double rad = deg * (Math.PI / 180);
				double dist = 48;
				projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
				projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
			}
			
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //target.immune[projectile.owner] = 15;
        }
	}
}
		