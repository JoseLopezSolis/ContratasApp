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
        var popup = new ClientDetailPopup();
        await this.ShowPopupAsync(popup);
    }
}