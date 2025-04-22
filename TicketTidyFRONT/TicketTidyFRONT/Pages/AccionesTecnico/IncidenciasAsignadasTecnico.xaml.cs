using System.Collections.ObjectModel;
using System.ComponentModel;
using APIBuenaTicketing.Models;
using TicketTidyFRONT.ClasesBinding;
using TicketTidyFRONT.Generic;
using TicketTidyFRONT.Pages.Auxiliares;

namespace TicketTidyFRONT.Pages.AccionesTecnico;

public partial class IncidenciasAsignadasTecnico : ContentPage
{
	public IncidenciasAsignadasViewModel ViewModel { get; set; }
    public async Task CargarIncidencias()
    {
        

        try
        {
            ViewModel.Incidencias = new ObservableCollection<Incidencia>();
            ViewModel.fecha = DateTime.Now.ToString("dd-MM-yyyy");
            ViewModel.cargando = true;
            int idTecnico = Convert.ToInt32(Preferences.Get("idTecnico", (long)0));
            var incidenciasHTTP = await HTTPHelper.GetAll<Incidencia>(
                "http://tickettidy.somee.com/getIncidenciasByTecnico/{id}?idtecnico=" + idTecnico
            );

            foreach (var item in incidenciasHTTP)
            {
                ViewModel.Incidencias.Add(item);
            }

            
            var a = 2;
        }
        catch (Exception ex)
        {
            // En caso de que haya un error, lo manejamos
            await DisplayAlert("Error", "No se pudieron cargar las incidencias", "OK");
        }
        finally
        {
            ViewModel.cargando = false;
        }


    }
    public IncidenciasAsignadasTecnico()
	{
		InitializeComponent();
        ViewModel = new IncidenciasAsignadasViewModel();
        ViewModel.image = "crono_icon";
        ViewModel.incidenciaIcono = "incidencia_icono_listas";
        _=CargarIncidencias();
        

        BindingContext = ViewModel;
    }

    private void viajarIncidencia_Clicked(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int id = Convert.ToInt32(btn.CommandParameter);
        Preferences.Set("idIncidencia", id);
        App.Navigate.PushAsync(new CerrarIncidenciasTecnico());
        var a = 2;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (infoLabel.IsVisible)
        {
            infoLabel.IsVisible = false;
        }
        else
        {
            infoLabel.IsVisible = true;
        }
    }

    private async void asignadasRefreshing_Refreshing(object sender, EventArgs e)
    {
        await CargarIncidencias();
    }
}