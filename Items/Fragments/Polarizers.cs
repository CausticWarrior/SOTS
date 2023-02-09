using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Items.Earth;
using SOTS.Items.Pyramid;
using SOTS.Items.ChestItems;
using SOTS.Items.AbandonedVillage;

namespace SOTS.Items.Fragments
{
	public class WorldlyPolarizer : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worldly Polarizer");
			Tooltip.SetDefault("Increases damage, endurance, and movement speed by 3%\nChanges the effects of worldly dissolving elements in your inventory");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 18;     
            Item.height = 24;   
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 0.03f;
			player.endurance += 0.03f;
			player.moveSpeed += 0.03f;
			DissolvingElementsPlayer DEP = DissolvingElementsPlayer.ModPlayer(player);
			DEP.PolarizeNature = true;
			DEP.PolarizeEarth = true;
			DEP.PolarizeDeluge = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<DissolvingNature>(1).AddIngredient<DissolvingEarth>(1).AddIngredient<DissolvingDeluge>(1).AddIngredient<AncientSteelBar>(5).AddIngredient<VibrantBar>(2).AddTile(TileID.Anvils).Register();
		}
	}
	public class ThermalPolarizer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermal Polarizer");
			Tooltip.SetDefault("Increases max life, mana, and void by 20\nChanges the effects of thermal dissolving elements in your inventory");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 32;
			Item.height = 20;
			Item.value = Item.sellPrice(gold: 7, silver: 50);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.statLifeMax2 += 20;
			player.statManaMax2 += 20;
			VoidPlayer.ModPlayer(player).voidMeterMax2 += 20;
			DissolvingElementsPlayer DEP = DissolvingElementsPlayer.ModPlayer(player);
			DEP.PolarizeAurora = true;
			DEP.PolarizeNether = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<DissolvingNether>(1).AddIngredient<DissolvingAurora>(1).AddIngredient<AncientSteelBar>(5).AddIngredient<Permafrost.FrigidBar>(2).AddIngredient(ItemID.HellstoneBar, 2).AddTile(TileID.MythrilAnvil).Register();
		}
	}
	public class ExoticPolarizer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Exotic Polarizer");
			Tooltip.SetDefault("Increases jump speed, life regen, and void gain by 2\nChanges the effects of exotic dissolving elements in your inventory");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 52;
			Item.height = 28;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Cyan;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.jumpSpeedBoost += 2;
			player.lifeRegen += 2;
			VoidPlayer.ModPlayer(player).bonusVoidGain += 2;
			DissolvingElementsPlayer DEP = DissolvingElementsPlayer.ModPlayer(player);
			DEP.PolarizeAether = true;
			DEP.PolarizeBrilliance = true;
			DEP.PolarizeUmbra = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<DissolvingAether>(1).AddIngredient<DissolvingBrilliance>(1).AddIngredient<DissolvingUmbra>(1).AddIngredient<AncientSteelBar>(5).AddIngredient<Chaos.PhaseBar>(2).AddTile(TileID.MythrilAnvil).Register();
		}
	}
	public class UltimatePolarizer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ultimate Polarizer");
			Tooltip.SetDefault("Increases damage, endurance, and movement speed by 3%\nIncreases max life, mana, and void by 20\nIncreases jump speed, life regen, and void gain by 2\nChanges the effects of all dissolving elements in your inventory");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 30;
			Item.height = 50;
			Item.value = Item.sellPrice(gold: 20);
			Item.rare = ItemRarityID.Cyan;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 0.03f;
			player.endurance += 0.03f;
			player.moveSpeed += 0.03f;
			player.statLifeMax2 += 20;
			player.statManaMax2 += 20;
			VoidPlayer.ModPlayer(player).voidMeterMax2 += 20;
			player.jumpSpeedBoost += 2;
			player.lifeRegen += 2;
			VoidPlayer.ModPlayer(player).bonusVoidGain += 2;
			DissolvingElementsPlayer DEP = DissolvingElementsPlayer.ModPlayer(player);
			DEP.PolarizeBrilliance = DEP.PolarizeNether = DEP.PolarizeUmbra = DEP.PolarizeDeluge = DEP.PolarizeAether = DEP.PolarizeAurora = DEP.PolarizeEarth = DEP.PolarizeNature = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<ExoticPolarizer>(1).AddIngredient<ThermalPolarizer>(1).AddIngredient<WorldlyPolarizer>(1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}