using System.Net;

namespace Aplicacion.ManejadorErrores
{
    public class ExcepcionError : Exception
    {

        public HttpStatusCode Codigo { get; }
        public string Titulo;
        public string Mensaje;
        public ExcepcionError(HttpStatusCode codigo, string titulo, string mensaje)
        {
            this.Codigo  = codigo;
            this.Titulo  = titulo;
            this.Mensaje = mensaje;
        }
    }
}
