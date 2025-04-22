using System.Collections.ObjectModel;
using APIBuenaTicketing.Models;
using TicketTidyFRONT.ClasesBinding;
using TicketTidyFRONT.Generic;

namespace TicketTidyFRONT.Pages.AccionesTecnico;

public partial class IncidenciasPorFechaTecnico : ContentPage
{

    //incidenciaFront.FechaCierre = DateOnly.FromDateTime(pickerFecha.Date);
    public IncidenciasAsignadasViewModel ViewModel { get; set; }

    public async Task CargarIncidencias()
    {


        try
        {

            ViewModel.Incidencias = new ObservableCollection<Incidencia>();
            
            //string tipo = pickerTipoIncidencia.SelectedItem?.ToString();
            DateOnly fechaIni = DateOnly.FromDateTime(pickerFechaIncio.Date);
            DateOnly fechaFin = DateOnly.FromDateTime(pickerFechaFin.Date);

            string fecha1 = fechaIni.ToString("yyyy-MM-dd");
            string fecha2 = fechaFin.ToString("yyyy-MM-dd");

            var a = 2;

            var incidenciasHTTP = await HTTPHelper.GetAll<Incidencia>(
            "http://tickettidy.somee.com/getIncidenciasByFechaApertura/" + fecha1 + "/" + fecha2);

            var i = 2;

            foreach (var item in incidenciasHTTP)
            {
                ViewModel.Incidencias.Add(item);
            }

            if (incidenciasHTTP.Count() == 0)
            {
                DisplayAlert("Aviso", "No hay incidencias iniciadas en las fechas seleccionadas", "Ok");
                listaBorder.IsVisible = false;
            }
            
            


        }
        catch (Exception ex)
        {
            // En caso de que haya un error, lo manejamos
            await DisplayAlert("Error", "No se pudieron cargar las incidencias", "OK");
        }


    }
    public IncidenciasPorFechaTecnico()
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
        listaBorder.IsVisible = true;
        ViewModel.loading = false;
    }

    private void pickerDesplegar_Tapped_1(object sender, TappedEventArgs e)
    {

    }
}