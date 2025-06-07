using CommunityToolkit.Maui.Views;
using ContratasApp.ViewModels;
using ContratasApp.Views.Popups;

namespace ContratasApp.Views;

public partial class ClientPage
{
    public ClientPage()
    {
        InitializeComponent(); 
    }
    
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        // Fire the RefreshContractsCommand you already have in your VM
        (BindingContext as ClientPageViewModel)
            ?.RefreshContractsCommand
            .Execute(null);
        
        ProgressBar.IsVisible = true;
        ProgressBar.Progress = 0;

        await ProgressBar.ProgressTo(0.7, 400, Easing.CubicInOut);

        (BindingContext as ClientPageViewModel)
            ?.RefreshContractsCommand
            .Execute(null);

        await ProgressBar.ProgressTo(1.0, 300, Easing.CubicOut);

        ProgressBar.IsVisible = false;
    }
    
    private async void OnSeeDetailClient(object sender, EventArgs e)
    {
        if (BindingContext is ClientPageViewModel vm && vm.Client is not null)
        {
            var client = vm.Client;

            var popup = new ClientDetailPopup(
                $"{client.FullName}",
                client.Phone,
                client.PaymentMethod,
                "itsjoselops@gmail.com", 
                client.ContractsCount
            );

            await this.ShowPopupAsync(popup);
        }
    }
    private async void OnProgressButtonClicked(object sender, EventArgs e)
    {
        await ProgressBar.ProgressTo(0.75, 500, Easing.CubicInOut);
    }
}