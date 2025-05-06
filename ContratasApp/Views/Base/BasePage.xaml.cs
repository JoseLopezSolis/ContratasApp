using ContratasApp.Extensions;
using ContratasApp.Interfaces;
using Microsoft.Maui.Controls;

namespace ContratasApp.Views.Base;

public partial class BasePage : ContentPage
{
    public BasePage()
    {
        InitializeComponent();
        BindingContext = this.GetViewModel();
    }

    #region Override_methods
    protected override void OnAppearing()
    {
        if (BindingContext is IViewModel viewModel)
            viewModel.OnAppearing();
    }
    
    protected override void OnDisappearing()
    {
        if (BindingContext is IViewModel viewModel)
            viewModel.OnDisappearing();
    }
    #endregion
}