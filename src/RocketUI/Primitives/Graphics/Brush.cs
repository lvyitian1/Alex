using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace RocketUI
{
	public abstract class Brush : IDisposable
	{
		/// <summary>
		/// Releases all resource used by the <see cref="Eto.Drawing.Brush"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose()"/> when you are finished using the <see cref="Eto.Drawing.Brush"/>. The
		/// <see cref="Dispose()"/> method leaves the <see cref="Eto.Drawing.Brush"/> in an unusable state. After calling
		/// <see cref="Dispose()"/>, you must release all references to the <see cref="Eto.Drawing.Brush"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Eto.Drawing.Brush"/> was occupying.</remarks>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes the brush
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> dispose was called explicitly, otherwise specify false if calling from a finalizer</param>
		protected virtual void Dispose(bool disposing) { }
	}
}
