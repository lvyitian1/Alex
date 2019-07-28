using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alex.API.Gui.Elements.Layout;
using Alex.API.Gui.Layout;
using Microsoft.Xna.Framework;
using RocketUI;
using GuiCursorEventArgs = Alex.API.Gui.Events.GuiCursorEventArgs;

namespace Alex.API.Gui.Elements.Controls
{
	public class GuiComboBox<TValue> : GuiItemsControl<TValue> where TValue : IEquatable<TValue>
	{
		public bool IsSearchable { get; set; } = false;

		public bool IsOpen { get; set; } = false;


		private GuiTextInput _textInput;
		private GuiButton    _openPopupButton;
		private GuiPopup     _popup;
		private GuiScrollableStackContainer _popupItemList;

		private List<GuiButton> _items = new List<GuiButton>();

		public GuiComboBox()
		{
			DisplayFormat = "{0}";
			AddChild(_textInput       = new GuiTextInput()
			{
				Anchor = Alignment.TopFill
			});
			//AddChild(_openPopupButton = new GuiButton());
			AddChild(_popup           = new GuiPopup()
			{
				Anchor = Alignment.BottomLeft,
				Background = Color.Black,
				MaxHeight = 128,
				MaxWidth = 256,
				Margin    = new Thickness(0, _textInput.Height + 1, 0, 0),
				IsVisible = false,
				ClipToBounds = true
			});

			_popup.AddChild(_popupItemList = new GuiScrollableStackContainer()
			{
				Anchor = Alignment.Fill,
				AutoSizeMode = AutoSizeMode.GrowOnly,
				Orientation = Orientation.Vertical,
				ChildAnchor = Alignment.TopFill,
				Background = Color.DarkBlue
				
			});
			
			CursorPressed += TogglePopup;
			_textInput.CursorPressed += TogglePopup;
			//_openPopupButton.CursorPressed += TogglePopup;
		}

		private void TogglePopup(object sender, GuiCursorEventArgs e)
		{
			IsOpen = !IsOpen;
			_popup.IsVisible = IsOpen;
		}

		protected override void OnFocusActivate()
		{
			base.OnFocusActivate();
			IsOpen = true;
			_popup.IsVisible = true;
		}

		protected override void OnFocusDeactivate()
		{
			base.OnFocusDeactivate();
			IsOpen = false;
			_popup.IsVisible = false;
		}

		protected override void OnItemsChanged()
		{
			foreach (var item in _items)
			{
				_popupItemList.RemoveChild(item);
			}
			_items.Clear();

			if (Items == null) return;

			var i = 0;
			foreach (var item in Items.ToArray())
			{
				var itemElement = new GuiButton(string.Format(DisplayFormat, item), () => OnItemPressed(item))
				{
					Height = 15,
					AutoSizeMode = AutoSizeMode.GrowOnly,
					Anchor = Alignment.TopFill,
					Background = (i % 2 == 0) ? Color.DarkRed : Color.DarkGray
				};
				
				_popupItemList.AddChild(itemElement);
				_items.Add(itemElement);
				i++;
			}

			InvalidateLayout();
		}

		private void OnItemPressed(TValue value)
		{
			SelectedItem = value;
			IsOpen = false;
			_popup.IsVisible = false;
		}

		protected override void OnSelectedItemChanged()
		{
			base.OnSelectedItemChanged();
			if (SelectedItem == null)
			{
				_textInput.Value = "Choose plz..";

			}
			else { 
				_textInput.Value = string.Format(DisplayFormat, SelectedItem);
			}
		}

		protected override void OnUpdateLayout()
		{
			_popup.Margin = new Thickness(_textInput.Margin.Left, _textInput.Height + _textInput.Margin.Vertical, _textInput.Margin.Right, 0);
		}

		protected override void PositionChildCore(GuiElement child, ref LayoutBoundingRectangle bounds, Thickness offset, Alignment alignment)
		{
			base.PositionChildCore(child, ref bounds, offset, alignment);
		}

		protected override Size MeasureChildrenCore(Size availableSize, IReadOnlyCollection<GuiElement> children)
		{
			base.MeasureChildrenCore(availableSize, new List<GuiElement>() {_popup});
			return base.MeasureChildrenCore(availableSize, children.Where(c => c != _popup).ToList());
		}
	}

	public class GuiPopup : GuiContainer
	{

		public GuiPopup()
		{

		}

	}
}