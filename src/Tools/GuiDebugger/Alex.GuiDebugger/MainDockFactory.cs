using System;
using System.Collections.Generic;
using Alex.GuiDebugger.ViewModels;
using Alex.GuiDebugger.ViewModels.Tools;
using Dock.Avalonia.Controls;
using Dock.Model;
using Dock.Model.Controls;
namespace Alex.GuiDebugger
{
	public class MainDockFactory : Factory
	{
		private object _context;

		public MainDockFactory(object context)
		{
			_context = context;
		}

		public override IDock CreateLayout()
		{
			var elementTreeView = new ElementTreeTool()
			{
				Id = "ElementTree",
				Title = "Element Tree"
			};

			var mainLayout = new ProportionalDock()
			{
				Id          = $"MainLayout",
				Title       = "MainLayout",
				Proportion  = double.NaN,
				Orientation = Orientation.Horizontal,
				ActiveDockable = null,
				VisibleDockables = CreateList<IDockable>
					(

					 #region LeftPane

					 new ProportionalDock
					 {
						 Id          = "LeftPane",
						 Title       = "LeftPane",
						 Proportion  = double.NaN,
						 Orientation = Orientation.Vertical,
						 ActiveDockable = null,
						 VisibleDockables = CreateList<IDockable>
							 (
							  new ToolDock
							  {
								  Id          = "LeftPaneTop",
								  Title       = "LeftPaneTop",
								  Proportion  = double.NaN,
								  ActiveDockable = elementTreeView,
								  VisibleDockables = CreateList<IDockable>(elementTreeView)
							  },
							  new SplitterDock()
							  {
								  Id    = "RightPaneTopSplitter",
								  Title = "RightPaneTopSplitter"
							  },
							  new ToolDock
							  {
								  Id          = "LeftPaneBottom",
								  Title       = "LeftPaneBottom",
								  Proportion  = double.NaN,
								  ActiveDockable = null,
								  VisibleDockables = CreateList<IDockable>()
							  }
							 ),
					 },
					 new SplitterDock()
					 {
						 Id    = "LeftSplitter",
						 Title = "LeftSplitter"
					 },

					 #endregion

					 #region DocumentsPane

					 new DocumentDock
					 {
						 Id          = "DocumentsPane",
						 Title       = "DocumentsPane",
						 Proportion  = double.NaN,
						 ActiveDockable = null,
						 VisibleDockables = CreateList<IDockable>()
					 },

					 #endregion

					 #region RightPane

					 new SplitterDock()
					 {
						 Id    = "RightSplitter",
						 Title = "RightSplitter"
					 },
					 new ProportionalDock
					 {
						 Id          = "RightPane",
						 Title       = "RightPane",
						 Proportion  = double.NaN,
						 Orientation = Orientation.Vertical,
						 ActiveDockable = null,
						 VisibleDockables = CreateList<IDockable>
							 (
							  new ToolDock
							  {
								  Id          = "RightPaneTop",
								  Title       = "RightPaneTop",
								  Proportion  = double.NaN,
								  ActiveDockable = null,
								  VisibleDockables = CreateList<IDockable>()
							  },
							  new SplitterDock()
							  {
								  Id    = "RightPaneTopSplitter",
								  Title = "RightPaneTopSplitter"
							  },
							  new ToolDock
							  {
								  Id          = "RightPaneBottom",
								  Title       = "RightPaneBottom",
								  Proportion  = double.NaN,
								  ActiveDockable = null,
								  VisibleDockables = CreateList<IDockable>()
							  }
							 )
					 }

					 #endregion

					)
			};

			var mainView = new MainView
			{
				Id          = "Main",
				Title       = "Main",
				ActiveDockable = mainLayout,
				VisibleDockables = CreateList<IDockable>(mainLayout)
			};

			var root = CreateRootDock();

			root.Id               = "Root";
			root.Title            = "Root";
			root.ActiveDockable      = mainView;
			root.DefaultDockable      = mainView;
			root.VisibleDockables            = CreateList<IDockable>(mainView);
			root.Top              = CreatePinDock();
			root.Top.Alignment    = Alignment.Top;
			root.Bottom           = CreatePinDock();
			root.Bottom.Alignment = Alignment.Bottom;
			root.Left             = CreatePinDock();
			root.Left.Alignment   = Alignment.Left;
			root.Right            = CreatePinDock();
			root.Right.Alignment  = Alignment.Right;

			return root;
		}

        public override void InitLayout(IDockable layout)
        {
            this.ContextLocator = new Dictionary<string, Func<object>>
            {
                [nameof(IRootDock)] = () => _context,
                [nameof(IPinDock)] = () => _context,
                [nameof(IProportionalDock)] = () => _context,
                [nameof(IDocumentDock)] = () => _context,
                [nameof(IToolDock)] = () => _context,
                [nameof(ISplitterDock)] = () => _context,
                [nameof(IDockWindow)] = () => _context,
                [nameof(IDocument)] = () => _context,
                [nameof(ITool)] = () => _context,
                ["ElementTree"] = () => new ElementTreeTool(),
                //["Document2"] = () => new Document2(),
                //["LeftTop1"] = () => new LeftTopTool1(),
                //["LeftTop2"] = () => new LeftTopTool2(),
                //["LeftBottom1"] = () => new LeftBottomTool1(),
                //["LeftBottom2"] = () => new LeftBottomTool2(),
                //["RightTop1"] = () => new RightTopTool1(),
                //["RightTop2"] = () => new RightTopTool2(),
                //["RightBottom1"] = () => new RightBottomTool1(),
                //["RightBottom2"] = () => new RightBottomTool2(),
                ["LeftPane"] = () => _context,
                ["LeftPaneTop"] = () => _context,
                ["LeftPaneTopSplitter"] = () => _context,
                ["LeftPaneBottom"] = () => _context,
                ["RightPane"] = () => _context,
                ["RightPaneTop"] = () => _context,
                ["RightPaneTopSplitter"] = () => _context,
                ["RightPaneBottom"] = () => _context,
                ["DocumentsPane"] = () => _context,
                ["MainLayout"] = () => _context,
                ["LeftSplitter"] = () => _context,
                ["RightSplitter"] = () => _context,
                ["MainLayout"] = () => _context,
                ["Main"] = () => _context,
            };

            this.HostWindowLocator = new Dictionary<string, Func<IHostWindow>>
            {
                [nameof(IDockWindow)] = () => new HostWindow()
            };
            
            this.DockableLocator = new Dictionary<string, Func<IDockable>>
            {
            };

            base.InitLayout(layout);
        }

	}
}