using APIBuenaTicketing.Models;

namespace TicketTidyFRONT.Pages;

public partial class PrincipalTecnico : FlyoutPage
{
	public PrincipalTecnico()
	{
		InitializeComponent();
        App.Navigate = Navigate;
        App.Menu = this;
    }

    
}