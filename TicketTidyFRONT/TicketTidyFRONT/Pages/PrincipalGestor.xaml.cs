namespace TicketTidyFRONT.Pages;

public partial class PrincipalGestor : FlyoutPage
{
	public PrincipalGestor()
	{
		InitializeComponent();
        App.Navigate = Navigate;
        App.MenuGestor = this;
    }
}