using System.Collections.ObjectModel;
using APIBuenaTicketing.Models;
using TicketTidyFRONT.ClasesBinding;
using TicketTidyFRONT.Generic;

namespace TicketTidyFRONT.Pages.AccionesGestor;

public partial class AccionAsignarIncidencia : ContentPage
{

    public IncidenciasAsignadasViewModel ViewModel { get; set; }
    public AccionAsignarIncidencia()
	{
        InitializeComponent();
        ViewModel = new IncidenciasAsignadasViewModel();
        ViewModel.fecha = DateTime.Now.ToString("dd/MM/yyyy");
        ViewModel.image = "crono_icon";
        _ = getGestores();
        _ = getTecnicos();
        _ = getUbasicos();
        _ = getDispositivos();
        _ = getEspacios();

        BindingContext = ViewModel;
    }

    public async Task getDispositivos()
    {
        try
        {
            ViewModel.Dispositivos = new ObservableCollection<Dispositivo>();

            int idIncidencia = Convert.ToInt32(Preferences.Get("idIncidencia", 0));
            var incidencia = await HTTPHelper.Get<Incidencia>(
                $"http://tickettidy.somee.com/getIncidenciasById/{idIncidencia}?idIncidencia=" + (long)idIncidencia
            );

            long idDis = (long)incidencia.DispositivoId;
            //cambiar url
            var dispositivos = await HTTPHelper.Get<Dispositivo>("http://tickettidy.somee.com/getDispositivoById/{id}?idDis="
                + idDis);

            ViewModel.DispositivoSeleccionado = dispositivos;
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

            int idIncidencia = Convert.ToInt32(Preferences.Get("idIncidencia", 0));
            var incidencia = await HTTPHelper.Get<Incidencia>(
                $"http://tickettidy.somee.com/getIncidenciasById/{idIncidencia}?idIncidencia=" + (long)idIncidencia
            );

            

            long idEsp = (long) incidencia.EspacioId;

            //cambiar url
            var espacios = await HTTPHelper.Get<Espacio>("http://tickettidy.somee.com/getEspacioById/{id}?idEsp="
                + idEsp);

            ViewModel.EspacioSeleccionado = espacios;

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al cargar los dispositivos", "OK");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing(); // Esto ahora es válido

        try
        {
            int idIncidencia = Convert.ToInt32(Preferences.Get("idIncidencia", 0));
            var incidencia = await HTTPHelper.Get<Incidencia>(
                $"http://tickettidy.somee.com/getIncidenciasById/{idIncidencia}?idIncidencia=" + (long)idIncidencia
            );
            ViewModel.Incidencia = incidencia;

            // Forzar actualización en la UI si es necesario

            if (ViewModel.Incidencia == null)
            {
                await DisplayAlert("Error", "No se ha encontrado la incidencia", "OK");
                return;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se ha encontrado la incidencia", "OK");
        }
    }

    private async void backBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    public async Task getGestores()
    {
        try
        {
            ViewModel.Gestores = new ObservableCollection<Gestor>();

            var gestores = await HTTPHelper.GetAll<Gestor>("http://tickettidy.somee.com/getGestores");


            foreach (var gestor in gestores)
            {
                ViewModel.Gestores.Add(gestor);

            }

            ViewModel.GestorSeleccionado = ViewModel.Gestores[0];
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al cargar los gestores", "OK");
        }
    }

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

    public async Task getUbasicos()
    {
        try
        {
            ViewModel.UsuariosBasicos = new ObservableCollection<UsuarioBasico>();

            var basicos = await HTTPHelper.GetAll<UsuarioBasico>("http://tickettidy.somee.com/getBasicos");


            foreach (var basico in basicos)
            {
                ViewModel.UsuariosBasicos.Add(basico);

            }

            ViewModel.UsuarioSeleccionado = ViewModel.UsuariosBasicos[0];
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al cargar los usuarios básicos", "OK");
        }
    }

    private async void saveBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            ViewModel.loading = true; // Cambiamos a true al iniciar el proceso
            asignarIncidencia();
            ViewModel.loading = false;

            bool confirmacion = await DisplayAlert("Aviso", "¿Estás seguro que quieres asignar la incidencia con estos datos?", "Sí", "No");

            if (!confirmacion)
            {
                // Si el usuario selecciona "No", salimos del método
                return;
            }

            var response = await HTTPHelper.Post<Incidencia>(
            "http://tickettidy.somee.com/saveIncidencia", ViewModel.Incidencia
            );

            if (response == 1)
            {
                await DisplayAlert("Éxito", "Incidencia asignada correctamente", "OK");
                await Navigation.PopAsync(); // Usamos await aquí también
            }


        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo cerrar la incidencia", "OK");
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

    public async void asignarIncidencia()
    {
        try
        {
            ViewModel.Incidencia.Estado = "Asignada";
            var tecnicoSeleccionado = pickerTecnicos.SelectedItem as Tecnico;
            var gestorSeleccionado = pickerGestores.SelectedItem as Gestor;
            var ubasicoSeleccionado = pickerBasicos.SelectedItem as UsuarioBasico;

            string tecnicoName = tecnicoSeleccionado.NombreUsuario;
            string gestorName = gestorSeleccionado.NombreUsuario;
            string ubasicoName = ubasicoSeleccionado.NombreUsuario;

            int idTecnico = await HTTPHelper.GetInt("http://tickettidy.somee.com/getTecnicoByName/" + tecnicoName);
            int idGestor = await HTTPHelper.GetInt("http://tickettidy.somee.com/getGestorByName/" + gestorName);
            int idBasico = await HTTPHelper.GetInt("http://tickettidy.somee.com/getBasicoByName/" + ubasicoName);

            ViewModel.Incidencia.TecnicoId = idTecnico;
            ViewModel.Incidencia.GestorId = idGestor;
            ViewModel.Incidencia.UbasicoId = idBasico;


        }
        catch (Exception e)
        {
            DisplayAlert("Error", "Ha ocurrido un error con la asignación de la incidencia" , "Cancelar");
        }
    }
}