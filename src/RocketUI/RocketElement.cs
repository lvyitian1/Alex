using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RocketUI
{

	public abstract class RocketElement : IRocketElement
	{
		private PropertyStore _properties;

		/// <summary>
		/// Gets the dictionary of properties for this widget
		/// </summary>
		public PropertyStore Properties => _properties ?? (_properties = new PropertyStore(this));

		public virtual IList<IRocketElement> Children
		{
			get => null;
		}
	}
}