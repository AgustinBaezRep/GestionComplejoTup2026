using System;
using System.Collections.Generic;
using System.Text;

namespace GestionComplejo.Application.Requests
{
    public class CanchaRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Deporte { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public double Precio { get; set; }
    }
}
