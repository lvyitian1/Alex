using System;
using System.Collections.Generic;
using System.Text;

namespace RocketUI
{
	public interface IVisualElement
	{
		PropertyStore Properties { get; }
		IList<IVisualElement> Children { get; }
	}
}
