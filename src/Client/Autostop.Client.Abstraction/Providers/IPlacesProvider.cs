﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.ViewModels;

namespace Autostop.Client.Abstraction.Providers
{
    public interface IPlacesProvider
    {
        Task<ObservableCollection<IAutoCompleteResultModel>> GetAutoCompleteResponse(string input);
    }
}