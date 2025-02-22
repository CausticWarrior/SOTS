using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth
{
	public class VibrantBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(99);
		}
		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 36;
			Item.maxStack = 999;
			Item.consumable = true;           
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(copper: 5);
			Item.rare = ItemRarityID.Blue;
			Item.shoot = ModContent.ProjectileType<Projectiles.Earth.VibrantBullet>(); 
			Item.shootSpeed = 4f;             
			Item.ammo = AmmoID.Bullet;   
            Item.UseSound = SoundID.Item23;
		}
		public override void AddRecipes()
		{
			CreateRecipe(200).AddIngredient(ModContent.ItemType<VibrantBar>(), 1).AddTile(TileID.Anvils).Register();
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * ((255 - Item.alpha) / 255f);
		}
	}
	public class VibrantArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(99);
		}
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 26;
			Item.height = 56;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(copper: 5);
			Item.rare = ItemRarityID.Blue;
			Item.shoot = ModContent.ProjectileType<Projectiles.Earth.VibrantArrow>();
			Item.shootSpeed = 5f;
			Item.ammo = AmmoID.Arrow;
			Item.UseSound = SoundID.Item23;
		}
		public override void AddRecipes()
		{
			CreateRecipe(200).AddIngredient(ModContent.ItemType<VibrantBar>(), 1).AddTile(TileID.Anvils).Register();
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * ((255 - Item.alpha) / 255f);
		}
	}
}