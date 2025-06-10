using CommunityToolkit.Maui.Views;

namespace ContratasApp.Views.Popups;

public partial class ClientDetailPopup: Popup
{
    public ClientDetailPopup(string  nameClient = "Default", string phoneRegistered = "Default", string methodOfPaymentSelected = "Default", string emailEntered = "Default", int loanClientCount = 0)
    {
        InitializeComponent(); 
        welcomeMessage.FormattedText = new FormattedString
        {
            Spans =
            {
                new Span { Text = "Detalles sobre \n" },
                new Span { Text = nameClient, FontAttributes = FontAttributes.Bold }
            }
        };
        phone.Text = $"{phoneRegistered}"; 
        methodOfPayment.Text = $"{methodOfPaymentSelected}";
        email.Text = $"{emailEntered}";
        loanCount.Text = $"{loanClientCount}";
    }
    
    void OnOKButtonClicked(object? sender, EventArgs e) => Close();
}