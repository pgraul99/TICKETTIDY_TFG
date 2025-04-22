using APIBuenaTicketing.Models;
using TicketTidyFRONT.ClasesBinding;
using TicketTidyFRONT.Generic;

namespace TicketTidyFRONT.Pages;

public partial class LoginPage : ContentPage
{
    public IncidenciasAsignadasViewModel viewModel { get; set; }
	public LoginPage()
	{
		InitializeComponent();
        viewModel = new IncidenciasAsignadasViewModel();
        BindingContext = viewModel;
	}

    private async void logButton_Clicked(object sender, EventArgs e)
    {

        try
        {
            viewModel.loading = true;
            var user = usernameEntry.Text;
            var psw = pswEntry.Text;

            Tecnico tecnico = await HTTPHelper.Get<Tecnico>("http://tickettidy.somee.com/loginTecnico/" + user + "/" + psw);
            Gestor gestor = await HTTPHelper.Get<Gestor>("http://tickettidy.somee.com/loginGestor/" + user + "/" + psw);
            Administrador admin = await HTTPHelper.Get<Administrador>("http://tickettidy.somee.com/loginAdmin/" + user + "/" + psw);
            UsuarioBasico basico = await HTTPHelper.Get<UsuarioBasico>("http://tickettidy.somee.com/loginBasico/" + user + "/" + psw);

            long idTecnico = tecnico.Id;
            long idGestor = gestor.Id;
            long idAdmin = admin.Id;
            long idBasico = basico.Id;

            if (tecnico.Id != 0)
            {
                Preferences.Set("perfil", "tecnico");
                Preferences.Set("idTecnico", idTecnico);

                App.Current.MainPage = new PrincipalTecnico();


                var a = 2;
            }
            else if (gestor.Id != 0)
            {
                Preferences.Set("perfil", "gestor");
                Preferences.Set("idGestor", idGestor);

                App.Current.MainPage = new PrincipalGestor();

            }
            else if (admin.Id != 0)
            {
                Preferences.Set("perfil", "admin");
                Preferences.Set("idAdmin", idAdmin);
                App.Current.MainPage = new MenuAdmin(admin);

            }
            else if (basico.Id != 0)
            {
                Preferences.Set("perfil", "basico");
                Preferences.Set("idBasico", idBasico);
                App.Current.MainPage = new MenuBasico(basico);

            }

            else
            {
                DisplayAlert("AVISO", "Usuario / contraseña incorrectas", "VOLVER");
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("AVISO", "Ocurrió un error en la app", "VOLVER");
        }
        finally
        {
            viewModel.loading = false;
        }
    }
}