using APIBuenaTicketing.Models;
using TicketTidyFRONT.Clases;
using TicketTidyFRONT.Generic;
using TicketTidyFRONT.Pages.AccionesGestor;
using TicketTidyFRONT.Pages.Auxiliares;

namespace TicketTidyFRONT.Pages;

public partial class MenuGestor : ContentPage
{

    public List<MenuTecnicoCLS> listaMenu { get; set; }

    public Gestor gestor { get; set; }
    public MenuGestor()
	{
		InitializeComponent();
        listaMenu = new List<MenuTecnicoCLS>()
        {
            new MenuTecnicoCLS { id = 2, nombre = "Crear incidencias", icono = "resolver_icon" },
            new MenuTecnicoCLS { id = 3, nombre = "Asignar incidencias", icono = "resolver_icon" },
            new MenuTecnicoCLS { id = 5, nombre = "Ver incidencias por tipo", icono = "incidencia_icono" },
            new MenuTecnicoCLS { id = 4, nombre = "Ver incidencias por técnico", icono = "incidencia_icono" },
            new MenuTecnicoCLS { id = 6, nombre = "Cerrar sesión", icono = "salida" }
        };
        getGestor();
        BindingContext = this;
    }

    private async void getGestor()
    {
        try
        {

            int id = Convert.ToInt32(Preferences.Get("idGestor", (long)0));
            gestor = await HTTPHelper.Get<Gestor>("http://tickettidy.somee.com/getGestor/{id}?idGestor=" + id);
            lblId.Text = "Id gestor: " + gestor.Id.ToString();
            lblNombre.Text = "Nombre gestor: " + gestor.NombreUsuario;
            lblEmail.Text = "Email: " + gestor.Email;
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

            if (id == 2)
            {
                await Navigation.PushModalAsync(new LoadingPage());
                await Task.Delay(100);
                App.Navigate.PushAsync(new CrearIncidencias());
                await Navigation.PopModalAsync();
            }

            if (id == 3)
            {
                await Navigation.PushModalAsync(new LoadingPage());
                await Task.Delay(100);
                App.Navigate.PushAsync(new AsignarIncidencias());
                await Navigation.PopModalAsync();
            }
            if (id == 4)
            {
                await Navigation.PushModalAsync(new LoadingPage());
                await Task.Delay(100);
                await App.Navigate.PushAsync(new IncidenciasByTecnico());
                await Navigation.PopModalAsync();

                //App.Navigate.PushAsync(new IncidenciasAsignadasTecnico());
            }
            if (id == 5)
            {
                await Navigation.PushModalAsync(new LoadingPage());
                await Task.Delay(100);
                App.Navigate.PushAsync(new IncidenciasByTipoGestor());
                await Navigation.PopModalAsync();
            }

            if (id == 6)
            {
                App.Current.MainPage = new LoginPage();
                Preferences.Remove("gestor");
                Preferences.Remove("idGestor");
            }

            App.MenuGestor.IsPresented = false;
        }


    }
}