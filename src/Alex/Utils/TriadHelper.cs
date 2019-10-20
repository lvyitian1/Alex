// /*
//  * Alex
//  *
//  * Copyright (c) 2019 Dan Spiteri
//  *
//  * /

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Alex.Utils
{
	public static class TriadHelper
	{
		const float TextSize = 0.1f;

		public static void DrawTriad(this GraphicsDevice device)
		{
			// Y
			DrawLine(device, Vector3.Zero, Vector3.UnitY,      Color.Green);
			DrawLine(device, Vector3.UnitY + new Vector3(-TextSize/2f, TextSize, 0f), Vector3.Up + new Vector3(0f, TextSize/2f, 0f), Color.Green);
			DrawLine(device, Vector3.UnitY + new Vector3(-TextSize/2f, 0f, 0f), Vector3.Up + new Vector3(TextSize/2f, TextSize, 0f), Color.Green);
			
			// Z
			DrawLine(device, Vector3.Zero, Vector3.UnitZ, Color.Blue);
			DrawLine(device, Vector3.UnitZ + new Vector3(-TextSize/2f, TextSize/2f, TextSize), Vector3.UnitZ + new Vector3(TextSize/2f, TextSize/2f, TextSize), Color.Blue);
			DrawLine(device, Vector3.UnitZ + new Vector3(TextSize/2f, TextSize/2f, TextSize), Vector3.UnitZ + new Vector3(-TextSize/2f, -TextSize/2f, TextSize), Color.Blue);
			DrawLine(device, Vector3.UnitZ + new Vector3(-TextSize/2f, -TextSize/2f, TextSize), Vector3.UnitZ + new Vector3(TextSize/2f, -TextSize/2f, TextSize), Color.Blue);
			
			
			// X
			DrawLine(device, Vector3.Zero, Vector3.UnitX,   Color.Red);

			DrawLine(device, Vector3.UnitX + new Vector3( TextSize, TextSize/2f, 0f), Vector3.UnitX + new Vector3(0f, -TextSize/2f, 0f), Color.Red);
			DrawLine(device, Vector3.UnitX + new Vector3(TextSize, -TextSize/2f, 0f), Vector3.UnitX + new Vector3(0f, TextSize/2f, 0f), Color.Red);
		}
		
		private static void DrawLine(GraphicsDevice device, Vector3 start, Vector3 end, Color color)
		{
			var vertices = new[] { new VertexPositionColor(start, color),  new VertexPositionColor(end, color) };
			device.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
		}
	}
}