using System;
using System.Collections.Generic;

namespace APIBuenaTicketing.Models;

public partial class Dispositivo
{
    public long Id { get; set; }

    public string? Descripcion { get; set; }

    public string? Marca { get; set; }

    public string? Modelo { get; set; }

    public string? Tipo { get; set; }
}
