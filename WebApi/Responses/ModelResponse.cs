using System.Net;

namespace WebApi.Responses
{
    public class ModelResponse<T> : GenericResponse
    {
        public T model { get; set; }

        public ModelResponse(HttpStatusCode codigo, string titulo, string mensaje, T model) : base(codigo, titulo, mensaje)
        {
            this.model = model;
        }
    }

}
