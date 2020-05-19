using System;
using System.Collections.Generic;
using Alex.GuiDebugger.Models;
using Alex.GuiDebugger.ViewModels;
using Alex.GuiDebugger.ViewModels.Tools;
using Dock.Avalonia.Controls;
using Dock.Model;
using Dock.Model.Controls;

namespace Alex.GuiDebugger.Factories
{
	public class DefaultDockFactory : Factory
	{
		private object _context;

		public DefaultDockFactory(object context)
		{
			_context = context;
		}

		public override IDock CreateLayout()
		{
			//var elementTreeDocument = new ElementTreeDocument
			//{
			//	Id    = "ElementTreeDocument",
			//	Title = "ElementTreeDocument"
			//};

			var elementTreeTool = new ElementTreeTool
			{
				Id    = "ElementTreeTool",
				Title = "ElementTreeTool"
			};

			var leftPaneTop = new ToolDock()
			{
				Id          = "LeftPaneTop",
				Title       = "LeftPaneTop",
				Proportion  = double.NaN,
				ActiveDockable = elementTreeTool,
				VisibleDockables       = CreateList<IDockable>(elementTreeTool)
			};
			var leftPane = new ProportionalDock()
			{
				Id          = "LeftPane",
				Title       = "LeftPane",
				Proportion  = double.NaN,
				Orientation = Orientation.Vertical,
				ActiveDockable = null,
				VisibleDockables       = CreateList<IDockable>(leftPaneTop)
			};
			var leftSplitter = new SplitterDock()
			{
				Id    = "LeftSplitter",
				Title = "LeftSplitter"
			};
			var documentsPane = new DocumentDock()
			{
				Id         = "DocumentsPane",
				Title      = "DocumentsPane",
				Proportion = double.NaN,
				ActiveDockable = null,
				VisibleDockables = CreateList<IDockable>()
				//ActiveDockable = elementTreeDocument,
				//VisibleDockables = CreateList<IDockable>(elementTreeDocument)
			};
			var mainLayout = new ProportionalDock
			{
				Id          = "MainLayout",
				Title       = "MainLayout",
				Proportion  = double.NaN,
				Orientation = Orientation.Horizontal,
				ActiveDockable = null,
				VisibleDockables       = CreateList<IDockable>(leftPane, leftSplitter, documentsPane)
			};

			var mainView = new MainView
			{
				Id          = "Main",
				Title       = "Main",
				ActiveDockable = mainLayout,
				VisibleDockables       = CreateList<IDockable>(mainLayout)
			};


			var root = CreateRootDock();

			root.Id             = "Root";
			root.Title          = "Root";
			root.ActiveDockable    = mainView;
			root.DefaultDockable    = mainView;
			root.VisibleDockables          = CreateList<IDockable>(mainView);
			root.Left           = CreatePinDock();
			root.Left.Alignment = Alignment.Left;

			AddAllVisibleDockables(root, mainView, mainLayout, documentsPane, leftSplitter, leftPane, leftPaneTop, elementTreeTool);

			return root;
		}

		private void AddAllVisibleDockables(params IDockable[] visibleDockables)
		{
			if(DockableLocator == null) DockableLocator = new Dictionary<string, Func<IDockable>>();

			foreach (var view in visibleDockables)
			{
				DockableLocator[view.Id] = () => view;
			}
		}

		public override void InitLayout(IDockable layout)
		{
			this.ContextLocator = new Dictionary<string, Func<object>>
			{
				[nameof(IRootDock)]     = () => _context,
				[nameof(IPinDock)]      = () => _context,
				[nameof(IDockable)]   = () => _context,
				[nameof(IDocumentDock)] = () => _context,
				[nameof(IToolDock)]     = () => _context,
				[nameof(ISplitterDock)] = () => _context,
				[nameof(IDockWindow)]   = () => _context,
				[nameof(IDocument)]  = () => _context,
				[nameof(ITool)]      = () => _context,
				["ElementTreeDocument"] = () => new ElementTreeDocumentModel(),
				["ElementTreeTool"]     = () => new ElementTreeToolModel(),
				["LeftPane"]            = () => _context,
				["LeftPaneTop"]         = () => _context,
				["ElementTreeTool"]     = () => _context,
				["DocumentsPane"]       = () => _context,
				["MainLayout"]          = () => _context,
				["LeftSplitter"]        = () => _context,
				["MainLayout"]          = () => _context,
				["Main"]                = () => layout,
				// ["Editor"] = () => new LayoutEditor()
				// {
				// 	Layout = layout
				// }
			};

			this.HostWindowLocator = new Dictionary<string, Func<IHostWindow>>
			{
				[nameof(IDockWindow)] = () => new HostWindow()
			};
			
			this.DockableLocator = new Dictionary<string, Func<IDockable>>()
			{
				
			};

			base.InitLayout(layout);
		}
	}
}