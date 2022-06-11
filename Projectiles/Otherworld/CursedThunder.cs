using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;

namespace SOTS.Projectiles.Otherworld
{
	public class CursedThunder : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder Column");
		}
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 5;
			Projectile.scale = 0.75f;
		}
		Vector2[] trailPos = new Vector2[6];
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/CursedThunder").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
                {
					return false;
                }
				Color color = new Color(50, 160, 20, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = Projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
				float max = betweenPositions.Length() / (14 * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 3; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f * scale;
						float y = Main.rand.Next(-10, 11) * 0.2f * scale;
						if (j < 2)
                        {
							x = 0;
							y = 0;
                        }
						if(trailPos[k] != Projectile.Center)
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
			return false;
		}
		bool runOnce = true;
		public void cataloguePos()
        {
			Vector2 current = Projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
        }
		public void checkPos()
		{
			bool flag = false;
			float iterator = 0f;
			Vector2 current = Projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				if (current == previousPosition)
				{
					iterator++;
				}
			}
			if(endHow == 1 && endHow != 2 && Main.rand.NextBool(3))
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 75);
				Main.dust[dust].scale *= 1f * (10f - iterator)/10f;
				Main.dust[dust].velocity *= 1f;
				Main.dust[dust].noGravity = true;
			}
			if (iterator >= 6)
				Projectile.Kill();
		}
		int endHow = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			endHow = 1;
			Projectile.tileCollide = false;
			Projectile.velocity *= 0f;
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[Projectile.owner] = 6;
			target.AddBuff(BuffID.CursedInferno, 1200, false);
		}
		int counter = 0;
		int counter2 = 0;
		Vector2 originalVelo = Vector2.Zero;
		Vector2 originalPos = Vector2.Zero;
        public override void AI()
		{
			if(Projectile.timeLeft < 220)
			{
				endHow = 2;
				Projectile.tileCollide = false;
				Projectile.velocity *= 0f;
			}
			if(runOnce)
			{
				originalVelo = Projectile.velocity;
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
				originalPos = Projectile.Center;
			}
			originalPos += originalVelo * 1.4f;
			checkPos();
			Player player = Main.player[Projectile.owner];
			Vector2 toPlayer = player.Center - Projectile.Center;
			if(counter2 > 30 - Projectile.ai[0] * 1)
            {
				if(Projectile.ai[0] > 0)
                {
					for (int i = 0; i < 3; i += 2)
					{
						if (Projectile.owner == Main.myPlayer)
						{
							Vector2 perturbedSpeed = new Vector2(originalVelo.X, originalVelo.Y).RotatedBy(MathHelper.ToRadians((i - 1) * 45f));
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<CursedThunder>(), Projectile.damage, 1f, Main.myPlayer, Projectile.ai[0] - 1);
						}
					}
				}
				Projectile.velocity *= 0f;
				originalVelo *= 0f;
				Projectile.ai[0] = 0f;
				Projectile.friendly = false;
			}
			counter++;
			counter2++;
			if (counter >= 0)
			{
				counter = -16;
				if(endHow == 0)
				{
					if (Projectile.velocity.Length() != 0f)
					{
						Vector2 toPos = originalPos - Projectile.Center;
						Projectile.velocity = new Vector2(originalVelo.Length(), 0).RotatedBy(toPos.ToRotation() + MathHelper.ToRadians(Projectile.ai[1]));
						Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
					}
					Projectile.ai[1] = Main.rand.Next(-45, 46);
				}
				cataloguePos();
			}
		}
	}
}