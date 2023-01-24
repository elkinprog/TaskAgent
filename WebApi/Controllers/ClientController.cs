using Aplicacion.ManejadorErrores;
using Aplicacion.Services;
using Dominio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi.Responses;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IGenericService<Client> _service;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IGenericService<Client> service, ILogger<ClientController> logger)
        {
           this._service= service;
            this._logger = logger;  
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> getAllClientes()
        {
            _logger.LogInformation("Se encontraron los clientes");
            var response = new { Titulo = "Bien Hecho!", Mensaje = "Se encontraron los Clientes", Codigo = HttpStatusCode.OK };
            IEnumerable<Client> ClientModel = null;
            if (!await _service.ExistsAsync(e => e.Id > 0))
            {
                _logger.LogWarning("No existen cliente");
                response = new { Titulo = "Algo salio mal", Mensaje = "No existen cliente", Codigo = HttpStatusCode.Accepted };
            }
            else
            {
                ClientModel = await _service.GetAsync();
            }
            var listModelResponse = new ListModelResponse<Client>(response.Codigo, response.Titulo, response.Mensaje, ClientModel);
            return StatusCode((int)listModelResponse.Codigo, listModelResponse);
        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<Client>> getClientesId(long Id)
        {
            var response = new { Titulo = "", Mensaje = "", Codigo = HttpStatusCode.Accepted };
            Client ClientModel = null;
            if (!await _service.ExistsAsync(e => e.Id > 0))
            {
                _logger.LogWarning("No existen clientes");
                response = new { Titulo = "Algo salio mal", Mensaje = "No existen clientes", Codigo = HttpStatusCode.BadRequest };
            }

            var client = await _service.GetAsync(e => e.Id == Id, e => e.OrderBy(e => e.Id), "");

            if (client.Count < 1)
            {
                _logger.LogWarning("No existe clientes con id con el id solicitado");
                response = new { Titulo = "Algo salio mal", Mensaje = "No existe clientes con id " + Id, Codigo = HttpStatusCode.NotFound };
            }
            else
            {
                _logger.LogInformation("Se obtuvo el cliente con el Id solicitado");
                ClientModel = client.First();
                response = new { Titulo = "Bien Hecho!", Mensaje = "Se obtuvo el cliente con el Id solicitado", Codigo = HttpStatusCode.OK };
            }
            var modelResponse = new ModelResponse<Client>(response.Codigo, response.Titulo, response.Mensaje, ClientModel);
            return StatusCode((int)modelResponse.Codigo, modelResponse);
        }

        [HttpPost]
        public async Task<IActionResult> InsertClientes([FromBody] Client client)
        {
            try
            {
                _logger.LogInformation("Cliente creado de forma correcta");
                var response = new { Titulo = "Bien Hecho!", Mensaje = "Cliente creado de forma correcta", Codigo = HttpStatusCode.Created };
                Client ClientModel = null;

                client.Date = DateTime.Now;
                bool guardo = await _service.CreateAsync(client);
                if (!guardo)
                {
                    _logger.LogError("No se puedo guardar el cliente");
                    response = new { Titulo = "Algo salio mal", Mensaje = "No se puedo guardar el cliente", Codigo = HttpStatusCode.BadRequest };
                }
                else
                {
                    ClientModel = client;
                }
                var modelResponse = new ModelResponse<Client>(response.Codigo, response.Titulo, response.Mensaje, ClientModel);
                return StatusCode((int)modelResponse.Codigo, modelResponse);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateClientes(long Id, Client client)
        {
            _logger.LogInformation("Se actualizó cliente de forma correcta");
            var response = new { Titulo = "Bien Hecho!", Mensaje = "Se actualizó cliente de forma correcta", Codigo = HttpStatusCode.OK };
            try
            {
                if (Id != client.Id)
                {
                    _logger.LogError("El id de cliente no corresponde con el modelo");
                    response = new { Titulo = "Algo salió mal!", Mensaje = "El id de cliente no corresponde con el modelo", Codigo = HttpStatusCode.BadRequest };
                }
                else if (client.Id < 1)
                {
                    _logger.LogError("El modelo de cliente no tiene el campo Id");
                    response = new { Titulo = "Algo salió mal!", Mensaje = "El modelo de cliente no tiene el campo Id ", Codigo = HttpStatusCode.BadRequest };
                }
                else
                {
                    var clientes = await _service.FindAsync(Id);

                    if (clientes == null)
                    {
                        _logger.LogError("No existe cliente con id ");
                        response = new { Titulo = "Algo salio mal", Mensaje = "No existe cliente con id " + Id, Codigo = HttpStatusCode.NotFound };
                    }
                    else
                    {
                        client.Date = DateTime.Now;
                        bool updated = await _service.UpdateAsync(Id, client);

                        if (!updated)
                        {
                            _logger.LogError("No fue posible actualizar el cliente");
                            response = new { Titulo = "Algo salió mal!", Mensaje = "No fue posible actualizar el cliente", Codigo = HttpStatusCode.NoContent };
                        }
                    }
                }
            }
            catch (ExcepcionError)
            {
                _logger.LogError("Ocurrio una excepción interna");
                response = new { Titulo = "Algo salió mal!", Mensaje = "Ocurrio una excepción interna", Codigo = HttpStatusCode.InternalServerError};
                var error = new ExcepcionError(response.Codigo, response.Titulo, response.Mensaje);
                return StatusCode((int)error.Codigo, error);
            }
            var updateResponse = new GenericResponse(response.Codigo, response.Titulo, response.Mensaje);
            return StatusCode((int)updateResponse.Codigo, updateResponse);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteClientes(long Id)
        {
            _logger.LogInformation("Se eliminó el cliente de forma correcta");
            var response = new { Titulo = "Bien Hecho!", Mensaje = "Se eliminó el cliente de forma correcta", Codigo = HttpStatusCode.OK };
            var cliente = await _service.FindAsync(Id);

            if (cliente == null)
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existe cliente con id " + Id, Codigo = HttpStatusCode.NotFound };
            }
            else
            {
                bool elimino = await _service.DeleteAsync(Id);
                if (!elimino)
                {
                    _logger.LogError("No se pudo eliminar el cliente con Id");
                    response = new { Titulo = "Algo salió mal!", Mensaje = "No se pudo eliminar el cliente con Id" + Id, Codigo = HttpStatusCode.NoContent };
                }
            }
            var updateResponse = new GenericResponse(response.Codigo, response.Titulo, response.Mensaje);
            return StatusCode((int)updateResponse.Codigo, updateResponse);
        }



    }
}
