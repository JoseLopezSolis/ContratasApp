using System.Collections.Generic;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContratasApp.Interfaces;
using ContratasApp.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace ContratasApp.ViewModels.Base;

public class BasePageViewModel : ObservableObject, IViewModel, IQueryAttributable
{
    #region Private Properties
    protected readonly INavigationService NavigationService;
    public bool needRefreshPage;
    private bool _firstStart = true;
    #endregion
    
    public BasePageViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        if (_firstStart) needRefreshPage = true;
        else
        {
            _firstStart = false;
        }
    }
    
    #region Virtual Methods
    public virtual void OnAppearing()
    {
    }

    public virtual void OnDisappearing()
    {
    }

    public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    protected virtual void InitProperties()
    {
    }
    
    public virtual Task InitializeAsync(object navigationData)
        => Task.CompletedTask;

    #endregion
}