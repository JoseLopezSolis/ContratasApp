using CommunityToolkit.Maui.Views;

namespace ContratasApp.Views.Popups;

public partial class ClientDetailPopup: Popup
{
    public ClientDetailPopup()
    {
        InitializeComponent(); 
    }
    
    void OnOKButtonClicked(object? sender, EventArgs e) => Close();
}