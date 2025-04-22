using TicketTidyFRONT.Pages.AccionesTecnico;

namespace TicketTidyFRONT
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("IncidenciasAsignadasTecnico", typeof(IncidenciasAsignadasTecnico));
        }
    }
}
