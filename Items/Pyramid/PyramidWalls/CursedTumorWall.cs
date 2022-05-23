using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Dusts;

namespace SOTS.Items.Pyramid.PyramidWalls
{
	public class CursedTumorWall : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<CursedTumorWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<CursedTumor>(), 1).AddTile(TileID.WorkBenches).Register();
			CreateRecipe(1).AddIngredient(this, 4).AddTile(TileID.WorkBenches).ReplaceResult(ModContent.ItemType<CursedTumor>());
		}
	}
	public class UnsafeCursedTumorWall : ModItem
	{
        public override string Texture => "SOTS/Items/Pyramid/PyramidWalls/CursedTumorWall";
        public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Changes the biome to pyramid when in front of\nAlso envokes the Pharaoh's Curse");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Red;
			Item.createWall = ModContent.WallType<UnsafeCursedTumorWallWall>();
		}
	}
	public class CursedTumorWallWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<CurseDust3>();
			drop = ModContent.ItemType<CursedTumorWall>();
			SoundType = SoundID.NPCHit;
			SoundStyle = 1;
			AddMapEntry(new Color(49, 33, 75));
		}
	}
	public class UnsafeCursedTumorWallWall : ModWall
	{
        public override bool Autoload(ref string name, ref string texture)
        {
			texture = "SOTS/Items/Pyramid/PyramidWalls/CursedTumorWallWall";
			return base.Autoload(ref name, ref texture);
        }
        public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			DustType = ModContent.DustType<CurseDust3>();
			drop = ModContent.ItemType<CursedTumorWall>();
			SoundType = SoundID.NPCHit;
			SoundStyle = 1;
			AddMapEntry(new Color(49, 33, 75));
		}
	}
}