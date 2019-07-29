using System;
using System.Collections.Generic;
using System.Text;

namespace RocketUI
{
	public class Style
	{
		public string StyleName { get; set; }

		public Type TargetType { get; set; }

		public List<Setter> Setters { get; set; }

	}
}
