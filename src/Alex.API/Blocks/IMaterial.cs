﻿using Alex.API.Utils;
using Microsoft.Xna.Framework;

namespace Alex.API.Blocks
{
	public interface IMaterial
	{
		IMapColor MapColorValue { get; }
		
		TintType TintType { get; }
		Color TintColor { get; }
		IMaterial SetTintType(TintType type, Color color);
		
		double Slipperiness { get; }

		IMaterial SetSlipperines(double value);
		
		float Hardness { get; }
		IMaterial SetHardness(float hardness);
		
		bool BlocksLight { get; }
		bool BlocksMovement { get; }
		IMaterial SetTranslucent();
		IMaterial SetRequiresTool();
		IMaterial SetBurning();
		bool CanBurn { get; }
		bool IsLiquid { get; }
		bool IsOpaque { get; }
		bool IsReplaceable { get; }
		bool IsSolid { get; }
		bool IsToolRequired { get; }
		IMaterial SetReplaceable();

		bool IsWatterLoggable { get; }

		IMaterial SetWaterLoggable();

		bool CanUseTool(ItemType type, ItemMaterial material);
		IMaterial SetRequiredTool(ItemType type, ItemMaterial material);
		
		IMaterial Clone();
	}

	public interface IMapColor
	{
		int GetMapColor(int index);
	}
}
