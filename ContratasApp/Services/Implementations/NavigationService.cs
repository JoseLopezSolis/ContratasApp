using System.Collections.Generic;
using System.Threading.Tasks;
using ContratasApp.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace ContratasApp.Services.Implementations;

public class NavigationService : INavigationService
{
    public Page? CurrentPage => GetCurrentPage();

    public async Task GoToAsync(string route, IDictionary<string, object>? parameters = null)
    {
        if (string.IsNullOrEmpty(route))
            return;

        if (parameters == null)
            await Shell.Current.GoToAsync(route);
        else
            await Shell.Current.GoToAsync(new ShellNavigationState(route), parameters);
    }

    public async Task GoBackAsync() => await Shell.Current.GoToAsync("../");

    private Page? GetCurrentPage() => Shell.Current.CurrentPage;
}