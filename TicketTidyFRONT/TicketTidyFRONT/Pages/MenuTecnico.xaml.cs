using APIBuenaTicketing.Models;
using TicketTidyFRONT.Clases;
using TicketTidyFRONT.Generic;
using TicketTidyFRONT.Pages.AccionesTecnico;
using TicketTidyFRONT.Pages.Auxiliares;

namespace TicketTidyFRONT.Pages;

public partial class MenuTecnico : ContentPage
{

    //resolver y cerrar incidencias
    //agregar tipos incidencia
    //ver incidencias cerradas en tiempo
    //ver incidencias asignadas
    //ver incidencias por tipo

    public List<MenuTecnicoCLS> listaMenu { get; set; }

    public Tecnico tecnico { get; set; }
    public MenuTecnico()
	{
		InitializeComponent();
        listaMenu = new List<MenuTecnicoCLS>()
        {
            
            new MenuTecnicoCLS { id = 3, nombre = "Ver incidencias por fechas", icono = "incidencia_icono" },
            new MenuTecnicoCLS { id = 5, nombre = "Ver incidencias por tipo", icono = "incidencia_icono" },
            new MenuTecnicoCLS { id = 4, nombre = "Ver incidencias asignadas", icono = "incidencia_icono" },
            new MenuTecnicoCLS { id = 6, nombre = "Cerrar sesión", icono = "salida" }
        };
        getTecnico();
        BindingContext = this;
	}

    
    public async void getTecnico()
    {
        try
        {
            
            int id = Convert.ToInt32(Preferences.Get("idTecnico", (long)0));
            tecnico = await HTTPHelper.Get<Tecnico>("http://tickettidy.somee.com/getTecnico/{id}?idTecnico=" + id);
            lblId.Text = "Id técnico: " + tecnico.Id.ToString();
            lblNombre.Text = "Nombre técnico: " + tecnico.NombreUsuario;
            lblEmail.Text = "Email: " + tecnico.Email;
            var a = 2;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private async void lstMenu_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item == null)
            return;


        else
        {
            int id = ((MenuTecnicoCLS)e.Item).id;
            
            
            if (id == 3)
            {
                await Navigation.PushModalAsync(new LoadingPage());
                await Task.Delay(100);
                App.Navigate.PushAsync(new IncidenciasPorFechaTecnico());
                await Navigation.PopModalAsync();
            }
            if(id == 4)
            {
                await Navigation.PushModalAsync(new LoadingPage());
                await Task.Delay(100);
                await App.Navigate.PushAsync(new IncidenciasAsignadasTecnico());
                await Navigation.PopModalAsync();

                //App.Navigate.PushAsync(new IncidenciasAsignadasTecnico());
            }
            if (id == 5)
            {
                await Navigation.PushModalAsync(new LoadingPage());
                await Task.Delay(100);
                App.Navigate.PushAsync(new IncidenciasPorTipoTecnico());
                await Navigation.PopModalAsync();
            }

            if (id == 6)
            {
                App.Current.MainPage = new LoginPage();
                Preferences.Remove("perfil");
                Preferences.Remove("idTecnico");
            }

            App.Menu.IsPresented = false;
        }


    }
}