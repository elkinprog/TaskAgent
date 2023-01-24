using System.Net;

namespace WebApi.Responses
{
    public class ListModelResponse<T> : GenericResponse
    {
        public IEnumerable<T> models { get; set; }

        public ListModelResponse(HttpStatusCode codigo, string titulo, string mensaje, IEnumerable<T> models) : base(codigo, titulo, mensaje)
        {
            this.models = models;
        }
    }

}
