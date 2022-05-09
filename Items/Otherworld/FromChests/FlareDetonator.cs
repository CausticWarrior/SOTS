using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class FlareDetonator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flare Detonator");
			Tooltip.SetDefault("Fire an explosive flare that detonates upon releasing the trigger");
		}
		public override void SetDefaults()
		{
			Item.noMelee = true;
			Item.damage = 23;  
            Item.DamageType = DamageClass.Ranged;    
            Item.width = 44;  
            Item.height = 26;   
            Item.useTime = 10;  
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 5f;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item11;
            Item.autoReuse = false;
            Item.shoot = 10; 
            Item.shootSpeed = 8f;
			Item.reuseDelay = 8;
			Item.useAmmo = AmmoID.Flare;
			Item.noUseGraphic = true;
			Item.channel = true;
		}
        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			Item.useTime = 10;
			Item.useAnimation = 10;
			base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int ai = 2;
			if (type == ProjectileID.BlueFlare)
			{
				ai = 1;
				speedX *= 0.65f;
				speedY *= 0.65f;
			}
			if (type == ProjectileID.Flare)
			{
				ai = 0;
				speedX *= 1.7f;
				speedY *= 1.7f;

			}
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), mod.ProjectileType("BombFlare"), damage, knockBack, player.whoAmI, ai);
			speedX *= 0f;
			speedY *= 0f;
			type = mod.ProjectileType("FlareDetonatorHold");
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FlareGun, 1);
			recipe.AddIngredient(null, "OtherworldlyAlloy", 12);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
