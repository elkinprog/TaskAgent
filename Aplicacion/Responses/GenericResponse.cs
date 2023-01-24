using System.Net;

namespace WebApi.Responses
{
    public class GenericResponse
    {

        public HttpStatusCode Codigo { get; set; }


        public string Titulo { get; set; }
        public string Mensaje { get; set; }

        public GenericResponse(HttpStatusCode codigo, string titulo, string mensaje)
        {
            this.Codigo = codigo;
            this.Titulo = titulo;
            this.Mensaje = mensaje;
        }


    }
}
