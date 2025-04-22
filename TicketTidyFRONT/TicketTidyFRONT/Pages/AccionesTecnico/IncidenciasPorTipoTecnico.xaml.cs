using System.Collections.ObjectModel;
using APIBuenaTicketing.Models;
using TicketTidyFRONT.ClasesBinding;
using TicketTidyFRONT.Generic;

namespace TicketTidyFRONT.Pages.AccionesTecnico;

public partial class IncidenciasPorTipoTecnico : ContentPage
{

    public IncidenciasAsignadasViewModel ViewModel { get; set; }
    public async Task CargarIncidencias()
    {


        try
        {

            ViewModel.Incidencias = new ObservableCollection<Incidencia>();
            string tipo = pickerTipoIncidencia.SelectedItem?.ToString();
            if (!string.IsNullOrWhiteSpace(tipo))
            {
                var incidenciasHTTP = await HTTPHelper.GetAll<Incidencia>(
                "http://tickettidy.somee.com/getIncidenciasByTipoTecnico/" + tipo
            );

                foreach (var item in incidenciasHTTP)
                {
                    ViewModel.Incidencias.Add(item);
                }

                if (incidenciasHTTP.Count()==0)
                {
                    DisplayAlert("Aviso", "No hay incidencias de ese tipo", "Ok");
                }
            }
            else
            {
                DisplayAlert("Aviso", "Necesitas elegir un valor de los posibles", "Ok");
            }

            
        }
        catch (Exception ex)
        {
            // En caso de que haya un error, lo manejamos
            await DisplayAlert("Error", "No se pudieron cargar las incidencias", "OK");
        }


    }
    public IncidenciasPorTipoTecnico()
	{
		InitializeComponent();
        ViewModel = new IncidenciasAsignadasViewModel();
        ViewModel.image = "crono_icon";
        ViewModel.fecha = DateTime.Now.ToString("dd-MM-yyyy");
        ViewModel.incidenciaIcono = "incidencia_icono_listas";
        BindingContext = ViewModel;
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

    private void pickerDesplegar_Tapped(object sender, TappedEventArgs e)
    {

    }

    private async void buscarBtn_Clicked(object sender, EventArgs e)
    {
        ViewModel.loading = true;
        await CargarIncidencias();
        ViewModel.loading = false;
    }
}