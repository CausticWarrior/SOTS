using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.NPCs.Boss;
using SOTS.NPCs.Boss.Lux;
using SOTS.Projectiles.Chaos;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class ChaosSpirit : ModNPC
	{
		float wingSpeedMult = 1f;
		float wingHeight; //wings offset in degrees
		Vector2 baseVelo = Vector2.Zero;
		public void WingStuff()
		{
			npc.ai[3] += 1.5f;
			npc.ai[2] += 7.5f * wingSpeedMult;
			float dipAndRise = (float)Math.Sin(MathHelper.ToRadians(npc.ai[2]));
			//dipAndRise *= (float)Math.sqrt(dipAndRise);
			wingHeight = 19 + dipAndRise * 27;
			baseVelo *= 0.935f;
			baseVelo += npc.velocity.SafeNormalize(Vector2.Zero) * (float)Math.Sqrt(npc.velocity.Length());
		}
		Vector2 lastCenter = Vector2.Zero;
		public void DrawChains(bool doDust = false)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/ChaosSpiritChain");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 ownerCenter = lastCenter;
			float dynamicScaling = (float)Math.Sin(MathHelper.ToRadians(npc.ai[2] * 0.15f)) * 10;
			float moreScaling = 1.1f - 0.1f * Math.Abs(dynamicScaling) / 10f;
			float tightenScale = ((ownerCenter - npc.Center).Length() - 300) / 80f;
			tightenScale = 1 - tightenScale;
			tightenScale = MathHelper.Clamp(tightenScale, 0, 1);
			Vector2 p0 = ownerCenter;
			Vector2 p1 = ownerCenter - baseVelo.RotatedBy(MathHelper.ToRadians(180 + dynamicScaling)) * 4f * moreScaling * tightenScale;
			Vector2 p2 = npc.Center - baseVelo * 8f * moreScaling * tightenScale;
			Vector2 p3 = npc.Center;
			int segments = 16;
			for (int i = 0; i < segments; i++)
			{
				float t = i / (float)segments;
				Vector2 drawPos2 = SOTS.CalculateBezierPoint(t, p0, p1, p2, p3);
				t = (i + 1) / (float)segments;
				Vector2 drawPosNext = SOTS.CalculateBezierPoint(t, p0, p1, p2, p3);
				if (!doDust)
				{
					float rotation = (drawPos2 - drawPosNext).ToRotation();
					for (int k = 0; k < 6; k++)
					{
						Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60)) * 0.5f;
						color.A = 0;
						Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
						Main.spriteBatch.Draw(texture, drawPos2 - Main.screenPosition + circular, null, color, rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
					}
				}
				else
				{
					for (int k = 0; k < 10; k++)
					{
						Dust dust = Dust.NewDustDirect(drawPos2, 0, 0, DustID.RainbowMk2);
						dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 1.8f;
						dust.velocity *= 2.2f;
					}
				}
			}
		}
		public static void DrawWings(float wingHeight, float dipAndRiseCounter, float baseRotation, Vector2 center, Color overrideColor, float scale = 1f)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/ChaosParticle");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float dipAndRise = (float)Math.Sin(MathHelper.ToRadians(dipAndRiseCounter));
			float supposedWingHeight = wingHeight - 19;
			dipAndRise = supposedWingHeight / 27f;
			dipAndRise = MathHelper.Clamp(dipAndRise, -1, 1);
			int width = 130;
			int height = 90;
			int amtOfParticles = 120;
			for (int j = -1; j <= 1; j += 2)
			{
				float positionalRotation = baseRotation + MathHelper.ToRadians(wingHeight * -j);
				Vector2 position = center + new Vector2(-28 * j, 16).RotatedBy(baseRotation) + new Vector2(((width + 70) / 2 + 54) * j, 0).RotatedBy(positionalRotation);
				float degreesCount = 0f;
				float flapMult = 0.0f;
				float baseGrowth = 0.35f;
				float scaleGrowth = 0.15f;
				float totalSin = 450f;
				if (dipAndRise < 0)
				{
					baseGrowth = MathHelper.Lerp(baseGrowth, 0.2f, -dipAndRise);
					scaleGrowth = MathHelper.Lerp(scaleGrowth, 0.3f, -dipAndRise);
					totalSin = MathHelper.Lerp(totalSin, 405, (float)Math.Pow((-dipAndRise), 1.2f));
				}
				else
				{
					baseGrowth = MathHelper.Lerp(baseGrowth, 0.1f, dipAndRise);
					scaleGrowth = MathHelper.Lerp(scaleGrowth, 0.05f, dipAndRise);
					flapMult = MathHelper.Lerp(scaleGrowth, 0.3f, (float)Math.Pow(dipAndRise, 1.2f));
				}
				for (float i = 0; i < amtOfParticles;)
				{
					float sinusoid = (float)Math.Sin(MathHelper.ToRadians(degreesCount));
					if (degreesCount < 0)
						sinusoid = 0;
					float radians = MathHelper.ToRadians(i * 360f / amtOfParticles);
					Color c = VoidPlayer.pastelAttempt(radians + MathHelper.ToRadians(Main.GameUpdateCount), overrideColor);
					Vector2 circular = new Vector2(-1, 0).RotatedBy(radians);
					float increaseAmount = 1f;
					if (i < amtOfParticles / 2)
					{
						degreesCount = MathHelper.Lerp(0, totalSin, (float)Math.Pow(i / amtOfParticles * 2f, 1.2f));
						circular.Y *= flapMult;
						circular.Y -= sinusoid * (baseGrowth + scaleGrowth * i / amtOfParticles);
					}
					else
					{
						float mult = (1 - (i - 60f) / amtOfParticles * 7f);
						if (mult < 0)
							mult = 0;
						circular.Y -= (float)Math.Sin(MathHelper.ToRadians(totalSin)) * (scaleGrowth + baseGrowth) * mult;
						if (circular.Y > 0)
						{
							sinusoid = (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * 20f + i * 36));
							if (sinusoid < 0.0f)
							{
								if (sinusoid > -0.2f)
									increaseAmount = 0.35f;
								else if (sinusoid > -0.4f)
									increaseAmount = 0.5f;
								else if (sinusoid > -0.6f)
									increaseAmount = 0.75f;
								sinusoid = 0.0f;
							}
							else
							{
								increaseAmount = 0.25f;
							}
							circular.Y *= 1f + sinusoid * 0.4f;
						}
					}
					i += increaseAmount;
					circular.X *= width / 2 * j;
					circular.Y *= height / 2;
					circular = circular.RotatedBy(positionalRotation);
					Main.spriteBatch.Draw(texture, position - Main.screenPosition + circular, null, new Color(c.R, c.G, c.B, 0), radians * j, origin, scale * 0.8f * (0.5f + 0.5f * (float)Math.Sqrt(increaseAmount)), SpriteEffects.None, 0f);
				}
			}
		}
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[npc.type] = 1;
			DisplayName.SetDefault("Chaos Spirit");
			NPCID.Sets.TrailCacheLength[npc.type] = 5;  
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 0;
            npc.lifeMax = 3000; 
            npc.damage = 100; 
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 70;
            npc.height = 70;
            npc.value = Item.buyPrice(0, 0, 0, 0);
            npc.npcSlots = 10f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = false;
			npc.rarity = 2;
		}
		bool rubbleActive = true;
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.damage = 150;
			npc.lifeMax = 4000;
		}
        public override bool PreAI()
		{
			Player player = Main.player[npc.target];
			WingStuff();
			int rubble = (int)npc.ai[0];
			bool ownerActive = false;
			NPC owner;
			if (rubble >= 0)
            {
				owner = Main.npc[rubble];
				if(owner.active && owner.type == ModContent.NPCType<ChaosRubble>())
                {
					Vector2 toRubble = owner.Center - npc.Center;
					if(toRubble.Length() > 560)
                    {
						float fromRubble = toRubble.Length() - 560;
						npc.Center += toRubble.SafeNormalize(Vector2.Zero) * fromRubble;
                    }
					ownerActive = true;
				}
				lastCenter = owner.Center;
			}
			if (!ownerActive)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.netUpdate = true;
				}
				npc.dontTakeDamage = true;
				npc.velocity.Y -= 0.026f;
				npc.velocity.X *= 1.095f;
				if(Main.netMode != NetmodeID.Server && rubble != -2 && rubbleActive)
				{
					DrawChains(true);
					rubbleActive = false;
					npc.ai[0] = -2;
				}
			}
			else
			{
				owner = Main.npc[rubble];
				npc.ai[1]++;
				DoPassiveMovement(Vector2.Lerp(player.Center, owner.Center, 0.6f));
			}
			npc.velocity.Y -= 0.014f;
			npc.rotation = npc.velocity.X * 0.06f;
			return true;
		}
		public void DoPassiveMovement(Vector2 center)
		{
			float sinusoid = (float)Math.Sin(MathHelper.ToRadians(npc.ai[3]));
			Vector2 offset = new Vector2(0, -300).RotatedBy(MathHelper.ToRadians(sinusoid * 30));
			offset.X *= 1.5f;
			Vector2 position = center + offset;
			float verticalOffset = 40 + 40 * (float)Math.Sin(MathHelper.ToRadians(npc.ai[1] * 2f));
			position.Y -= verticalOffset;
			Vector2 goTo = position - npc.Center;
			float speed = 13f;
			if (goTo.Length() < speed)
			{
				speed = goTo.Length();
			}
			npc.velocity *= 0.3f;
			npc.velocity += 0.6f * speed * goTo.SafeNormalize(Vector2.Zero);
		}
		public override void AI()
		{
			int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.RainbowMk2);
			Dust dust = Main.dust[dust2];
			dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < npc.oldPos.Length; k++) {
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Color color = VoidPlayer.pastelRainbow * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
				color.A = 0;
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, npc.rotation, drawOrigin, npc.scale * 1.1f, SpriteEffects.None, 0f);
			}
			if((int)npc.ai[0] != -2 && lastCenter != Vector2.Zero)
				DrawChains();
			DrawWings(wingHeight, npc.ai[2], npc.rotation, npc.Center, Color.White);
			return false;
		}	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				if(Main.netMode	!= NetmodeID.Server)
				{
					for (int i = 0; i < 50; i++)
					{
						Dust dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.RainbowMk2);
						dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 2.2f;
						dust.velocity *= 4f;
					}
					DrawChains(true);
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				Color color = new Color(100, 100, 100, 0);
				Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
				if (k != 0)
				{
					color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
					color.A = 0;
				}
				else
					circular *= 0f;
				Main.spriteBatch.Draw(texture, npc.Center + circular - Main.screenPosition, null, color, 0f, drawOrigin, npc.scale * 1.1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void NPCLoot()
		{
			int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<Lux>(), 0, npc.ai[2]);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				Main.npc[n].netUpdate = true;
		}
    }
}
