using System;
using System.Collections.Generic;
using System.Text;

namespace RocketUI
{
	public interface IRocketElement
	{
		PropertyStore Properties { get; }
		IList<IRocketElement> Children { get; }
	}
}
