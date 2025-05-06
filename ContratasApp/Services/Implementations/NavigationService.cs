using System.Collections.Generic;
using System.Threading.Tasks;
using ContratasApp.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace ContratasApp.Services.Implementations;

public class NavigationService : INavigationService
{
    #region Public Properties

    public Page? CurrentPage => GetCurrentPage();

    #endregion
    
    #region Public Methods

    public async Task GoToAsync(string route, IDictionary<string, object>? parameters = null)
    {
        if (string.IsNullOrEmpty(route))
        {
            return;
        }
        
        await Shell.Current.GoToAsync(new ShellNavigationState(route), parameters);
    }

    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("../");
    }

    #endregion

    #region Private Methods

    private Page? GetCurrentPage()
    {
        return Shell.Current.CurrentPage;
    }

    #endregion
}