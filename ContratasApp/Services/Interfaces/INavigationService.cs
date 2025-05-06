using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace ContratasApp.Services.Interfaces;

public interface INavigationService
{
    Page? CurrentPage { get; }
    
    Task GoToAsync(string route, IDictionary<string, object>? parameters = null);
    
    Task GoBackAsync();
}