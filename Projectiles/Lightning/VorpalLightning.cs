using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Lightning
{
	public class VorpalLightning : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vorpal Thunder");
		}
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.DamageType = ModContent.GetInstance<Void.VoidMelee>();
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 120;
			Projectile.scale = 1.75f;
		}
		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}
		Vector2[] trailPos = new Vector2[200];
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce || !hit)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Color color = new Color(90, 140, 90, 0);
				float scale = Projectile.scale * 0.6f;
				Vector2 drawPos;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = Projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length);
				float max = betweenPositions.Length() / (texture.Width * scale * 0.5f);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 4; j++)
					{
						if (trailPos[k] != Projectile.Center)
							Main.spriteBatch.Draw(texture, drawPos + Main.rand.NextVector2Circular(1, 1) * scale, null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
			return false;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		bool runOnce = true;
		Vector2 addPos = Vector2.Zero;
		Vector2 originalVelo = Vector2.Zero;
		Vector2 originalPos = Vector2.Zero;
		Vector2 nextPos = Vector2.Zero;
		int[] randStorage = new int[200];
		int dist = 200;
		int counter = 0;
		bool hit = false;
		public override void AI()
		{
			NPC target = Main.npc[(int)Projectile.ai[0]];
			if (runOnce)
			{
				Projectile.velocity = target.Center - Projectile.Center;
				for (int i = 0; i < randStorage.Length; i++)
				{
					randStorage[i] = Main.rand.Next(-45, 46);
				}
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				nextPos = target.Center;
				originalVelo = Projectile.velocity.SafeNormalize(Vector2.Zero) * 8f;
				originalPos = Projectile.Center;
				runOnce = false;
			}

			Vector2 temp = originalPos;
			addPos = Projectile.Center;
			for (int i = 0; i < dist; i++)
			{
				bool collided = false;
				originalPos += originalVelo;

				for (int reps = 0; reps < 3; reps++)
				{
					if (counter < 2)
						nextPos = target.Center;
					Vector2 attemptToPosition = nextPos - addPos;
					addPos += new Vector2(originalVelo.Length(), 0).RotatedBy(attemptToPosition.ToRotation() + MathHelper.ToRadians(randStorage[i]));
					trailPos[i] = addPos;
				}
				NPC npc = target;
				if (npc.active && npc.Hitbox.Intersects(new Rectangle((int)addPos.X - 12, (int)addPos.Y - 12, 24, 24)) && !npc.friendly)
				{
					if (Projectile.owner == Main.myPlayer && Projectile.friendly)
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), addPos.X, addPos.Y, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<VorpalLightningDamage>(), Projectile.damage, 3f, Main.myPlayer, (int)Projectile.knockBack, Projectile.ai[1] - 1);
					if(Projectile.friendly)
                    {
						hit = true;
						collided = true;
					}
					Projectile.friendly = false;
					for (int k = i + 1; k < trailPos.Length; k++)
					{
						trailPos[k] = Vector2.Zero;
					}
				}
				if (collided)
				{
					dist = i + 1;
					break;
				}
			}
			originalPos = temp;
			Projectile.alpha += 4;
			if (Projectile.alpha >= 255)
				Projectile.Kill();
			counter++;
			Projectile.scale *= 0.98f;
			if(counter >= 2)
				Projectile.friendly = false;
		}
	}
}