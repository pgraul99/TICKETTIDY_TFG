using System.Threading.Tasks;
using APIBuenaTicketing.Models;
using TicketTidyFRONT.Generic;

namespace TicketTidyFRONT.Pages.AccionesTecnico;

public partial class CerrarIncidenciasTecnico : ContentPage
{
	public string fecha { get; set; }
    public Incidencia incidenciaFront { get; set; }

    public long idIncidenciaFront { get; set; }

    public DateOnly? FechaApertura { get; set; }

    public long dispositivoId { get; set; }

    public long espacioId { get; set; }

    public long gestorId { get; set; }

    public long tecnicoId { get; set; }

    public long ubasicoId { get; set; }

    public string? descripcionIncidencia { get; set; }

    public string? descripcionSolucion { get; set; }

    public string? tipoIncidencia { get; set; }

    public string? estado { get; set; }

    public CerrarIncidenciasTecnico()
	{
		InitializeComponent();
		fecha = DateTime.Now.ToString("dd-MM-yyyy");
        BindingContext = this;
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
            incidenciaFront = incidencia;

            // Forzar actualización en la UI si es necesario
            OnPropertyChanged(nameof(incidenciaFront));

            if (incidenciaFront != null)
            {
                idIncidenciaFront = incidenciaFront.Id;
                FechaApertura = incidenciaFront.FechaApertura;
                dispositivoId = (long)incidenciaFront.DispositivoId;
                espacioId = (long)incidenciaFront.EspacioId;
                gestorId = (long)incidenciaFront.GestorId;
                tecnicoId = (long)incidenciaFront.TecnicoId;
                ubasicoId = (long)incidenciaFront.UbasicoId;
                descripcionIncidencia = incidenciaFront.DescripcionIncidencia;
                tipoIncidencia = incidenciaFront.TipoIncidencia;
                estado = incidenciaFront.Estado;
                var a = 2;
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

    private async void saveBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            incidenciaFront.DescripcionSolucion = lblSolucion.Text;
            incidenciaFront.FechaCierre = DateOnly.FromDateTime(pickerFecha.Date);
            incidenciaFront.Estado = pickerCierre.SelectedItem?.ToString();
            incidenciaFront.TipoIncidencia = pickerTipoIncidencia.SelectedItem?.ToString();

            bool confirmacion = await DisplayAlert("Aviso", "¿Estás seguro que quieres cerrar la incidencia con estos datos?", "Sí", "No");

            if (!confirmacion)
            {
                // Si el usuario selecciona "No", salimos del método
                return;
            }

            var response = await HTTPHelper.Post<Incidencia>(
            "http://tickettidy.somee.com/saveIncidencia", incidenciaFront
            );

            if (response != null)
            {
                await DisplayAlert("Éxito", "Incidencia cerrada correctamente", "OK");
                await Navigation.PopAsync(); // Usamos await aquí también
            }


        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo cerrar la incidencia", "OK");
        }

    }
}