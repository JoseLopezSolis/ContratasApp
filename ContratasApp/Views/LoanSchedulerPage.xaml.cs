using ContratasApp.Models;

namespace ContratasApp.Views;

public partial class LoanSchedulerPage
{
    public LoanSchedulerPage() : base()
    {
        InitializeComponent();
    }
    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        bool isChecked = e.Value;
        var payment = ((CheckBox)sender).BindingContext as Payment;
        Console.WriteLine($"Payment {payment?.Id} marcado como pagado: {isChecked}");
    }
}