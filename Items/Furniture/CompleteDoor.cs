using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture
{
    public abstract class CompleteDoor<TDrop, TOpen> : ModTile where TDrop : ModItem where TOpen : ModTile
    {
        protected virtual Color MapColor => new Color(191, 142, 111, 255);
        public abstract class OpenVariant<TClosed> : ModTile where TClosed : ModTile
        {
            protected virtual Color MapColor => new Color(191, 142, 111, 255);
            public override void SetDefaults()
            {
                Main.tileFrameImportant[Type] = true;
                Main.tileSolid[Type] = false;
                Main.tileNoSunLight[Type] = true;
                TileObjectData.newTile.Width = 2;
                TileObjectData.newTile.Height = 3;
                TileObjectData.newTile.Origin = new Point16(0, 0);
                TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 0);
                TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
                TileObjectData.newTile.UsesCustomCanPlace = true;
                TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
                TileObjectData.newTile.CoordinateWidth = 16;
                TileObjectData.newTile.CoordinatePadding = 2;
                TileObjectData.newTile.StyleHorizontal = true;
                TileObjectData.newTile.StyleMultiplier = 2;
                TileObjectData.newTile.StyleWrapLimit = 2;
                TileObjectData.newTile.Direction = TileObjectDirection.PlaceRight;
                TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
                TileObjectData.newAlternate.Origin = new Point16(0, 1);
                TileObjectData.addAlternate(0);
                TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
                TileObjectData.newAlternate.Origin = new Point16(0, 2);
                TileObjectData.addAlternate(0);
                TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
                TileObjectData.newAlternate.Origin = new Point16(1, 0);
                TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
                TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
                TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
                TileObjectData.addAlternate(1);
                TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
                TileObjectData.newAlternate.Origin = new Point16(1, 1);
                TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
                TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
                TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
                TileObjectData.addAlternate(1);
                TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
                TileObjectData.newAlternate.Origin = new Point16(1, 2);
                TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
                TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
                TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
                TileObjectData.addAlternate(1);
                TileObjectData.addTile(Type);
                AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
                TileID.Sets.HousingWalls[Type] = true; //needed for non-solid blocks to count as walls
                TileID.Sets.HasOutlines[Type] = true;
                AddMapEntry(MapColor, CreateMapEntryName(GetType().Name));
                disableSmartCursor = true;
                adjTiles = new int[] { TileID.OpenDoor };
                closeDoorID = ModContent.TileType<TClosed>();
            }
            public override void NumDust(int i, int j, bool fail, ref int num)
            {
                num = 0;
            }
            public override bool HasSmartInteract()
            {
                return true;
            }
            public override void KillMultiTile(int i, int j, int frameX, int frameY)
            {
                Item.NewItem(i * 16, j * 16, 32, 48, ModContent.ItemType<TDrop>());
            }
            public override void MouseOver(int i, int j)
            {
                Player player = Main.LocalPlayer;
                player.noThrow = 2;
                player.showItemIcon = true;
                player.showItemIcon2 = ModContent.ItemType<TDrop>();
            }
        }
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.NotReallySolid[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(0, 1);
            TileObjectData.addAlternate(0);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(0, 2);
            TileObjectData.addAlternate(0);
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
            AddMapEntry(MapColor, CreateMapEntryName(GetType().Name));
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.ClosedDoor };
            openDoorID = ModContent.TileType<TOpen>();
        }
        public override bool HasSmartInteract()
        {
            return true;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 0;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 16, 48, ModContent.ItemType<TDrop>());
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = ModContent.ItemType<TDrop>();
        }
    }
}