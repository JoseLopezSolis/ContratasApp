using CommunityToolkit.Maui.Views;

namespace ContratasApp.Views.Popups;

public partial class ClientDetailPopup: Popup
{
    public ClientDetailPopup(string  nameClient = "Default", string phoneRegistered = "Default", string methodOfPaymentSelected = "Default", string emailEntered = "Default", int loanClientCount = 0)
    {
        InitializeComponent(); 
        welcomeMessage.Text = $"Detalles sobre \n {nameClient}";
        phone.Text = $"{phoneRegistered}"; 
        methodOfPayment.Text = $"{methodOfPaymentSelected}";
        email.Text = $"{emailEntered}";
        loanCount.Text = $"{loanClientCount}";
    }
    
    void OnOKButtonClicked(object? sender, EventArgs e) => Close();
}