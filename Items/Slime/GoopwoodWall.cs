using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class GoopwoodWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(100);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<GoopwoodWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<Wormwood>(), 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ModContent.ItemType<Wormwood>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class GoopwoodWallWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = 7;
			ItemDrop = ModContent.ItemType<GoopwoodWall>();
			AddMapEntry(new Color(120, 54, 16));
		}
	}
}