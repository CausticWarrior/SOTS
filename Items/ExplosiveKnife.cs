using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Items.GhostTown;

namespace SOTS.Items
{
	public class ExplosiveKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosive Knife");
			Tooltip.SetDefault("'Quite a deadly combination'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.ThrowingKnife);
			Item.damage = 15;
			Item.useTime = 17;
			Item.useAnimation = 17;
			Item.DamageType = DamageClass.Throwing;
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<Projectiles.ExplosiveKnife>(); 
            Item.shootSpeed = 12f;
			Item.consumable = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			return true; 
		}
		public override void AddRecipes()
		{
			CreateRecipe(15).AddIngredient(ItemID.ThrowingKnife, 15).AddIngredient(ItemID.Grenade, 15).AddIngredient(ModContent.ItemType<AncientSteelBar>(), 1).AddTile(TileID.Anvils).Register();
		}
	}
}