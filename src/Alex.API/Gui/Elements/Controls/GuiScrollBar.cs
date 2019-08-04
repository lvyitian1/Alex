using System;
using System.Diagnostics;
using Alex.API.Gui.Graphics;
using Microsoft.Xna.Framework;
using NLog;
using RocketUI;
using GuiCursorEventArgs = Alex.API.Gui.Events.GuiCursorEventArgs;
using GuiCursorMoveEventArgs = Alex.API.Gui.Events.GuiCursorMoveEventArgs;

namespace Alex.API.Gui.Elements.Controls
{
	public class ScrollOffsetValueChangedEventArgs : EventArgs
	{
		public int ScrollOffsetValue { get; }

		internal ScrollOffsetValueChangedEventArgs(int scrollOffsetValue)
		{
			ScrollOffsetValue = scrollOffsetValue;
		}
	}

	public class GuiScrollBar : GuiControl
	{
		private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		public event EventHandler<ScrollOffsetValueChangedEventArgs> ScrollOffsetValueChanged;

		public Orientation Orientation
		{
			get => _orientation;
			set
			{
				_orientation = value;
				OnOrientationChanged();
			}
		}

		private GuiButton ScrollDownButton;
		private GuiButton ScrollUpButton;
		private GuiButton Track;

		public GuiTexture2D ThumbBackground;
		public GuiTexture2D ThumbHighlightBackground;

		private Orientation _orientation     = Orientation.Vertical;
		private int         _maxScrollOffset = 0;
		private int         _scrollOffsetValue;

		public bool CanScrollUp { get; private set; }
		public bool CanScrollDown { get; private set; }

		public int ScrollButtonStep { get; set; } = 25;

		public int ScrollOffsetValue
		{
			get => _scrollOffsetValue;
			set
			{
				if (value == _scrollOffsetValue) return;

				var prevValue = _scrollOffsetValue;
				_scrollOffsetValue = Math.Clamp(value, 0, _maxScrollOffset);
				ScrollOffsetValueChanged?.Invoke(this, new ScrollOffsetValueChangedEventArgs(_scrollOffsetValue));

				OnScrollOffsetValueChanged(prevValue, _scrollOffsetValue);

				Log.Info($"ScrollOffsetValue.Change {{ScrollOffsetValue=({prevValue} => {_scrollOffsetValue}), ScrollButtonStep={ScrollButtonStep}, MaxScrollOffset={MaxScrollOffset}}}");
				Debug.WriteLine($"ScrollOffsetValue.Change {{ScrollOffsetValue=({prevValue} => {_scrollOffsetValue}), ScrollButtonStep={ScrollButtonStep}, MaxScrollOffset={MaxScrollOffset}}}");
			}
		}

		public int MaxScrollOffset
		{
			get => _maxScrollOffset;
			set
			{
				if (value == _maxScrollOffset) return;

				_maxScrollOffset  = value;
				ScrollOffsetValue = ScrollOffsetValue;
			}
		}

		public GuiScrollBar()
		{
			Background               = Color.Black;
			ThumbBackground          = "ButtonDefault";
			ThumbHighlightBackground = "ButtonHover";

			Background.RepeatMode               = TextureRepeatMode.NoScaleCenterSlice;
			ThumbBackground.RepeatMode          = TextureRepeatMode.NoScaleCenterSlice;
			ThumbHighlightBackground.RepeatMode = TextureRepeatMode.NoScaleCenterSlice;

			MinWidth  = 10;
			MinHeight = 10;

			Padding = Thickness.Zero;
			Margin  = Thickness.Zero;

			AddChild(ScrollDownButton = new GuiButton(() => ScrollOffsetValue += ScrollButtonStep)
			{
				Width  = 10,
				Height = 10,
				Margin = new Thickness(0, 0, 0, 0),

				Background            = "ScrollBarDownButtonDefault",
				HighlightedBackground = "ScrollBarDownButtonHover",
				FocusedBackground     = "ScrollBarDownButtonFocused",
				DisabledBackground    = "ScrollBarDownButtonDisabled",

				InvokeOnPress = false
			});

			AddChild(ScrollUpButton = new GuiButton(() => ScrollOffsetValue -= ScrollButtonStep)
			{
				Width  = 10,
				Height = 10,
				Margin = new Thickness(0, 0, 0, 0),

				Background            = "ScrollBarUpButtonDefault",
				HighlightedBackground = "ScrollBarUpButtonHover",
				FocusedBackground     = "ScrollBarUpButtonFocused",
				DisabledBackground    = "ScrollBarUpButtonDisabled",


				InvokeOnPress = false
			});

			AddChild(Track = new GuiButton()
			{
				MinWidth  = 10,
				MinHeight = 10,
				Margin    = new Thickness(0, 0, 0, 0),

				Background            = "ScrollBarTrackDefault",
				HighlightedBackground = "ScrollBarTrackHover",
				FocusedBackground     = "ScrollBarTrackFocused",
				DisabledBackground    = "ScrollBarTrackDisabled"
			});

			Track.Background.RepeatMode            = TextureRepeatMode.NoScaleCenterSlice;
			Track.HighlightedBackground.RepeatMode = TextureRepeatMode.NoScaleCenterSlice;
			Track.FocusedBackground.RepeatMode     = TextureRepeatMode.NoScaleCenterSlice;
			Track.DisabledBackground.RepeatMode    = TextureRepeatMode.NoScaleCenterSlice;

			Track.CursorMove    += TrackOnCursorMove;
			Track.CursorPressed += TrackOnCursorPressed;

			Orientation = Orientation.Vertical;
		}

		private void TrackOnCursorPressed(object sender, GuiCursorEventArgs e)
		{
			SetValueFromCursor(e.CursorPosition);
		}

		private void TrackOnCursorMove(object sender, GuiCursorMoveEventArgs e)
		{
			if (e.IsCursorDown)
				SetValueFromCursor(e.CursorPosition);
		}

		private void SetValueFromCursor(Point relativePosition)
		{
			var percentageClicked = Orientation == Orientation.Vertical
										? (relativePosition.Y / (float) RenderBounds.Height)
										: (relativePosition.X / (float) RenderBounds.Width);

			ScrollOffsetValue = (int) Math.Round(MaxScrollOffset * percentageClicked);
		}

		protected override void OnInit(IGuiRenderer renderer)
		{
			base.OnInit(renderer);

			ThumbBackground.TryResolveTexture(renderer);
			ThumbHighlightBackground.TryResolveTexture(renderer);
		}

		private void OnScrollOffsetValueChanged(int prevValue, int scrollOffsetValue)
		{
			UpdateTrack();
		}

		private void OnOrientationChanged()
		{
			if (Orientation == Orientation.Vertical)
			{
				ScrollDownButton.Anchor = Alignment.BottomFill;
				ScrollDownButton.Margin = new Thickness(0, 0, 0, 0);

				ScrollUpButton.Anchor = Alignment.TopFill;
				ScrollUpButton.Margin = new Thickness(0, 0, 0, 10);
				
				Track.Anchor = Alignment.TopFill;
			}
			else
			{
				ScrollDownButton.Anchor = Alignment.FillRight;
				ScrollDownButton.Margin = new Thickness(0, 0, 0, 0);

				ScrollUpButton.Anchor = Alignment.FillLeft;
				ScrollUpButton.Margin = new Thickness(0, 0, 10, 0);

				Track.Anchor = Alignment.FillLeft;

				ScrollUpButton.Rotation   = 270f;
				ScrollDownButton.Rotation = 270f;
			}

			UpdateTrack();
		}

		private void UpdateTrack()
		{
			var containerSize = Orientation == Orientation.Vertical ? RenderBounds.Height : RenderBounds.Width;
			var trackSize = Orientation == Orientation.Vertical
								? (containerSize - ScrollUpButton.Height - ScrollDownButton.Height)
								: (containerSize - ScrollUpButton.Width - ScrollDownButton.Width);
			var maxOffset = MaxScrollOffset;

			var contentSize = containerSize + maxOffset;

			var visibleSizeAsPercentage = (containerSize / (double) contentSize);
			if (visibleSizeAsPercentage >= 1.0d)
			{
				if (Orientation == Orientation.Vertical)
					Track.Height = trackSize;
				else
					Track.Width = trackSize;
				
				Track.Margin = Thickness.Zero;
				
				Track.Enabled            = false;

				ScrollUpButton.Enabled   = CanScrollUp = false;
				ScrollDownButton.Enabled = CanScrollDown = false;
			}
			else
			{
				var size = (int) Math.Floor(visibleSizeAsPercentage * trackSize);


				if (Orientation == Orientation.Vertical)
					Track.Height = size;
				else
					Track.Width = size;

				var trackMaxOffset = trackSize - size;
				var trackOffset = (int)((double) trackMaxOffset *
								  (double) ((double) ScrollOffsetValue / (double) MaxScrollOffset));

				if (Orientation == Orientation.Vertical)
				{
					Track.Margin = new Thickness(0, trackOffset, 0, 0);
				}
				else
				{
					Track.Margin = new Thickness(trackOffset, 0, 0, 0);
				}

				Track.Enabled = true;

				ScrollUpButton.Enabled   = CanScrollUp = (ScrollOffsetValue > 0);
				ScrollDownButton.Enabled = CanScrollDown = (ScrollOffsetValue < MaxScrollOffset);
			}
		}
	}
}