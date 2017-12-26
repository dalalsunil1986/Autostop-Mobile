﻿using System;
using System.Collections.ObjectModel;
using Autostop.Client.Abstraction.Models;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.ViewModels.Passenger.Places
{
	public interface IBaseSearchPlaceViewModel : ISearchableViewModel
	{	
		ObservableCollection<IAutoCompleteResultModel> SearchResults { get; set; }

		IAutoCompleteResultModel SelectedSearchResult { get; set; }

		IObservable<Address> SelectedAddress { get; }
	}
}