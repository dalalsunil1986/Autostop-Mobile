﻿using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.Models;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger.Places
{
	[UsedImplicitly]
	public sealed class SearchWorkAddressViewModel : BaseSearchPlaceViewModel
	{
		private readonly IEmptyAutocompleteResultProvider _autocompleteResultProvider;

		public SearchWorkAddressViewModel(
			ISchedulerProvider schedulerProvider,
			INavigationService navigationService,
			IPlacesProvider placesProvider,
			IEmptyAutocompleteResultProvider autocompleteResultProvider,
			ISettingsProvider settingsProvider,
			IGeocodingProvider geocodingProvider) : base(schedulerProvider, placesProvider, geocodingProvider, navigationService)
		{
			_autocompleteResultProvider = autocompleteResultProvider;

			this.SelectedAutoCompleteResultModelObservable()
				.Subscribe(async result =>
				{
					var address = await geocodingProvider.ReverseGeocodingFromPlaceId(result.PlaceId);
					settingsProvider.WorkAddress = address;
					GoBackCallback?.Invoke();
				});

		    this.Changed(() => SelectedSearchResult)
		        .Where(r => r is SetLocationOnMapResultModel)
		        .Subscribe(result =>
		        {
		            navigationService.NavigateTo<ChooseWorkAddressOnMapViewModel>();
		        });
        }

		public Action GoBackCallback { get; set; }

		protected override ObservableCollection<IAutoCompleteResultModel> GetEmptyAutocompleteResult()
		{
			return new ObservableCollection<IAutoCompleteResultModel>
			{
				_autocompleteResultProvider.GetSetLocationOnMapResultModel()
			};
		}

		public override string PlaceholderText => "Set work address";
	}
}
