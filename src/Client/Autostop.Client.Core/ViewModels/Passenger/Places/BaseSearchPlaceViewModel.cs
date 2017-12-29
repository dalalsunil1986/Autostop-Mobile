﻿using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.Models;
using Autostop.Common.Shared.Models;
using Conditions;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger.Places
{
	public abstract class BaseSearchPlaceViewModel : BaseViewModel, IBaseSearchPlaceViewModel
	{
		private readonly INavigationService _navigationService;
		private ObservableCollection<IAutoCompleteResultModel> _searchResults;
		private IAutoCompleteResultModel _selectedSearchResult;
		private bool _isLoading;
		private string _searchText;

		protected BaseSearchPlaceViewModel(
			ISchedulerProvider schedulerProvider,
			IPlacesProvider placesProvider,
			IGeocodingProvider geocodingProvider,
			INavigationService navigationService)
        {
            placesProvider.Requires(nameof(placesProvider)).IsNotNull();
            navigationService.Requires(nameof(navigationService)).IsNotNull();

            _navigationService = navigationService;

            this.Changed(() => SearchText)
                .Throttle(TimeSpan.FromMilliseconds(300), schedulerProvider.DefaultScheduler)
                .ObserveOn(schedulerProvider.SynchronizationContextScheduler)
                .Subscribe(async searchText =>
                {
                    if (string.IsNullOrEmpty(searchText))
                    {
                        SearchResults = GetEmptyAutocompleteResult();
                    }
                    else
                    {
                        IsSearching = true;
                        SearchResults = await placesProvider.GetAutoCompleteResponse(searchText);
                        IsSearching = false;
                    }
                });


            var selectedEmptyAddress = SelectedEmptyAutocompleteResultModelObservable()
                .Where(r => r.Address != null)
                .Select(r => r.Address);

            SelectedAddress = SelectedAutoCompleteResultModelObservable()
                .Select(x => Observable.FromAsync(() => geocodingProvider.ReverseGeocodingFromPlaceId(x.PlaceId)))
                .Concat()
                .Merge(selectedEmptyAddress);

            GoBack = new RelayCommand(GoBackAction);
        }

        protected IObservable<IAutoCompleteResultModel> SelectedAutoCompleteResultModelObservable()
        {
            return this.Changed(() => SelectedSearchResult)
                            .Where(r => r is AutoCompleteResultModel);
        }

        protected IObservable<EmptyAutocompleteResultModel> SelectedEmptyAutocompleteResultModelObservable()
		{
			return this.Changed(() => SelectedSearchResult)
				.Where(r => r is EmptyAutocompleteResultModel)
				.Cast<EmptyAutocompleteResultModel>();
		}

		private void GoBackAction()
		{
			_navigationService.GoBack();
		}

		public bool IsSearching
		{
			get => _isLoading;
			set => RaiseAndSetIfChanged(ref _isLoading, value);
		}

		public virtual string SearchText
		{
			get => _searchText;
			set => RaiseAndSetIfChanged(ref _searchText, value);
		}

		public ObservableCollection<IAutoCompleteResultModel> SearchResults
		{
			get => _searchResults;
			set => RaiseAndSetIfChanged(ref _searchResults, value);
		}

		public IAutoCompleteResultModel SelectedSearchResult
		{
			get => _selectedSearchResult;
			set => RaiseAndSetIfChanged(ref _selectedSearchResult, value);
		}

		public IObservable<Address> SelectedAddress { get; }
		
		[UsedImplicitly]
		public virtual string PlaceholderText { get; }

		public virtual ICommand GoBack { get; }

		protected abstract ObservableCollection<IAutoCompleteResultModel> GetEmptyAutocompleteResult();
	}
}