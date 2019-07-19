using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Alex.GuiDebugger.Markup
{
	/// <summary>
	/// Provides a means to specify DataTemplates to be selected from within WPF code
	/// </summary>
	public class DynamicTemplateSelector : DataTemplateSelector
	{
		private DynamicTemplateSelectorExt templateSelector;

		public DynamicTemplateSelector() : base()
		{

		}

		public DynamicTemplateSelector(DynamicTemplateSelectorExt extension) : this()
		{
			templateSelector = extension;
		}

		/// <summary>
		/// Overriden base method to allow the selection of the correct DataTemplate
		/// </summary>
		/// <param name="item">The item for which the template should be retrieved</param>
		/// <param name="container">The object containing the current item</param>
		/// <returns>The <see cref="DataTemplate"/> to use when rendering the <paramref name="item"/></returns>
		public override System.Windows.DataTemplate SelectTemplate(object                          item,
																   System.Windows.DependencyObject container)
		{

			if(item == null) return null;
			if(container as FrameworkElement != null)
			{
				Type itemType = item.GetType();
				DataTemplate dataTemplate = null;

				if((templateSelector.TemplateDictionary).TryGetValue(itemType, out dataTemplate))
					return dataTemplate;
				
				foreach(var key in (templateSelector.TemplateDictionary).Keys)
				{
					if(((Type) key).IsInstanceOfType(item) && (templateSelector.TemplateDictionary).TryGetValue(key, out dataTemplate))
						return dataTemplate;
				}
				
				return dataTemplate;
			}
			else return null;

			////This should ensure that the item we are getting is in fact capable of holding our property
			////before we attempt to retrieve it.
			//if (!(container is UIElement))
			//	return base.SelectTemplate(item, container);

			////First, we gather all the templates associated with the current control through our dependency property
			//TemplateCollection templates = GetTemplates(container as UIElement);
			//if (templates == null || templates.Count == 0)
			//	base.SelectTemplate(item, container);

			
			////Then we go through them checking if any of them match our criteria
			//foreach (var template in templates)
			//	//In this case, we are checking whether the type of the item
			//	//is the same as the type supported by our DataTemplate
			//	if (template.Value == item.GetType())
			//		//And if it is, then we return that DataTemplate
			//		return template.DataTemplate;

			////Then we go through them checking if any of them match our criteria
			//foreach (var template in templates)
			//	//In this case, we are checking whether the type of the item
			//	//is the same as the type supported by our DataTemplate
			//	if (template.Value.IsInstanceOfType(item))
			//		//And if it is, then we return that DataTemplate
			//		return template.DataTemplate;

			////If all else fails, then we go back to using the default DataTemplate
			//return base.SelectTemplate(item, container);
		}
	}

	/// <summary>
	/// Holds a collection of <see cref="Template"/> items
	/// for application as a control's DataTemplate.
	/// </summary>
	public class DynamicTemplateDictionary : Dictionary<object, DataTemplate>
	{
		public DynamicTemplateDictionary() : base()
		{

		}

	}

	[MarkupExtensionReturnType(typeof(DynamicTemplateSelector))]
	public class DynamicTemplateSelectorExt : MarkupExtension
	{

		public DynamicTemplateSelectorExt() : base() { }
		
		public DynamicTemplateSelectorExt(DynamicTemplateDictionary templatePresenter) : this()
		{
			TemplateDictionary = templatePresenter;
		}

		public string Property { get; set; }
		public DynamicTemplateDictionary TemplateDictionary { get; set; }

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if(String.IsNullOrEmpty(Property)) throw new ArgumentException("Corrupt template collection");
			return new DynamicTemplateSelector(this);
		}

	}
}