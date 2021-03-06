﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Alex.Api;
using Alex.API.Blocks;
using Alex.API.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Alex
{
	public static class Extensions
	{
		static Extensions()
		{
			
		}
		
		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Xna.Framework.Vector3" /> that contains a transformation of 3d-vector by the specified <see cref="T:Microsoft.Xna.Framework.Matrix" />.
		/// </summary>
		/// <param name="position">Source <see cref="T:Microsoft.Xna.Framework.Vector3" />.</param>
		/// <param name="matrix">The transformation <see cref="T:Microsoft.Xna.Framework.Matrix" />.</param>
		/// <returns>Transformed <see cref="T:Microsoft.Xna.Framework.Vector3" />.</returns>
		public static Vector3 Transform(this Vector3 position, MCMatrix matrix)
		{
			return Vector3.Transform(position, matrix);
			Transform(ref position, ref matrix, out position);
			return position;
		}

		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Xna.Framework.Vector3" /> that contains a transformation of 3d-vector by the specified <see cref="T:Microsoft.Xna.Framework.Matrix" />.
		/// </summary>
		/// <param name="position">Source <see cref="T:Microsoft.Xna.Framework.Vector3" />.</param>
		/// <param name="matrix">The transformation <see cref="T:Microsoft.Xna.Framework.Matrix" />.</param>
		/// <param name="result">Transformed <see cref="T:Microsoft.Xna.Framework.Vector3" /> as an output parameter.</param>
		public static void Transform(ref Vector3 position, ref MCMatrix matrix, out Vector3 result)
		{
			result.X =  position.X * matrix.M11 +  position.Y *  matrix.M21 +  position.Z *  matrix.M31 + matrix.M41;
			result.Y =  position.X * matrix.M12 +  position.Y *  matrix.M22 +  position.Z *  matrix.M32 + matrix.M42;
			result.Z =  position.X * matrix.M13 +  position.Y *  matrix.M23 +  position.Z *  matrix.M33 + matrix.M43;
		}

		public static RasterizerState Copy(this RasterizerState state)
		{
			return new RasterizerState()
			{
				CullMode = state.CullMode,
				DepthBias = state.DepthBias,
				FillMode = state.FillMode,
				MultiSampleAntiAlias = state.MultiSampleAntiAlias,
				Name = state.Name,
				ScissorTestEnable = state.ScissorTestEnable,
				SlopeScaleDepthBias = state.SlopeScaleDepthBias,
				Tag = state.Tag,
			};
		}

		public static string SplitPascalCase(this string input)
		{
			var result = new StringBuilder();

			foreach (var ch in input)
			{
				if (char.IsUpper(ch) && result.Length > 0)
				{
					result.Append(' ');
				}
				result.Append(ch);
			}
			
			return result.ToString();
		}

		public static byte[] ReadAllBytes(this Stream reader)
		{
			const int bufferSize = 4096;
			using (var ms = new MemoryStream())
			{
				byte[] buffer = new byte[bufferSize];
				int count;
				while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
					ms.Write(buffer, 0, count);
				return ms.ToArray();
			}
		}
		

		public static BoundingBox OffsetBy(this BoundingBox box, Vector3 offset)
		{
			box.Min += offset;
			box.Max += offset;
			return box;
		}

	    public static Vector3 Floor(this Vector3 toFloor)
	    {
	        return new Vector3((float)Math.Floor(toFloor.X), (float)Math.Floor(toFloor.Y), (float)Math.Floor(toFloor.Z));
	    }

		public static BlockFace GetBlockFace(this Vector3 vector)
		{
			BlockFace face = BlockFace.None;

			if (vector == Vector3.Up)
			{
				face = BlockFace.Up;
			}
			else if (vector == Vector3.Down)
			{
				face = BlockFace.Down;
			}
			else if (vector == Vector3.Backward)
			{
				face = BlockFace.South;
			}
			else if (vector == Vector3.Forward)
			{
				face = BlockFace.North;
			}
			else if (vector == Vector3.Left)
			{
				face = BlockFace.West;
			}
			else if (vector == Vector3.Right)
			{
				face = BlockFace.East;
			}

			return face;
		}

		public static void Fill<TType>(this TType[] data, TType value)
		{
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = value;
			}
		}

		public static Guid GuidFromBits(long least, long most)
		{
			byte[] uuidMostSignificantBytes = BitConverter.GetBytes(most);
			byte[] uuidLeastSignificantBytes = BitConverter.GetBytes(least);
			byte[] guidBytes = new byte[16] {
				uuidMostSignificantBytes[4],
				uuidMostSignificantBytes[5],
				uuidMostSignificantBytes[6],
				uuidMostSignificantBytes[7],
				uuidMostSignificantBytes[2],
				uuidMostSignificantBytes[3],
				uuidMostSignificantBytes[0],
				uuidMostSignificantBytes[1],
				uuidLeastSignificantBytes[7],
				uuidLeastSignificantBytes[6],
				uuidLeastSignificantBytes[5],
				uuidLeastSignificantBytes[4],
				uuidLeastSignificantBytes[3],
				uuidLeastSignificantBytes[2],
				uuidLeastSignificantBytes[1],
				uuidLeastSignificantBytes[0]
			};

			return new Guid(guidBytes);
		}

		public static bool IsBitSet(this byte b, int pos)
		{
			return (b & (1 << pos)) != 0;
		}
	}
}
