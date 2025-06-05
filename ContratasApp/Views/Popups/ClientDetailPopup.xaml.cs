using CommunityToolkit.Maui.Views;

namespace ContratasApp.Views.Popups;

public partial class ClientDetailPopup
{
    public ClientDetailPopup()
    {
        InitializeComponent(); // ⚠️ Esto es crucial
    }

    private void Cerrar_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}