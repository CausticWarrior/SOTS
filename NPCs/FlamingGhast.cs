using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Pyramid;
using SOTS.Items.Void;
using SOTS.Projectiles.Pyramid;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class FlamingGhast : ModNPC
	{	float ai1 = 0;
		public override void SetDefaults()
		{
            NPC.aiStyle =0; 
            NPC.lifeMax = 185;   
            NPC.damage = 45; 
            NPC.defense = 20;  
            NPC.knockBackResist = 0f;
            NPC.width = 48;
            NPC.height = 56;
			Main.npcFrameCount[NPC.type] = 4;  
            NPC.value = 4450;
            NPC.npcSlots = 0.6f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = true;
            NPC.netUpdate = true;
			Banner = NPC.type;
			BannerItem = ItemType<FlamingGhastBanner>();
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 8);
			Vector2 drawPos = NPC.Center - screenPos;
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, 48, 56), drawColor, NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/FlamingGhastGlow");
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, 48, 56), Color.White, NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			bool lineOfSight = Collision.CanHitLine(player.position, player.width, player.height, NPC.position, NPC.width, NPC.height);
			float speed = 0.65f + 0.35f * (float)Math.Sin(MathHelper.ToRadians(ai1 * 3));
			Vector2 toPlayer = player.Center - NPC.Center;
			float length = toPlayer.Length();
			if (lineOfSight || length <= 640)
			{
				if (length > 320 && !lineOfSight)
					speed *= 0.5f;
				toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
				NPC.velocity.Y *= 0.98f;
				NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 0.15f * speed;
            }
			NPC.velocity.Y += 0.02f * (float)Math.Sin(MathHelper.ToRadians(ai1 * 6));
			for(int i = 0; i < 2; i++)
			{
				int num1 = Dust.NewDust(NPC.position, NPC.width - 4, 24, ModContent.DustType<CurseDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity.X = NPC.velocity.X;
				Main.dust[num1].velocity.Y = -3 + i * 1.5f;
				Main.dust[num1].scale *= 1.25f + i * 0.15f;
			}
			ai1++;
			if (ai1 >= 600)
            {
				ai1 = 0;
			}
			//Main.NewText(ai1);
			/*if ((int)ai1 % 600 == 480 && (lineOfSight || length <= 320))
			{
				//Main.NewText("bruh");
				if (Main.netMode != 1)
					for (int i = 0; i < 8; i ++)
					{
						Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileType<GhastDrop>(), damage2, 1f, Main.myPlayer, i * 45f, npc.whoAmI);
					}
            }
			else 
			{
			}*/
			if (Main.netMode != NetmodeID.MultiplayerClient && Main.rand.NextBool(75))
			{
				int damage = SOTSNPCs.GetBaseDamage(NPC) / 2;
				Vector2 spawn = (NPC.position + new Vector2(8, 8) + new Vector2(Main.rand.Next(NPC.width - 16), Main.rand.Next(NPC.height - 16)));
				Projectile.NewProjectile(NPC.GetSource_FromAI(), spawn, NPC.velocity * Main.rand.NextFloat(-0.1f, 0.1f), ProjectileType<GhastDrop>(), damage, 1f, Main.myPlayer, -1, -1f);
			}
			NPC.velocity = Collision.TileCollision(NPC.position + new Vector2(8, 8), NPC.velocity, NPC.width - 16, NPC.height - 16, true);
		}
		public override void FindFrame(int frameHeight) 
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= 5f) 
			{
				NPC.frameCounter -= 5f;
				NPC.frame.Y += frameHeight;
				if(NPC.frame.Y >= 4 * frameHeight)
				{
					NPC.frame.Y = 0;
				}
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemType<CursedMatter>(), 1, 2, 3));
			npcLoot.Add(ItemDropRule.Common(ItemID.CursedFlame, 1, 2, 5));
			npcLoot.Add(ItemDropRule.Common(ItemType<CursedCaviar>(), 10, 1, 1));
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while (num < damage / NPC.lifeMax * 50.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CurseDust>(), (float)(2 * hitDirection), -2f);
					num++;
				}
			}
            else
			{
				for (int k = 0; k < 30; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CurseDust>(), (float)(2 * hitDirection), -2f);
				}
			}		
		}
	}
}





















