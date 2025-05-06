using System;
using Microsoft.Maui.Controls;
using ServiceProvider = ContratasApp.Utils.ServiceProvider;

namespace ContratasApp.Extensions;

public static class ContentPageExtensions
{
    public static object? GetViewModel(this ContentPage contentPage)
    {
        var pageType = contentPage.GetType();
        var viewModelName = pageType.FullName?.Replace("Views", "ViewModels") + "ViewModel";
        var viewModelType = Type.GetType(viewModelName);

        if (viewModelType != null)
        {
            return ServiceProvider.GetService(viewModelType) ?? Activator.CreateInstance(viewModelType);
        }

        return null;
    }
}