using System.Collections.ObjectModel;
using APIBuenaTicketing.Models;
using TicketTidyFRONT.ClasesBinding;
using TicketTidyFRONT.Generic;

namespace TicketTidyFRONT.Pages.AccionesGestor;

public partial class CrearIncidencias : ContentPage
{
	public IncidenciasAsignadasViewModel ViewModel { get; set; }

    public Incidencia incidencia { get; set; }
	public CrearIncidencias()
	{
		InitializeComponent();
		ViewModel = new IncidenciasAsignadasViewModel();
		ViewModel.fecha = DateTime.Now.ToString("dd/MM/yyyy");
		ViewModel.image = "crono_icon";
        _= getDispositivos();
        _= getEspacios();
        BindingContext = ViewModel;

    }

    public async Task getDispositivos()
    {
        try
        {
            ViewModel.Dispositivos = new ObservableCollection<Dispositivo>();

            var dispositivos = await HTTPHelper.GetAll<Dispositivo>("http://tickettidy.somee.com/getDispositivos");
            

            foreach (var dispositivo in dispositivos)
            {
                ViewModel.Dispositivos.Add(dispositivo);
                
            }

            ViewModel.DispositivoSeleccionado = ViewModel.Dispositivos[0];
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al cargar los dispositivos", "OK");
        }
    }

    public async Task getEspacios()
    {
        try
        {
            ViewModel.Espacios = new ObservableCollection<Espacio>();

            var espacios = await HTTPHelper.GetAll<Espacio>("http://tickettidy.somee.com/getEspacios");
            foreach (var espacio in espacios)
            {
                ViewModel.Espacios.Add(espacio);
            }

            ViewModel.EspacioSeleccionado = ViewModel.Espacios[0];
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al cargar los dispositivos", "OK");
        }
    }
    private async void backBtn_Clicked(object sender, EventArgs e)
    {

        await Navigation.PopAsync();
    }

    private async void saveBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            incidencia = new Incidencia();

            if (string.IsNullOrEmpty(descripcionEntry.Text))
            {
                DisplayAlert("Error", "Tienes que escribir una breve descripción de la incidencia", "Volver");
                return;
            }
            else
            {
                
                incidencia.FechaApertura = DateOnly.FromDateTime(pickerFecha.Date);
                incidencia.Estado = "Alta";
                incidencia.DescripcionIncidencia = descripcionEntry.Text;
                incidencia.TecnicoId = null;
                incidencia.GestorId = null;
                incidencia.UbasicoId = null;
                incidencia.FechaCierre = null;
                incidencia.DescripcionSolucion = null;
                incidencia.TipoIncidencia = null;
                Dispositivo dis = (Dispositivo)pickerDispositivos.SelectedItem;
                incidencia.DispositivoId = dis.Id;
                Espacio es = (Espacio)pickerEspacios.SelectedItem;
                incidencia.EspacioId = es.Id;
                bool confirmacion = await DisplayAlert("Aviso", "¿Estás seguro que quieres cerrar la incidencia con estos datos?", "Sí", "No");

                if (!confirmacion)
                {
                    // Si el usuario selecciona "No", salimos del método
                    return;
                }

                var response = await HTTPHelper.Post<Incidencia>(
                "http://tickettidy.somee.com/saveIncidenciaNueva", incidencia
                );

                if (response == 1)
                {
                    await DisplayAlert("Éxito", "Incidencia creada correctamente", "OK");
                    await Navigation.PopAsync(); // Usamos await aquí también
                }
                else
                {
                    await DisplayAlert("Error", "No se ha podido guardar la incidencia", "Volver");
                }

                    var i = 2;
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "No se ha podido guardar", "Volver");
        }

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
}