﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.Items.Furniture.Permafrost
{
	public class PermafrostPlatingBed : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(34, 20);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<PermafrostPlatingBedTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PermafrostPlating>(), 15).AddIngredient(ItemID.Silk, 5).AddTile(TileID.Anvils).Register();
		}
	}
	public class PermafrostPlatingBedTile : Bed<PermafrostPlatingBed>
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
    }
}