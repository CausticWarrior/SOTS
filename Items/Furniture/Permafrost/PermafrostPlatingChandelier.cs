using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using SOTS.Items.Fragments;

namespace SOTS.Items.Furniture.Permafrost
{
	public class PermafrostPlatingChandelier : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(30, 22);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<PermafrostPlatingChandelierTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PermafrostPlating>(), 4).AddIngredient(ItemID.Torch, 4).AddIngredient(ItemID.Chain, 1).AddTile(TileID.Anvils).Register();
		}
	}
	public class PermafrostPlatingChandelierTile : Chandelier<PermafrostPlatingChandelier>
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		protected override Vector3 LightClr => SOTSTile.PermafrostPlatingLight * 3f;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            for (int k = 0; k < 5; k++)
            {
                SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, new Color(30, 30, 60, 0), Main.rand.NextVector2Circular(1, 1) * (k * 0.25f));
            }
        }
    }
}