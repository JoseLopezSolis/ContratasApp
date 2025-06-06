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
    
    protected override void OnAppearing()
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
                "itsjoselops@gmail.com", 
                client.ContractsCount
            );

            await this.ShowPopupAsync(popup);
        }
    }
    
    private async void OnCallButtonClicked(object sender, EventArgs e)
    {
        string phoneNumber = "3121219656"; // Replace with your desired number

        try
        {
            // Create tel URI
            var phoneUri = new Uri($"tel:{phoneNumber}");

            // Use Launcher to initiate the call
            await Launcher.Default.OpenAsync(phoneUri);
        }
        catch (Exception ex)
        {
            // Handle error, e.g., unsupported device
            await DisplayAlert("Error", $"Unable to initiate call: {ex.Message}", "OK");
        }
    }
}