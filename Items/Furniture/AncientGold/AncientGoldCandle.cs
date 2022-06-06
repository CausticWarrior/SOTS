using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.AncientGold
{
    public class AncientGoldCandle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Gold Candle");
            Tooltip.SetDefault("");
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(16, 20);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<AncientGoldCandleTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<RoyalGoldBrick>(), 4).AddIngredient(ModContent.ItemType<AncientGoldTorch>(), 1).AddTile(TileID.WorkBenches).Register();
        }
    }
    public class AncientGoldCandleTile : Candle<AncientGoldCandle>
    {
        protected override Vector3 LightClr => new Vector3(1.1f, 0.9f, 0.9f);
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("_Flame"));
            for (int k = 0; k < 5; k++)
            {
                SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, new Color(100, 100, 100, 0), Main.rand.NextVector2Circular(1, 1) * (k * 0.25f));
            }
        }
    }
}