using System.Collections.ObjectModel;
using APIBuenaTicketing.Models;
using TicketTidyFRONT.ClasesBinding;
using TicketTidyFRONT.Generic;

namespace TicketTidyFRONT.Pages.AccionesGestor;

public partial class IncidenciasByTecnico : ContentPage
{
    public IncidenciasAsignadasViewModel ViewModel { get; set; }

    public async Task getTecnicos()
    {
        try
        {
            ViewModel.Tecnicos = new ObservableCollection<Tecnico>();

            var tecnicos = await HTTPHelper.GetAll<Tecnico>("http://tickettidy.somee.com/getTecnicos");


            foreach (var tecnico in tecnicos)
            {
                ViewModel.Tecnicos.Add(tecnico);

            }

            ViewModel.TecnicoSeleccionado = ViewModel.Tecnicos[0];
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al cargar los técnicos", "OK");
        }
    }
    public async Task CargarIncidencias()
    {


        try
        {
            ViewModel.Incidencias = new ObservableCollection<Incidencia>();
            ViewModel.fecha = DateTime.Now.ToString("dd-MM-yyyy");
            ViewModel.cargando = true;
            var tecnico = pickerTecnicos.SelectedItem as Tecnico;
            int idTecnico = (int) tecnico.Id;
            var incidenciasHTTP = await HTTPHelper.GetAll<Incidencia>(
                "http://tickettidy.somee.com/getIncidenciasByTecnico/{id}?idtecnico=" + idTecnico
            );

            if (incidenciasHTTP.Count() == 0)
            {
                await DisplayAlert("Aviso", "No hay incidencias asigandas a este técnico", "Ok");
            }

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
    public IncidenciasByTecnico()
	{
		InitializeComponent();
        ViewModel = new IncidenciasAsignadasViewModel();
        ViewModel.fecha = DateTime.Now.ToString("dd-MM-yyyy");
        ViewModel.image = "crono_icon";
        _=getTecnicos();
        BindingContext = ViewModel;

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

    private async void verDetalleIncidencia_Clicked(object sender, EventArgs e)
    {
        try
        {
            Button btn = (Button)sender;
            int id = Convert.ToInt32(btn.CommandParameter);
            var incidencia = await HTTPHelper.Get<Incidencia>(
                $"http://tickettidy.somee.com/getIncidenciasById/{id}?idIncidencia=" + (long)id
            );

            if(incidencia != null)
            {
                DisplayAlert("Info",
                    "ID incidencia: " + incidencia.Id + "\n" +
                    "Fecha Apertura: " + incidencia.FechaApertura + "\n" +
                    "Descripcion: " + incidencia.DescripcionIncidencia + "\n" +
                    "Estado: " + incidencia.Estado + "\n" 
                    , "Volver");
            }
            

        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "No se pudo cargar la incidencia", "OK");
        }
    }
}