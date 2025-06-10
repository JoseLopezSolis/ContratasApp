using CommunityToolkit.Maui.Views;
using ContratasApp.Models;

namespace ContratasApp.Views.Popups;

public partial class ClientDetailPopup : Popup
{
    public ClientDetailPopup()
    {
        InitializeComponent();
        BindingContext = this; // Importante para que funcione el binding a BindableProperty
    }

    public static readonly BindableProperty ClientProperty =
        BindableProperty.Create(
            nameof(Client),
            typeof(Client),
            typeof(ClientDetailPopup),
            propertyChanged: OnClientChanged);

    public Client Client
    {
        get => (Client)GetValue(ClientProperty);
        set => SetValue(ClientProperty, value);
    }

    private static void OnClientChanged(BindableObject bindable, object oldValue, object newValue)
    {
        // Aquí puedes manejar lógica si necesitas reaccionar a cambios
    }
}
    // public ClientDetailPopup(string  nameClient, string phoneRegistered, string methodOfPaymentSelected, string emailEntered, int loanClientCount)
    // {
    //     InitializeComponent();
    //     welcomeMessage.Text = $"Detalles sobre \n {nameClient}";
    //     phone.Text = $"{phoneRegistered}"; 
    //     methodOfPayment.Text = $"{methodOfPaymentSelected}";
    //     email.Text = $"{emailEntered}";
    //     loanCount.Text = $"{loanClientCount}";
    // }
    //
    // void OnOKButtonClicked(object? sender, EventArgs e) => Close();
