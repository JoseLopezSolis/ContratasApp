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
                client.Email, 
                client.ContractsCount
            );

            await this.ShowPopupAsync(popup);
        }
    }
}