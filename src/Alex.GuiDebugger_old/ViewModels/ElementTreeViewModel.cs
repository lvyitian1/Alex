using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Alex.GuiDebugger.Common;
using Alex.GuiDebugger.Models;
using Alex.GuiDebugger.Services;
using Catel;
using Catel.Collections;
using Catel.Fody;
using Catel.Services;
using Catel.Threading;
using Orc.Search;

namespace Alex.GuiDebugger.ViewModels
{
	using Catel.MVVM;

	/// <summary>
	/// UserControl view model.
	/// </summary>
	public class ElementTreeViewModel : ViewModelBase
	{
		private readonly IGuiDebugDataService _guiDebugDataService;
		private readonly ISearchService       _searchService;
		private readonly IUIVisualizerService _uiVisualizerService;

		[Model]
		public GuiDebuggerData GuiDebuggerData { get; private set; }


		public           object    SelectedObject { get; set; }

		public string Filter { get; set; }

		public FastObservableCollection<ISearchable> AllElements { get; set; }

		public FastObservableCollection<GuiDebuggerElementInfo> FilteredElements { get; set; }

		public int FilteredObjectCount { get; private set; }

		public TimeSpan LastSearchDuration { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ElementTreeViewModel"/> class.
		/// </summary>
		public ElementTreeViewModel(IGuiDebugDataService guiDebugDataService, ISearchService searchService,
									IUIVisualizerService uiVisualizerService)
		{
			_guiDebugDataService = guiDebugDataService;
			_searchService       = searchService;
			_uiVisualizerService = uiVisualizerService;
			Title                = "Element Tree";

			AllElements      = new FastObservableCollection<ISearchable>();
			FilteredElements = new FastObservableCollection<GuiDebuggerElementInfo>();

			GuiDebuggerData = _guiDebugDataService.GuiDebuggerData;
			GuiDebuggerData.SubscribeToPropertyChanged(nameof(Models.GuiDebuggerData.Elements), Handler);
			GuiDebuggerData.Elements.CollectionChanged += ElementsOnCollectionChanged;
		}

		private void Handler(object sender, PropertyChangedEventArgs args)
		{
			AllElements.ReplaceRange(GuiDebuggerData.Elements.Select(e => new ReflectionSearchable(e)).ToList());
		}

		private void ElementsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			AllElements.ReplaceRange(GuiDebuggerData.Elements.Select(e => new ReflectionSearchable(e)).ToList());
		}

		protected ElementTreeViewModel()
		{
			Title = "Element Tree";
		}

		#region Methods

		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();

			_searchService.Searched  += OnSearchServiceSearched;

			using (AllElements.SuspendChangeNotifications())
			{
				var generatedSearchables = GuiDebuggerData.Elements.Select(e => new ReflectionSearchable(e)).ToList();

				((ICollection<ISearchable>) AllElements).ReplaceRange(generatedSearchables);
				AllElements.CollectionChanged += OnAllElementsOnCollectionChanged;

				await TaskHelper.Run(() => _searchService.AddObjects(generatedSearchables), true);
			}
		}

		private void OnAllElementsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (args.OldItems != null && args.OldItems.Count > 0)
			{
				var oldItems = args.OldItems;
				_searchService.RemoveObjects(oldItems.OfType<ISearchable>());
			}

			if (args.NewItems != null && args.NewItems.Count > 0)
			{
				var newItems = args.NewItems;
				_searchService.RemoveObjects(newItems.OfType<ISearchable>());
			}

			_searchService.Search(Filter);
		}

		protected override async Task CloseAsync()
		{
			_searchService.Searched  -= OnSearchServiceSearched;

			await base.CloseAsync();
		}

		private void OnSearchServiceSearched(object sender, SearchEventArgs e)
		{
			var filteredObjects = FilteredElements;
			
			using (filteredObjects.SuspendChangeNotifications())
			{
				if (string.IsNullOrEmpty(e.Filter))
				{
					((ICollection<GuiDebuggerElementInfo>) filteredObjects).ReplaceRange(AllElements.Select(r => r.Instance as GuiDebuggerElementInfo));
				}
				else
				{
					((ICollection<GuiDebuggerElementInfo>) filteredObjects).ReplaceRange(e.Results.Select(r => r.Instance as GuiDebuggerElementInfo));
				}
				
				FilteredObjectCount = filteredObjects.Count;
			}
		}

		#endregion
	}
}