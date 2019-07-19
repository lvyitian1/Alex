using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Catel.Fody;
using Catel.MVVM;
using Orc.Search;
using Orchestra.Models;
using Orchestra.Services;

namespace Alex.GuiDebugger.ViewModels
{
    public class StatusBarViewModel : ViewModelBase
    {
        private readonly IAboutInfoService _aboutInfoService;
        private readonly ISearchService _searchService;

        [Model]
        [Expose(nameof(Orchestra.Models.AboutInfo.ProductName))]
        [Expose(nameof(Orchestra.Models.AboutInfo.Version))]
        public AboutInfo AboutInfo { get; private set; }


        public bool IsUpdatingSearch { get; private set; }

        public bool IsSearching { get; private set; }

        public StatusBarViewModel(IAboutInfoService aboutInfoService, ISearchService searchService)
        {
            _aboutInfoService = aboutInfoService;
            _searchService = searchService;
        }


        protected override async Task InitializeAsync()
        {
            AboutInfo = _aboutInfoService.GetAboutInfo();

            _searchService.Updating += OnSearchServiceUpdating;
            _searchService.Updated  += OnSearchServiceUpdated;

            _searchService.Searching += OnSearchServiceSearching;
            _searchService.Searched  += OnSearchServiceSearched;
        }

        private void OnSearchServiceUpdating(object sender, EventArgs e)
        {
            IsUpdatingSearch = true;
        }

        private void OnSearchServiceUpdated(object sender, EventArgs e)
        {
            IsUpdatingSearch = false;
        }

        private void OnSearchServiceSearching(object sender, SearchEventArgs e)
        {
            IsSearching = true;
        }

        private void OnSearchServiceSearched(object sender, SearchEventArgs e)
        {
            IsSearching = true;
        }
    }
}
