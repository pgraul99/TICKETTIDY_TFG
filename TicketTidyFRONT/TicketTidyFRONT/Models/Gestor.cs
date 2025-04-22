using System;
using System.Collections.Generic;

namespace APIBuenaTicketing.Models;

public partial class Gestor
{
    public long Id { get; set; }

    public string? Contraseña { get; set; }

    public string? Email { get; set; }

    public string? NombreUsuario { get; set; }

    public string? Telefono { get; set; }

}
