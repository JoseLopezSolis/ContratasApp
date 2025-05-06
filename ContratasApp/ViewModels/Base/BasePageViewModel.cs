using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using ContratasApp.Interfaces;
using ContratasApp.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace ContratasApp.ViewModels.Base;

public class BasePageViewModel : ObservableObject, IViewModel, IQueryAttributable
{
    #region Private Properties
    protected readonly INavigationService NavigationService;
    #endregion
    
    public BasePageViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService; 
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

    #endregion
}